using PG.StorySystem;
using PG.StorySystem.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UIElements;
using PG.StorySystem.Graph;



//For Unity 6 and later
#if UNITY_6000_0_OR_NEWER
[UxmlElement]
#endif
public sealed partial class StoryGraphView : GraphView
{
#if !UNITY_6000_0_OR_NEWER
    public new class UxmlFactory : UxmlFactory<StoryGraphView, UxmlTraits> { }
#endif

    private StoryGraph _storyGraph;
    public StoryGraph storyGraph => _storyGraph;
    internal Action<StoryNodeView> onNodeSelected;

    public BaseGroupNode currentGroupNode => _currentGroupNode;

    [NonSerialized] private List<StoryNode> _temporaryNodes = new List<StoryNode>();
    private BaseGroupNode _currentGroupNode;


    private int _currentClicks = 0;

    private bool _saveAssetsScheduled = false;
    private ClipboardNodes _clipboardNodes = new ClipboardNodes();


    [System.Serializable]
    private class ClipboardNodes
    {
        public List<StoryNode> copiedNodes = new List<StoryNode>();
    }

    private event System.Action<StoryNode> _nodeCreated = default;

    private Vector2 _createNodeOffset = new Vector2(25, 40);
    private int _pasteIndex = 1;

    public StoryGraphEditorWindow window
    {
        get => _window;
        set { _window = value; if (_window != null) InitializeObjects(); }
    }
    private StoryGraphEditorWindow _window;
    private StoryGraphSearchWindow _searchWindow;

    public List<ToolbarButton> buttons = new List<ToolbarButton>();

    public StoryGraphView()
    {
        _storyGraph = StoryGraphEditorWindow.storyGraph;
        Insert(0, new GridBackground());

        var stylesheet = Resources.Load<StyleSheet>("StoryGraph/StoryGraphEditorWindowStyle");
        styleSheets.Add(stylesheet);

        nodeCreationRequest += OpenSearchWindow;

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        Undo.undoRedoPerformed += OnUndoRedo;
        InitializeObjects();

        CustomStoryNodeViewFactory.RegisterCustomNodeViews();

        serializeGraphElements += CutCopyOperation;
        unserializeAndPaste += PasteOperation;

        this.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);

