using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using PG.StorySystem.Graph;
using PG.StorySystem.Nodes;
using PG.StorySystem;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using System.Threading.Tasks;
using UnityEngine.Profiling;


//For Unity 6 and later
#if UNITY_6000_0_OR_NEWER
[UxmlElement]
#endif
public sealed partial class StoryGraphView : GraphView
{
    //For Unity 2022 LTS and older
#if !UNITY_6000_0_OR_NEWER
    public new class UxmlFactory : UxmlFactory<StoryGraphView, UxmlTraits> { }
#endif

    private StoryGraph _storyGraph;
    public StoryGraph storyGraph => _storyGraph;
    internal Action<StoryNodeView> onNodeSelected;

    public BaseGroupNode currentGroupNode => _currentGroupNode;

    [NonSerialized]
    private List<StoryNode> _temporaryNodes = new List<StoryNode>(); // ��������� ������ ��� ��� �������� �����������
    private BaseGroupNode _currentGroupNode; // ������� ������ (����)

    private int _currentClicks = 0;

    private bool _saveAssetsScheduled = false;
    // ������ ��� �������� ������������� �����
    private ClipboardNodes _clipboardNodes = new ClipboardNodes();

    private Vector2 _createdNodeOffset = new Vector2(25, 40);
    private int _pasteIndex = 1;

    [Serializable]
    private class ClipboardNodes
    {
        public List<StoryNode> copiedNodes = new List<StoryNode>();
    }

    private event Action<StoryNode> _nodeCreated = default;

    public StoryGraphEditorWindow window
    {
        get => _window;
        set
        {
            _window = value;
            if (_window != null)
            {
                InitializeObjects();
            }
        }
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


        // ������������� �� ������� ��� ��������� �������� ����������� � �������
        serializeGraphElements += CutCopyOperation;
        unserializeAndPaste += PasteOperation;

        this.RegisterCallback<MouseDownEvent>(OnMouseDown, TrickleDown.TrickleDown);
    }
    #region Copy, Paste and Dublicate
    private void PasteOperation(string operationName, string data)
    {
        JsonUtility.FromJsonOverwrite(data, _clipboardNodes);

        Dictionary<int, int> idMapping = new Dictionary<int, int>(); // ������������ ������ � ����� ID
        List<StoryNode> newNodes = new List<StoryNode>(); // �������� ���� ����� ��� ��� ����������� ��������� ������

        for (int i = 0; i < _clipboardNodes.copiedNodes.Count; i++)
        {
            StoryNode originalNode = _clipboardNodes.copiedNodes[i];
            StoryNode newNode = ScriptableObject.Instantiate(originalNode);

            // ������� ����� ���������� ID � ��������� ������������
            newNode.guid = GUID.Generate().ToString();
            newNode.nodePosition += _createdNodeOffset * _pasteIndex;
            _pasteIndex++;

            storyGraph.nextId++;
            int oldId = originalNode.id;
            int newId = storyGraph.nextId;
            newNode.id = newId;
            idMapping[oldId] = newId;

            // ����������� �� ������ �����
            newNode.childrenID = new List<int>();

            // ��������� ����� ���� � AssetDatabase
            AssetDatabase.AddObjectToAsset(newNode, storyGraph);
            if (newNode is BaseGroupNode groupNode1)
            {
                CopyGroup(groupNode1);
            }
            Undo.RegisterCreatedObjectUndo(newNode, "Story Graph (Copied Node)");

            // ��������� ����� ���� � ����
            Undo.RecordObject(storyGraph, "Story Graph (Copied Node)");

            _temporaryNodes.Add(newNode);

            newNodes.Add(newNode); // ��������� ��� ����������� ���������
        }

        // ��������������� ����� ����� �������������� ������
        foreach (StoryNode newNode in newNodes)
        {
            StoryNode originalNode = _clipboardNodes.copiedNodes.FirstOrDefault(node => node.id == idMapping.FirstOrDefault(pair => pair.Value == newNode.id).Key);

            if (originalNode != null)
            {
                newNode.OnDublicateChildren(idMapping, originalNode);
            }
        }
        SaveChanges();
        PopulateView(storyGraph);
    }



