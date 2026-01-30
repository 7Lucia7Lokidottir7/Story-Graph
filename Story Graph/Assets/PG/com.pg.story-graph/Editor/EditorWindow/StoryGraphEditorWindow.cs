using UnityEditor;
using UnityEngine;
namespace PG.StorySystem
{
    using Graph;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
    using PG.StorySystem.Nodes;

    public class StoryGraphEditorWindow : EditorWindow
    {
        public VisualElement root;
        public VisualElement graphViewContainer { get; private set; }
        public static StoryGraphView graphView { get; private set; }
        private static StoryInspectorView _inspectorView;
        public static DropdownField dataDropdown { get; private set; }
        public static ToolbarBreadcrumbs toolbarBreadcrumbs { get; private set; }
        public static StoryGraph storyGraph {  get; private set; }
        public static bool isDebug;



        [MenuItem("Window/PG/StoryGraph")]
        public static void OpenEmptyGraph()
        {
            StoryGraphEditorWindow wnd = GetWindow<StoryGraphEditorWindow>();
            wnd.titleContent = new GUIContent("StoryGraph");
            wnd.titleContent.image = Resources.Load<Texture>("StoryGraph/Icon");
            graphView.window = wnd;
            toolbarBreadcrumbs?.Clear();
            //_storyGraph = CreateInstance<StoryGraph>();
        }

        public static void OpenGraph(StoryGraph storyGraph)
        {
            StoryGraphEditorWindow wnd = GetWindow<StoryGraphEditorWindow>();
            wnd.titleContent = new GUIContent("StoryGraph");
            wnd.titleContent.image = Resources.Load<Texture>("StoryGraph/Icon");
            toolbarBreadcrumbs?.Clear();
            if (storyGraph != null && graphView != null)
            {
                graphView.window = wnd;
                StoryGraphEditorWindow.storyGraph = storyGraph;
                graphView.LoadGraph(storyGraph);
                //graphView.PopulateView(storyGraph);
            }

        }
        private void OnDestroy()
        {
            graphView.UnsubscribeGraphChange();
        }
        public void CreateGUI()
        {

            // Each editor window contains a root VisualElement object
            root = rootVisualElement;
            SetupStyles();
            InitializeObjects();
            // Обработчик изменения дропдауна
            dataDropdown.RegisterValueChangedCallback(c =>
            {
                if (storyGraph != null)
                {
                    _inspectorView.UpdatePanel(storyGraph); // Меняем панель при изменении дропдауна
                }
            });

            graphView.onNodeSelected = OnNodeSelection;
            OnToolbarMenu();

        }
        private void OnInspectorUpdate()
        {
            graphView?.UpdateNodesState();
        }
        private void InitializeObjects()
        {
            graphViewContainer = root.Q<VisualElement>("container-graph-view");

            graphView = root.Q<StoryGraphView>();


            ToolbarButton saveButton = root.Q<ToolbarButton>("BaseLayerButton");
            saveButton.clicked += AssetDatabase.SaveAssets;


            _inspectorView = root.Q<StoryInspectorView>();
            dataDropdown = root.Q<DropdownField>("data-dropdown");
            toolbarBreadcrumbs = root.Q<ToolbarBreadcrumbs>("ToolbarBreadcrumbs");

            ToolbarButton layerButton = root.Q<ToolbarButton>("BaseLayerButton");
            layerButton.clicked += () => {
                graphView.LoadGraph(storyGraph);
                toolbarBreadcrumbs.Clear();
                graphView.contentContainer.resolvedStyle.translate.Set(storyGraph.rootNode.nodePosition.x, storyGraph.rootNode.nodePosition.y, 0);
            };

        }

        private void SetupStyles()
        {
            var visualTree = Resources.Load<VisualTreeAsset>("StoryGraph/StoryGraphEditorWindow");
            visualTree.CloneTree(root);

            var stylesheet = Resources.Load<StyleSheet>("StoryGraph/StoryGraphEditorWindowStyle");
            root.styleSheets.Add(stylesheet);
        }

        private void OnToolbarMenu()
        {
            ToolbarMenu toolbarMenu = root.Q<ToolbarMenu>("AssetsMenu");
            toolbarMenu.menu.AppendAction("Create", c =>
            {
                string basePath = "Assets/New Story Graph.asset";
                string path = basePath;
                int counter = 1;

                // Проверяем, существует ли файл с таким названием, и если да, добавляем счетчик
                while (AssetDatabase.LoadAssetAtPath<StoryGraph>(path) != null)
                {
                    path = $"Assets/New Story Graph {counter}.asset";
                    counter++;
                }

                // Создаем новый ассет с уникальным именем
                StoryGraph graph = CreateInstance<StoryGraph>();
                AssetDatabase.CreateAsset(graph, path);
                
                AssetDatabase.Refresh();
                OpenGraph(graph);
            });

            toolbarMenu.menu.AppendAction("Open", c =>
            {
                string fullPath = EditorUtility.OpenFilePanel("Open Story Graph", Application.dataPath, "asset");

                if (!string.IsNullOrEmpty(fullPath))
                {
                    string relativePath = "Assets" + fullPath.Substring(Application.dataPath.Length);

                    StoryGraph graph = AssetDatabase.LoadAssetAtPath<StoryGraph>(relativePath);

                    if (graph != null)
                    {
                        OpenGraph(graph);
                    }
                    else
                    {
                        Debug.LogError("Не удалось загрузить ассет по указанному пути.");
                    }
                }
            });

        }

        private void OnEnable()
        {
            EditorApplication.playModeStateChanged += UpdateView;
        }
        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= UpdateView;
        }
        void UpdateView(PlayModeStateChange stateChange)
        {
            switch (stateChange)
            {
                case PlayModeStateChange.EnteredEditMode:
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    ResetToBaseStoryGraph();
                    break;
            }
        }
        private void ResetToBaseStoryGraph()
        {
            if (Selection.activeGameObject)
            {
                StoryGraphRunner storyGraphRunner = Selection.activeGameObject.GetComponent<StoryGraphRunner>();
                if (storyGraphRunner != null)
                {
                    storyGraph = storyGraphRunner.baseStoryGraph;
                    OpenGraph(storyGraph);
                }
            }
        }

        private void OnSelectionChange()
        {
            StoryGraph newStoryGraph = Selection.activeObject as StoryGraph;
            if (!newStoryGraph)
            {
                if (Selection.activeGameObject)
                {
                    StoryGraphRunner storyGraphRunner = Selection.activeGameObject.GetComponent<StoryGraphRunner>();
                    if (storyGraphRunner != null)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            storyGraph = storyGraphRunner.storyGraph;
                        }
                        else
                        {
                            storyGraph = storyGraphRunner.baseStoryGraph;
                        }
                    }
                }
            }
            else
            {
                if (storyGraph != newStoryGraph)
                {
                    storyGraph = newStoryGraph;
                    if (storyGraph != null)
                    {
                        OpenGraph(storyGraph);
                    }
                }
            }
        }
        void OnNodeSelection(StoryNodeView nodeView)
        {
            _inspectorView.UpdateSelection(nodeView);
        }
    }
}