        this.RegisterCallback<KeyDownEvent>(RegisterSave);
    }

    private void RegisterSave(KeyDownEvent evt)
    {
        if (evt != null && evt.keyCode == KeyCode.S && evt.ctrlKey)
        {
            SaveChanges();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }


    /// <summary>
    /// Возвращает центр экрана (вьюпорта) в координатах контента графа.
    /// </summary>
    private Vector2 GetContentViewportCenter()
    {
        // 1. Берём визуальные границы видимого окна GraphView
        var vp = viewport.worldBound;
        Vector2 vpCenter = new Vector2(vp.width * 0.5f, vp.height * 0.5f);

        // 2. Берём текущие пан и масштаб контента
        var t = contentViewContainer.resolvedStyle;
        Vector2 pan = new Vector2(t.translate.x, t.translate.y);
        Vector2 scale = new Vector2(t.scale.value.x, t.scale.value.y);

        // 3. Конвертируем координаты центра вьюпорта в координаты контента
        float sx = Mathf.Approximately(scale.x, 0f) ? 1f : scale.x;
        float sy = Mathf.Approximately(scale.y, 0f) ? 1f : scale.y;

        return new Vector2((vpCenter.x - pan.x) / sx, (vpCenter.y - pan.y) / sy);
    }




    #region Copy, Paste and Dublicate
    private void PasteOperation(string operationName, string data)
    {
        JsonUtility.FromJsonOverwrite(data, _clipboardNodes);

        var idMapping = new Dictionary<int, int>();
        var newNodes = new List<StoryNode>();

        // PERF: пакетируем добавление в ассет
        AssetDatabase.StartAssetEditing();
        try
        {
            for (int i = 0; i < _clipboardNodes.copiedNodes.Count; i++)
            {
                var originalNode = _clipboardNodes.copiedNodes[i];
                var newNode = ScriptableObject.Instantiate(originalNode);

                newNode.guid = GUID.Generate().ToString();
                newNode.nodePosition = GetContentViewportCenter() + _createNodeOffset * _pasteIndex;

                storyGraph.nextId++;
                int oldId = originalNode.id;
                int newId = storyGraph.nextId;
                newNode.id = newId;
                idMapping[oldId] = newId;

                newNode.childrenID = new List<int>();

                AssetDatabase.AddObjectToAsset(newNode, storyGraph);
                if (newNode is BaseGroupNode groupNode1)
                {
                    CopyGroup(groupNode1);
                }
                Undo.RegisterCreatedObjectUndo(newNode, "Story Graph (Copied Node)");

                _temporaryNodes.Add(newNode);
                newNodes.Add(newNode);
            }
        }
        finally
        {
            AssetDatabase.StopAssetEditing();
        }

        _pasteIndex++;


        foreach (var newNode in newNodes)
        {
            var originalNode = _clipboardNodes.copiedNodes
                .FirstOrDefault(node => node.id == idMapping.FirstOrDefault(pair => pair.Value == newNode.id).Key);
            if (originalNode != null)
                newNode.OnDublicateChildren(idMapping, originalNode);
        }


        foreach (var newNode in newNodes)
            CreateStoryNodeView(newNode, newNode.nodePosition);

        foreach (var parent in newNodes)
        {
            var parentView = FindNodeView(parent);
            if (parentView == null) continue;

            foreach (int childId in parent.childrenID)
            {
                var childNode = storyGraph.GetNodeByID(childId, _currentGroupNode);
                var childView = FindNodeView(childNode);
                if (childView != null && parentView.output != null && childView.input != null)
                {
                    var e = parentView.output.ConnectTo(childView.input);
                    AddElement(e);
                }
            }
        }

        SaveChanges();
    }

    private string CutCopyOperation(IEnumerable<GraphElement> elements)
    {
        _clipboardNodes.copiedNodes.Clear();
        _pasteIndex = 1;

        foreach (var element in elements)
        {
            if (element is StoryNodeView nodeView && nodeView.storyNode != null)
            {
                var clone = ScriptableObject.Instantiate(nodeView.storyNode);
                clone.guid = GUID.Generate().ToString();
                _clipboardNodes.copiedNodes.Add(clone);
            }
        }
        return JsonUtility.ToJson(_clipboardNodes);
    }

    void CopyGroup(BaseGroupNode groupNode)
    {
        var originalNodes = new List<StoryNode>(groupNode.storyNodes);
        var newNodes = new List<StoryNode>();
        var idMapping = new Dictionary<int, int>();

        foreach (StoryNode originalNode in originalNodes)
        {
            var newNode = ScriptableObject.Instantiate(originalNode);
            newNode.guid = GUID.Generate().ToString();

            storyGraph.nextId++;
            int oldId = originalNode.id;
            int newId = storyGraph.nextId;
            newNode.id = newId;
            idMapping[oldId] = newId;

            if (newNode is BaseGroupNode childGroupNode)
                CopyGroup(childGroupNode);

            newNode.childrenID = new List<int>();

            AssetDatabase.AddObjectToAsset(newNode, storyGraph);
            Undo.RegisterCreatedObjectUndo(newNode, "Story Graph (Copied Node)");

            newNodes.Add(newNode);
        }

        foreach (var newNode in newNodes)
        {
            var originalNode = originalNodes.FirstOrDefault(node => node.id == idMapping.FirstOrDefault(p => p.Value == newNode.id).Key);
            if (originalNode != null)
            {
                foreach (int oldChildId in originalNode.childrenID)
                    if (idMapping.TryGetValue(oldChildId, out int newChildId))
                        newNode.childrenID.Add(newChildId);
            }
        }

        groupNode.storyNodes.Clear();
        groupNode.storyNodes.AddRange(newNodes);

        Undo.RecordObject(storyGraph, "Story Graph (Copied Node)");
    }
    #endregion



    public void AddGroupLayer(BaseGroupNode targetGroupNode)
    {
        var layerButton = new ToolbarButton();
        layerButton.text = !string.IsNullOrWhiteSpace(targetGroupNode.nameGroup) ? targetGroupNode.nameGroup : targetGroupNode.GetName();
        layerButton.clicked += () => { _currentGroupNode = targetGroupNode; RemoveLayer(layerButton); };
        buttons.Add(layerButton);
        if (StoryGraphEditorWindow.toolbarBreadcrumbs != null)
            StoryGraphEditorWindow.toolbarBreadcrumbs.Add(layerButton);
    }

    public void RemoveLayer(ToolbarButton button)
    {
        for (int i = buttons.IndexOf(button) + 1; i < buttons.Count; i++)
        {
            StoryGraphEditorWindow.toolbarBreadcrumbs.Remove(buttons[i]);
            buttons.RemoveAt(i);
        }
        LoadGraph(_storyGraph, _currentGroupNode);
    }

    private void InitializeObjects()
    {
        if (window != null)
        {
            Button centerButton = window.root.Q<Button>("CenterButton");
            if (centerButton != null)
            {
                centerButton.clicked += () =>
                {
                    CenterOnCurrentLayer();
                };

            }
        }
    }
    private void CenterOnCurrentLayer()
    {
        StoryNode nodeToCenter = null;

        if (_currentGroupNode != null)
            nodeToCenter = _currentGroupNode.rootNode;
        else if (_storyGraph != null)
            nodeToCenter = _storyGraph.rootNode;

        // Если есть главная нода и у неё есть StoryNodeView — центрируемся по ней
        if (nodeToCenter != null && FindNodeView(nodeToCenter) != null)
        {
            CenterViewport(nodeToCenter.nodePosition);
        }
        else
        {
            // Иначе — в (0, 0)
            CenterViewport(Vector2.zero);
        }
    }
    public void CenterViewport(Vector2 contentPosition)
    {
        schedule.Execute(() =>
        {
            var vp = viewport.worldBound;
            Vector2 vpCenter = new Vector2(vp.width * 0.5f, vp.height * 0.5f);

            var s = contentViewContainer.resolvedStyle.scale;
            Vector2 scale = new Vector2(s.value.x, s.value.y);

            Vector2 pan = vpCenter - Vector2.Scale(contentPosition, scale);

            UpdateViewTransform(
                new Vector3(pan.x, pan.y, 0f),
                new Vector3(scale.x, scale.y, 1f)
            );
        }).ExecuteLater(0);
    }


    void OpenSearchWindow(NodeCreationContext context)
    {
        if (_searchWindow == null)
            _searchWindow = ScriptableObject.CreateInstance<StoryGraphSearchWindow>();

        _searchWindow.Initialize(this);
        SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    #region Interact with nodes
    private async void OnMouseDown(MouseDownEvent evt)
    {


        if (evt.button == 0 && evt.target is Edge edge)
        {
            _currentClicks++;
            if (_currentClicks == 2)
            {
                CreateSeparatorNode(edge);
                _currentClicks = 0;
            }
            await Task.Delay(300);
            _currentClicks = 0;
        }
    }

    public void CustomValidateTransform() => ValidateTransform();
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);

        var ve = evt.target as VisualElement;
        var sticky = ve?.GetFirstAncestorOfType<StickyNote>();

        // --- Спец-меню для StickyNote ---
        if (sticky != null)
        {
            evt.menu.AppendSeparator();

            evt.menu.AppendAction("Copy StickyNote", _ =>
            {
                // Копируем только этот стикер в буфер GraphView
                var list = new List<GraphElement> { sticky };
                var json = CutCopyOperation(list);
                GUIUtility.systemCopyBuffer = json;
            });

            evt.menu.AppendAction("Paste StickyNote", _ =>
            {
                var json = GUIUtility.systemCopyBuffer;
                if (!string.IsNullOrEmpty(json))
                    PasteOperation("Paste", json);
            });

            evt.menu.AppendAction("Delete StickyNote", _ =>
            {
                DeleteElements(new[] { sticky }); // триггернет OnGraphChanged и данные тоже удалятся
            });
        }

        // --- Остальное меню, что у тебя было ---
        if (evt.menu != null && evt.target is Edge edge)
            evt.menu.AppendAction("Create Separator", _ => CreateSeparatorNode(edge));

        evt.menu.AppendAction("Create Story Node Script", _ => NodeScriptGenerator.ShowWindow());
    }


    void CreateSeparatorNode(Edge edge)
    {
        var parentStoryNodeView = edge.output.node as StoryNodeView;
        var childStoryNodeView = edge.input.node as StoryNodeView;
        if (parentStoryNodeView == null || childStoryNodeView == null)
        {
            Debug.LogError("Cannot create SeparatorNode: parent or child node not found.");
            return;
        }

        Vector2 parentPos = parentStoryNodeView.storyNode.nodePosition;
        Vector2 childPos = childStoryNodeView.storyNode.nodePosition;
        Vector2 separatorPos = (parentPos + childPos) / 2;

        System.Action<StoryNode> onCreated = null;
        onCreated = (n) =>
        {
            if (n is SeparatorNode separatorNode)
            {
                _nodeCreated -= onCreated;
                EditorApplication.delayCall += () => ConnectSeparator(edge, separatorNode);
            }
        };
        _nodeCreated += onCreated;

        CreateNode(typeof(SeparatorNode), separatorPos);
    }

    void ConnectSeparator(Edge edge, SeparatorNode separatorNode)
    {
        if (edge == null || edge.output == null || edge.input == null) return;

        StoryNodeView parentStoryNodeView = edge.output.node as StoryNodeView;
        StoryNode parentNode = parentStoryNodeView.storyNode;

        StoryNodeView childStoryNodeView = edge.input.node as StoryNodeView;
        StoryNode childNode = childStoryNodeView.storyNode;

        SeparatorNodeView separatorStoryNodeView = FindNodeView(separatorNode) as SeparatorNodeView;

        separatorNode.childrenID.Add(childNode.id);
        separatorNode.previousNodesCount++;
        parentNode.childrenID.Add(separatorNode.id);

        parentNode.childrenID.Remove(childNode.id);
        AddElement(parentStoryNodeView.output.ConnectTo(separatorStoryNodeView.input));
        AddElement(separatorStoryNodeView.output.ConnectTo(edge.input));
        RemoveElement(edge);

        EditorUtility.SetDirty(_storyGraph);
    }





    public void CreateNode(System.Type type, Vector2 position)
    {
        var node = _storyGraph.CreateBaseNode(type, position);
        _temporaryNodes.Add(node);
        _nodeCreated?.Invoke(node);
        CreateStoryNodeView(node, position);

        if (!_saveAssetsScheduled)
        {
            _saveAssetsScheduled = true;
            EditorApplication.delayCall += () =>
            {
                SaveChanges();
                _saveAssetsScheduled = false;
            };
        }
    }

    void CreateStoryNodeView(StoryNode node, Vector2 position)
    {
        if (node == null) return;

        var nodeView = CustomStoryNodeViewFactory.CreateNodeView(node);
        Rect rect = new Rect(position, nodeView.GetPosition().size);
        nodeView.SetPosition(rect);
        nodeView.onNodeSelected = onNodeSelected;
        AddElement(nodeView);
    }

    public Vector2 GetLocalMousePosition(Vector2 mousePosition)
    {
        Vector2 worldPosition = mousePosition;
        worldPosition -= window.position.position;
        Vector2 localPosition = contentViewContainer.WorldToLocal(worldPosition);
        return localPosition;
    }

    public StoryNodeView FindNodeView(StoryNode node)
    {
        if (node == null) return null;
        var nodeView = GetNodeByGuid(node.guid) as StoryNodeView;
        if (nodeView == null)
            Debug.LogWarning($"StoryNodeView not found for node with GUID: {node.guid} & {node.GetName()}");
        return nodeView;
    }

    public void UpdateNodesState()
    {
        nodes.ForEach(n =>
        {
            if (n is StoryNodeView nv) nv.UpdateCurrentState();
        });
    }
    #endregion

    #region Interact with GraphView
    private void OnUndoRedo()
    {
        if (_storyGraph == null)
            return;

        // 1. После отката/повтора операции ассет уже в нужном состоянии.
        //    Подтягиваем это состояние в наш кэш.
        SyncTemporaryNodesFromAsset();

        // 2. (опционально) чистим битые ссылки, если используешь CleanupNullNodes
        CleanupNullNodes();

        // 3. Пересобираем вьюху
        PopulateView(_storyGraph);
    }





    // Все ноды, относящиеся к текущей "области" (весь граф или конкретная группа)
    private IEnumerable<StoryNode> GetScopeNodes(BaseGroupNode group)
    {
        if (_storyGraph == null) yield break;

        if (group == null)
        {
            // Весь граф, но только ноды верхнего уровня (без groupNode)
            foreach (var n in _storyGraph.nodes)
            {
                if (n != null && n.groupNode == null)
                    yield return n;
            }
        }
        else
        {
            foreach (var n in group.storyNodes)
            {
                if (n != null)
                    yield return n;
            }
        }
    }

    // Считаем, сколько родителей у ноды в пределах этой области
    private int CountParentsInScope(StoryNode node, IEnumerable<StoryNode> scopeNodes)
    {
        if (node == null) return 0;
        int id = node.id;
        int count = 0;

        foreach (var n in scopeNodes)
        {
            if (n == null || n.childrenID == null) continue;
            if (n.childrenID.Contains(id))
                count++;
        }

        return count;
    }


    private RootNode FindRootNode(BaseGroupNode group)
    {
        if (_storyGraph == null) return null;

        var scopeNodes = GetScopeNodes(group).ToList();
        if (scopeNodes.Count == 0) return null;

        var roots = scopeNodes.OfType<RootNode>().ToList();
        if (roots.Count == 0) return null;

        // 1. Считаем количество родителей в текущей области
        // 2. Берём Root с МИНИМАЛЬНЫМ числом родителей (обычно 0 — настоящий вход)
        // 3. Если несколько — берём визуально "верхний-левый"
        var selected = roots
            .Select(r => new
            {
                node = r,
                parents = CountParentsInScope(r, scopeNodes)
            })
            .OrderBy(x => x.parents)                  // меньше родителей = лучше
            .ThenBy(x => x.node.nodePosition.y)       // выше по Y
            .ThenBy(x => x.node.nodePosition.x)       // левее по X
            .First().node;

        return selected;
    }


    private ReturnNode FindReturnNode(BaseGroupNode group)
    {
        if (_storyGraph == null) return null;

        var scopeNodes = GetScopeNodes(group).ToList();
        if (scopeNodes.Count == 0) return null;

        var returns = scopeNodes.OfType<ReturnNode>().ToList();
        if (returns.Count == 0) return null;

        // 1. Сначала пробуем взять Return без детей (типичный "конец")
        var noChildren = returns
            .Where(r => r.childrenID == null || r.childrenID.Count == 0)
            .ToList();

        var candidates = noChildren.Count > 0 ? noChildren : returns;

        // 2. Среди кандидатов берём ту, у которой МАКСИМУМ родителей (чаще всего "самый конец" цепочки)
        // 3. Если всё равно несколько — берём визуально "нижнюю-правую"
        var selected = candidates
            .Select(r => new
            {
                node = r,
                parents = CountParentsInScope(r, scopeNodes)
            })
            .OrderByDescending(x => x.parents)        // больше родителей = дальше по графу
            .ThenByDescending(x => x.node.nodePosition.y) // ниже по Y
            .ThenByDescending(x => x.node.nodePosition.x) // правее по X
            .First().node;

        return selected;
    }

    public void LoadGraph(StoryGraph storyGraph, BaseGroupNode groupNode = null)
    {
        _storyGraph = storyGraph;
        _currentGroupNode = groupNode;

        // тут при желании можно сначала почистить null-ы
        CleanupNullNodes();

        // а потом подтянуть кэш из ассета
        SyncTemporaryNodesFromAsset();

        if (_currentGroupNode == null)
        {
            _temporaryNodes = new List<StoryNode>(_storyGraph.nodes);

            var autoRoot = FindRootNode(null);
            if (autoRoot != null)
                _storyGraph.rootNode = autoRoot;

            if (_storyGraph.rootNode == null)
            {
                var rootNode = _storyGraph.CreateNode(typeof(RootNode), Vector2.zero) as RootNode;
                _storyGraph.rootNode = rootNode;
                _temporaryNodes.Add(rootNode);
            }
        }
        else
        {
            _temporaryNodes = new List<StoryNode>(_currentGroupNode.storyNodes);

            var autoRoot = FindRootNode(_currentGroupNode);
            if (autoRoot != null)
                _currentGroupNode.rootNode = autoRoot;

            if (_currentGroupNode.rootNode == null)
            {
                var rootNode = _storyGraph.CreateBaseNode(typeof(RootNode), Vector2.zero) as RootNode;
                _currentGroupNode.rootNode = rootNode;
                _temporaryNodes.Add(rootNode);
            }

            var autoReturn = FindReturnNode(_currentGroupNode);
            if (autoReturn != null)
                _currentGroupNode.returnNode = autoReturn;

            if (_currentGroupNode.returnNode == null)
            {
                var returnNode = _storyGraph.CreateBaseNode(typeof(ReturnNode), Vector2.up * 280) as ReturnNode;
                _currentGroupNode.returnNode = returnNode;
                _temporaryNodes.Add(returnNode);
            }
        }


        SaveChanges(_currentGroupNode);
        PopulateView(_storyGraph);

        CenterOnCurrentLayer();
    }


    internal void PopulateView(StoryGraph storyGraph)
    {
        if (storyGraph == null) return;

        _storyGraph = storyGraph;

        CleanupNullNodes();

        Profiler.BeginSample("StoryGraphView Update(Populate View)");

        graphViewChanged -= OnGraphChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphChanged;

        _temporaryNodes.ForEach(n =>
        {
            if (n != null)
                CreateStoryNodeView(n, n.nodePosition);
        });

        for (int i = _temporaryNodes.Count - 1; i >= 0; i--)
            if (_temporaryNodes[i] == null)
                _temporaryNodes.RemoveAt(i);

        _temporaryNodes.ForEach(n =>
        {
            n.storyGraph = storyGraph;
            var nodeView = FindNodeView(n);
            if (nodeView != null)
                nodeView.ConnectNodes(this);

            var children = _storyGraph.GetChildren(n);
            for (int i = children.Count - 1; i >= 0; i--)
                if (children[i] == null)
                    children.RemoveAt(i);
        });

        Profiler.EndSample();
    }

    public void UnsubscribeGraphChange() => graphViewChanged -= OnGraphChanged;

    private GraphViewChange OnGraphChanged(GraphViewChange graphViewChange)
    {
        bool dataChanged = false;


        if (graphViewChange.elementsToRemove != null && graphViewChange.elementsToRemove.Count > 0)
        {
            var copy = new List<GraphElement>(graphViewChange.elementsToRemove);
            foreach (var elem in copy)
            {
                if (elem is StoryNodeView nodeView)
                {
                    if (nodeView.storyNode is BaseGroupNode groupNode)
                        _storyGraph.DeleteGroupNode(groupNode);
                    else
                        _storyGraph.DeleteNode(nodeView.storyNode, nodeView.groupNode);



                    _temporaryNodes.Remove(nodeView.storyNode);
                    dataChanged = true;
                }



                if (elem is Edge edge)
                {
                    var parent = edge.output.node as StoryNodeView;
                    var child = edge.input.node as StoryNodeView;
                    parent?.DisconnectFromOutputPort(edge);
                    child?.DisconnectFromInputPort(edge);
                    dataChanged = true;
                }

            }
        }

        if (graphViewChange.edgesToCreate != null && graphViewChange.edgesToCreate.Count > 0)
        {
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var parent = edge.output.node as StoryNodeView;
                var child = edge.input.node as StoryNodeView;
                parent?.ConnectFromOutputPort(edge);
                child?.ConnectToInputNode(edge);
            }
            dataChanged = true;
        }

        if (graphViewChange.movedElements != null && graphViewChange.movedElements.Count > 0)
        {
            foreach (var element in graphViewChange.movedElements)
            {
                if (element is StoryNodeView nodeView && nodeView.storyNode != null)
                {
                    Undo.RecordObject(nodeView.storyNode, $"Change position {nodeView.storyNode.GetName()}");
                    var pos = nodeView.GetPosition();
                    nodeView.storyNode.nodePosition = new Vector2(pos.xMin, pos.yMin);

                    EditorUtility.SetDirty(nodeView.storyNode);
                }
            }
            dataChanged = true;
        }


        if (dataChanged)
        {
            EditorUtility.SetDirty(_storyGraph);

            // пересобираем кэш из ассета, чтобы Undo/Redo не ломал голову
            SyncTemporaryNodesFromAsset();

            schedule.Execute(() =>
            {
                foreach (var n in nodes.ToList())
                {
                    if (n is StoryNodeView nv && nv.storyNode == null)
                        RemoveElement(nv);
                }

                _temporaryNodes.RemoveAll(n => n == null);
            }).ExecuteLater(0);
        }



        return graphViewChange;
    }
    public void SaveChanges(BaseGroupNode baseGroupNode = null)
    {
        if (baseGroupNode == null) baseGroupNode = _currentGroupNode;

        _temporaryNodes.RemoveAll(n => n == null);

        if (baseGroupNode == null)
            _storyGraph.nodes = new List<StoryNode>(_temporaryNodes);
        else
            _currentGroupNode.storyNodes = new List<StoryNode>(_temporaryNodes);

        // после присваивания списков ещё раз приводим asset в порядок
        CleanupNullNodes();

        EditorUtility.SetDirty(_storyGraph);
    }



    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        {

            return endPort.portType == endPort.portType &&
                   endPort.direction != startPort.direction &&
                   endPort.node != startPort.node;
        }).ToList();
    }
    #endregion


    /// <summary>
    /// Полная зачистка null-нод и битых childrenID в ScriptableObject графа.
    /// </summary>
    private void CleanupNullNodes()
    {
        if (_storyGraph == null)
            return;

        bool dirty = false;

        // 1. Убираем null из общего списка нод графа
        if (_storyGraph.nodes != null)
        {
            int removed = _storyGraph.nodes.RemoveAll(n => n == null);
            if (removed > 0) dirty = true;
        }

        // 2. Убираем null из всех групп (storyNodes)
        if (_storyGraph.nodes != null)
        {
            foreach (var groupNode in _storyGraph.nodes.OfType<BaseGroupNode>())
            {
                if (groupNode.storyNodes == null)
                    continue;

                int removed = groupNode.storyNodes.RemoveAll(n => n == null);
                if (removed > 0) dirty = true;
            }
        }

        // 3. Чистим childrenID от ссылок на несуществующие ноды
        CleanupChildrenLists(null); // верхний уровень

        if (_storyGraph.nodes != null)
        {
            foreach (var groupNode in _storyGraph.nodes.OfType<BaseGroupNode>())
                CleanupChildrenLists(groupNode);        // внутри каждой группы
        }

        if (dirty)
            EditorUtility.SetDirty(_storyGraph);
    }

    /// <summary>
    /// Чистим childrenID в рамках конкретной "области" (весь граф или группа),
    /// убирая id, для которых GetNodeByID возвращает null.
    /// </summary>
    private void CleanupChildrenLists(BaseGroupNode group)
    {
        var scopeNodes = GetScopeNodes(group).ToList();
        if (scopeNodes.Count == 0)
            return;

        foreach (var node in scopeNodes)
        {
            if (node == null || node.childrenID == null)
                continue;

            for (int i = node.childrenID.Count - 1; i >= 0; i--)
            {
                int childId = node.childrenID[i];
                var childNode = _storyGraph.GetNodeByID(childId, group);

                if (childNode == null)
                {
                    node.childrenID.RemoveAt(i);
                }
            }
        }
    }
    /// <summary>
    /// Пересобирает _temporaryNodes из ассета (StoryGraph / текущей группы).
    /// ScriptableObject = источник правды, _temporaryNodes = просто кэш.
    /// </summary>
    private void SyncTemporaryNodesFromAsset()
    {
        if (_storyGraph == null)
        {
            _temporaryNodes = new List<StoryNode>();
            return;
        }

        List<StoryNode> source;

        if (_currentGroupNode == null)
        {
            if (_storyGraph.nodes == null)
                _storyGraph.nodes = new List<StoryNode>();

            source = _storyGraph.nodes;
        }
        else
        {
            if (_currentGroupNode.storyNodes == null)
                _currentGroupNode.storyNodes = new List<StoryNode>();

            source = _currentGroupNode.storyNodes;
        }

        // Кладём в кэш только не-null ноды
        _temporaryNodes = source
            .Where(n => n != null)
            .ToList();
    }

}