    private string CutCopyOperation(IEnumerable<GraphElement> elements)
    {
        _clipboardNodes.copiedNodes.Clear();
        _pasteIndex = 1;
        //throw new NotImplementedException();
        foreach (var element in elements)
        {
            if (element != null && element is StoryNodeView nodeView)
            {
                StoryNode storyNode = ScriptableObject.Instantiate(nodeView.storyNode);
                storyNode.guid = GUID.Generate().ToString();
                storyNode.nodePosition += _createdNodeOffset * _pasteIndex;
                _clipboardNodes.copiedNodes.Add(storyNode);
            }
        }
        return JsonUtility.ToJson(_clipboardNodes);
    }
    void CopyGroup(BaseGroupNode groupNode)
    {
        List<StoryNode> originalNodes = new List<StoryNode>(groupNode.storyNodes); // ��������� �������� ������
        List<StoryNode> newNodes = new List<StoryNode>(); // ��� �������� ����� ������������� ���

        Dictionary<int, int> idMapping = new Dictionary<int, int>(); // ��� ���������� ������ � ����� ID

        // �������� ������ ����
        foreach (StoryNode originalNode in originalNodes)
        {
            StoryNode newNode = ScriptableObject.Instantiate(originalNode);

            // ������� ����� ���������� ID
            newNode.guid = GUID.Generate().ToString();
            newNode.nodePosition += _createdNodeOffset * _pasteIndex;

            storyGraph.nextId++;
            int oldId = originalNode.id;
            int newId = storyGraph.nextId;
            newNode.id = newId;

            // ��������� ������������ ������� � ������ ID
            idMapping[oldId] = newId;

            // ���� ���� � ��� ��������� ������, �������� CopyGroup ����������
            if (newNode is BaseGroupNode childGroupNode)
            {
                CopyGroup(childGroupNode);
            }

            // ����������� �� ������ ������
            newNode.childrenID = new List<int>();

            // ��������� ����� ���� � AssetDatabase
            AssetDatabase.AddObjectToAsset(newNode, storyGraph);
            Undo.RegisterCreatedObjectUndo(newNode, "Story Graph (Copied Node)");

            newNodes.Add(newNode);
        }

        // ��������������� ����� ����� �������������� ������
        foreach (StoryNode newNode in newNodes)
        {
            StoryNode originalNode = originalNodes.FirstOrDefault(node => node.id == idMapping.FirstOrDefault(pair => pair.Value == newNode.id).Key);

            if (originalNode != null)
            {
                foreach (int oldChildId in originalNode.childrenID)
                {
                    if (idMapping.TryGetValue(oldChildId, out int newChildId))
                    {
                        newNode.childrenID.Add(newChildId); // ������������� ����� � ������ ������
                    }
                }
            }
        }

        // �������� ������ ���� � ������ ������
        groupNode.storyNodes.Clear();
        groupNode.storyNodes.AddRange(newNodes);

        Undo.RecordObject(storyGraph, "Story Graph (Copied Node)");
    }

    #endregion

    public void AddGroupLayer(BaseGroupNode targetGroupNode)
    {
        ToolbarButton layerButton = new ToolbarButton();
        layerButton.text = !string.IsNullOrWhiteSpace(targetGroupNode.nameGroup) ? targetGroupNode.nameGroup : targetGroupNode.GetName();
        layerButton.clicked += () => {
            _currentGroupNode = targetGroupNode;
            RemoveLayer(layerButton);
        };
        buttons.Add(layerButton);
        if (StoryGraphEditorWindow.toolbarBreadcrumbs != null)
        {
            StoryGraphEditorWindow.toolbarBreadcrumbs.Add(layerButton);
        }
    }
    public void RemoveLayer(ToolbarButton button)
    {
        for (int i = buttons.IndexOf(button)+1; i < buttons.Count; i++)
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
            SetupSliderZoom();

            Button centerButton = window.root.Q<Button>("CenterButton");
            centerButton.clicked += () => viewTransform.position = viewTransform.scale / storyGraph.rootNode.nodePosition;
        }
    }

    void OpenSearchWindow(NodeCreationContext context)
    {
        if (_searchWindow == null)
        {
            _searchWindow = ScriptableObject.CreateInstance<StoryGraphSearchWindow>();
        }
        _searchWindow.Initialize(this);
        SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    #region Interact with nodes
    private async void OnMouseDown(MouseDownEvent evt)
    {
        if (evt.button == 0)
        {
            if (evt.target is Edge edge)
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
            //Debug.Log("Current Clicks " + _currentClicks);
            //Debug.Log("Target " + evt.target);
        }
    }


    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        if (evt.menu != null && evt.target is Edge edge)
        {
            evt.menu.AppendAction("Create Separator", e => {
                CreateSeparatorNode(edge);
            });
        }
    }

    void CreateSeparatorNode(Edge edge)
    {
        StoryNodeView parentNodeView = edge.output.node as StoryNodeView;
        StoryNodeView childNodeView = edge.input.node as StoryNodeView;
        if (parentNodeView == null || childNodeView == null)
        {
            Debug.LogError("���������� ������� SeparatorNode: ������������ ��� �������� ���� �� �������.");
            return;
        }

        Vector2 parentPos = parentNodeView.storyNode.nodePosition;
        Vector2 childPos = childNodeView.storyNode.nodePosition;
        Vector2 separatorPos = (parentPos + childPos) / 2;

        // ��������� ��������� ��� ��������� ������� �������� SeparatorNode
        System.Action<StoryNode> onCreated = null;
        onCreated = (n) =>
        {
            if (n is SeparatorNode separatorNode)
            {
                // ������������, ����� ���������� �������� ������ ���� ���
                _nodeCreated -= onCreated;
                // ���������� SeparatorNode � Edge
                ConnectSeparator(edge, separatorNode);
            }
        };
        _nodeCreated += onCreated;

        // ������� ���� SeparatorNode � ��������� �������
        CreateNode(typeof(SeparatorNode), separatorPos);
    }

    void ConnectSeparator(Edge edge, SeparatorNode separatorNode)
    {
        if (edge == null || edge.output == null || edge.input == null)
        {
            //Debug.LogError("Edge or its connections are null.");
            return;
        }

        StoryNodeView parentNodeView = edge.output.node as StoryNodeView;
        StoryNodeView childNodeView = edge.input.node as StoryNodeView;

        if (parentNodeView == null || childNodeView == null)
        {
            //Debug.LogError("Edge nodes are not of type NodeView.");
            return;
        }

        if (childNodeView.storyNode == null)
        {
            //Debug.LogError("Child node's StoryNode is null.");
            return;
        }

        int childId = childNodeView.storyNode.id;

        List<int> children = parentNodeView.GetChildrenList(childId);
        if (children == null)
        {
            //Debug.LogWarning("Children list is null. Creating a new one.");
            children = new List<int>();
        }

        children.Remove(childId);
        children.Add(separatorNode.id);

        if (separatorNode != null)
        {
            separatorNode.childrenID.Add(childId);
            separatorNode.previousNodesCount++;
        }
        else
        {
            Debug.LogError("SeparatorNode is null.");
        }

        PopulateView(storyGraph);
    }

    public void CreateNode(System.Type type, Vector2 position)
    {
        StoryNode node = _storyGraph.CreateBaseNode(type, position);
        _temporaryNodes.Add(node);
        _nodeCreated?.Invoke(node);
        CreateNodeView(node, position);
        PopulateView(storyGraph);

        if (!_saveAssetsScheduled)
        {
            _saveAssetsScheduled = true;
            EditorApplication.delayCall += () =>
            {
                // ��������� ���������
                SaveChanges();
                _saveAssetsScheduled = false;
            };
        }
        return;
    }
    void CreateNodeView(StoryNode node, Vector2 position)
    {
        if (node == null)
        {
            return;
        }

        // ������������ �������� NodeView ����� �������
        StoryNodeView nodeView = CustomStoryNodeViewFactory.CreateNodeView(node);
        // ��������� ������� � ���������� ���� � ����
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
    internal StoryNodeView FindNodeView(StoryNode node)
    {
        if (node == null)
        {
            return null;
        }
        var nodeView = GetNodeByGuid(node.guid) as StoryNodeView;
        if (nodeView == null)
        {
            Debug.LogWarning($"NodeView not found for node with GUID: {node.guid} & {node.GetName()}");
        }
        return nodeView;
    }

    public void UpdateNodesState()
    {
        nodes.ForEach(node =>
        {
            StoryNodeView nodeView = node as StoryNodeView;
            nodeView.UpdateCurrentState();
        });
    }

    #endregion

    #region Interact with GraphView
    private void SetupSliderZoom()
    {
        Slider zoomSlider = window.root.Q<Slider>("Zoom");

        zoomSlider.RegisterValueChangedCallback((v) =>
        {
            float newScale = Mathf.Clamp(v.newValue, zoomSlider.lowValue, zoomSlider.highValue);
            viewTransform.scale = Vector3.one * newScale;
            viewTransform.scale = Vector3.ClampMagnitude(viewTransform.scale, zoomSlider.highValue);

            ValidateTransform();
        });

        // ��� ������������� � ����� ������� ���� (�� ������ �� �������)
        RegisterCallback<WheelEvent>((e) =>
        {
            float scrollDelta = e.delta.y > 0 ? -0.1f : 0.1f;
            zoomSlider.SetValueWithoutNotify(Mathf.Clamp(zoomSlider.value + scrollDelta, zoomSlider.lowValue, zoomSlider.highValue));
        });
    }

    private void OnUndoRedo()
    {
        PopulateView(_storyGraph);
    }
    // ����� ��� �������� ����� � ������������� ���������� ������ ���
    public void LoadGraph(StoryGraph storyGraph, BaseGroupNode groupNode = null)
    {
        _storyGraph = storyGraph;
        _currentGroupNode = groupNode;

        if (_currentGroupNode == null)
        {
            // ������ � �������� �������
            _temporaryNodes = new List<StoryNode>(_storyGraph.nodes);

            if (_storyGraph.rootNode == null)
            {
                RootNode rootNode = _storyGraph.CreateNode(typeof(RootNode), Vector2.zero) as RootNode;
                _storyGraph.rootNode = rootNode;
                _temporaryNodes.Add(rootNode);
            }
        }
        else
        {
            // ������ � ��������� �����
            _temporaryNodes = new List<StoryNode>(_currentGroupNode.storyNodes);


            if (_currentGroupNode.rootNode == null)
            {
                RootNode rootNode = _storyGraph.CreateBaseNode(typeof(RootNode), Vector2.zero) as RootNode;
                _currentGroupNode.rootNode = rootNode;
                _temporaryNodes.Add(rootNode);
            }
            if (_currentGroupNode.returnNode == null)
            {
                ReturnNode returnNode = _storyGraph.CreateBaseNode(typeof(ReturnNode), Vector2.up * 280) as ReturnNode;
                _currentGroupNode.returnNode = returnNode;
                _temporaryNodes.Add(returnNode);
            }
        }
        SaveChanges(_currentGroupNode);
        PopulateView(_storyGraph);
    }
    internal void PopulateView(StoryGraph storyGraph)
    {
        if (storyGraph != null)
        {
            _storyGraph = storyGraph;

            Profiler.BeginSample("StoryGraphView Update(Populate View)");


            graphViewChanged -= OnGraphChanged;
            DeleteElements(graphElements);
            graphViewChanged += OnGraphChanged;

            //Create nodes as NodeView type
            _temporaryNodes.ForEach(n => {
                if (n != null)
                {
                    CreateNodeView(n, n.nodePosition);
                }
            });

            for (int i = _temporaryNodes.Count - 1; i >= 0; i--)
            {
                if (_temporaryNodes[i] == null)
                {
                    _temporaryNodes.RemoveAt(i);
                }
            }


            //Create edges
            _temporaryNodes.ForEach(n =>
            {
                n.storyGraph = storyGraph;
                StoryNodeView nodeView = FindNodeView(n);
                if (nodeView != null)
                {
                    nodeView?.ConnectNodes(this);
                }
                var children = _storyGraph.GetChildren(n);
                for (int i = children.Count-1; i >= 0; i--)
                {
                    if (children[i] == null)
                    {
                        children.RemoveAt(i);
                    }
                }
            });

            Profiler.EndSample();
        }
    }

    public void UnsubscribeGraphChange()
    {
        graphViewChanged -= OnGraphChanged;
    }
    private GraphViewChange OnGraphChanged(GraphViewChange graphViewChange)
    {
        //Profiler.BeginSample("StoryGraphView Update");
        if (graphViewChange.elementsToRemove != null)
        {
            // ������� ����� ��������� ��� ���������� ��������
            var elementsToRemoveCopy = new List<GraphElement>(graphViewChange.elementsToRemove);

            foreach (var elem in elementsToRemoveCopy)
            {
                //Profiler.BeginSample("StoryGraphView Update(Deletes Nodes)");
                // �������� �����
                if (elem is StoryNodeView nodeView)
                {
                    if (nodeView.storyNode is BaseGroupNode groupNode)
                    {
                        _storyGraph.DeleteGroupNode(groupNode);
                    }
                    else
                    {
                        _storyGraph.DeleteNode(nodeView.storyNode, nodeView.groupNode);
                    }
                }
                //Profiler.EndSample();

                //Profiler.BeginSample("StoryGraphView Update(Deletes Edges)");
                // �������� ������ (�����)
                if (elem is Edge edge)
                {
                    var parent = edge.output.node as StoryNodeView;
                    var child = edge.input.node as StoryNodeView;

                    parent?.UnConnectToOutputNode(edge);
                    child?.UnConnectToInputNode(edge);
                }
                EditorUtility.SetDirty(_storyGraph);
               // Profiler.EndSample();
            }
        }

        // �������� ����� �����
        if (graphViewChange.edgesToCreate != null)
        {
            //Profiler.BeginSample("StoryGraphView Update(Create edges)");
            foreach (var edge in graphViewChange.edgesToCreate)
            {
                var parent = edge.output.node as StoryNodeView;
                var child = edge.input.node as StoryNodeView;

                parent?.ConnectToOutputNode(edge);
                child?.ConnectToInputNode(edge);
            }
            EditorUtility.SetDirty(_storyGraph);
            //Profiler.EndSample();
        }

        // ����������� ���������
        if (graphViewChange.movedElements != null)
        {
            //Profiler.BeginSample("StoryGraphView Update(Move Nodes)");
            foreach (var element in graphViewChange.movedElements)
            {
                if (element is StoryNodeView nodeView)
                {
                    nodeView.storyNode.storyGraph = _storyGraph;
                }
            }

            //Profiler.EndSample();
        }

        PopulateView(_storyGraph);

        // ��������� ���������� ������� ������ ���� ��� ��� �� �������������
        if (graphViewChange.elementsToRemove != null || graphViewChange.edgesToCreate != null)
        {
            if (!_saveAssetsScheduled)
            {
                _saveAssetsScheduled = true;
                EditorApplication.delayCall += () =>
                {
                    // ��������� ���������
                    SaveChanges();
                    _saveAssetsScheduled = false;
                };
            }
        }
        //Profiler.EndSample();
        return graphViewChange;
    }
    // ����� ��� ���������� ��������� � �������� ����
    public void SaveChanges(BaseGroupNode baseGroupNode = null)
    {
        if (baseGroupNode == null)
        {
            baseGroupNode = _currentGroupNode;
        }
        if (baseGroupNode == null)
        {
            // ��������� �������� ������ ���
            _storyGraph.nodes = new List<StoryNode>(_temporaryNodes);
        }
        else
        {
            // ��������� ������ ��� � ������� ������
            _currentGroupNode.storyNodes = new List<StoryNode>(_temporaryNodes);
        }

        // �������� ������ ��� ���������� � ���������
        EditorUtility.SetDirty(_storyGraph);
        
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort =>
        {

            return endPort.direction != startPort.direction && 
                endPort.portType == startPort.portType &&
                endPort.node != startPort.node;
        }).ToList();
    }
    #endregion
}
