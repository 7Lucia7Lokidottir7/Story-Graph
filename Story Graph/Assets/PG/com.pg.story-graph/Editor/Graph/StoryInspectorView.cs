using UnityEditor;
using UnityEngine;
using PG.StorySystem.Graph;
using PG.StorySystem.Nodes;
using PG.StorySystem;
    using UnityEngine.UIElements;
//For Unity 6 and later

#if UNITY_6000_0_OR_NEWER
    [UxmlElement]
#endif
public sealed partial class StoryInspectorView : VisualElement
{
    //For Unity 2022 LTS and older

#if !UNITY_6000_0_OR_NEWER
    public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }
#endif

    private bool _createVariableFoldout;
    public StoryInspectorView() { }
    private VisualTreeAsset _objectTreeAsset;

    private VisualTreeAsset _variablesContainer;

    private VisualElement _objectsContainer = new VisualElement();

    // Метод для обновления панели
    internal void UpdatePanel(StoryGraph storyGraph)
    {
        Clear();

        switch (StoryGraphEditorWindow.dataDropdown.index)
        {
            default:
                RenderInspector();
                break;
            case 0:
                RenderInspector();
                break;
            case 1:

                if (_objectTreeAsset == null)
                {
                    _objectTreeAsset = Resources.Load("StoryGraph/ObjectElementGUI") as VisualTreeAsset;
                }

                Button addButton = new Button();
                addButton.text = "+";
                addButton.clicked += () => AddObject(storyGraph);
                Add(addButton);
                RenderObjectList(storyGraph);
                Add(_objectsContainer);
                break;
        }
    }

    void RenderInspector()
    {
        // Панель, соответствующая первому значению дропдауна
        IMGUIContainer container = new IMGUIContainer(() =>
        {
        });
        Add(container);
        if (_editor != null)
        {
            Add(_editor.CreateInspectorGUI());
        }
    }
    void AddObject(StoryGraph storyGraph)
    {
        storyGraph.objects.Add("");
        RenderObjectList(storyGraph);
    }

    void RenderObjectList(StoryGraph storyGraph)
    {
        _objectsContainer.Clear();  // Очищаем контейнер перед перерисовкой
        if (storyGraph.objects.Count > 0)
        {
            for (int i = 0; i < storyGraph.objects.Count; i++)
            {
                int index = i; // Локальная копия индекса для использования в лямбде



                VisualElement objectElement = new VisualElement();
                _objectTreeAsset.CloneTree(objectElement);


                _objectsContainer.Add(objectElement);

                TextField textField = objectElement.Q<TextField>();
                textField.value = storyGraph.objects[index];
                textField.RegisterValueChangedCallback(evt =>
                {
                    Undo.RecordObject(storyGraph, "Changed Objects");
                    storyGraph.objects[index] = evt.newValue;
                    EditorUtility.SetDirty(storyGraph);
                });
                Button removeButton = objectElement.Q<Button>();
                removeButton.clicked += () => RemoveObject(index, storyGraph);

            }
        }
    }

    void RemoveObject(int index, StoryGraph storyGraph)
    {
        if (index >= 0 && index < storyGraph.objects.Count)
		{
			Undo.RecordObject(storyGraph, "Removed Object");
			storyGraph.objects.RemoveAt(index);
			EditorUtility.SetDirty(storyGraph);
			RenderObjectList(storyGraph); // Перерисовываем список только после удаления
        }
    }



    private Editor _editor;
    internal void UpdateSelection(StoryNodeView nodeView)
    {
        if (StoryGraphEditorWindow.dataDropdown.index == 0)
        {
            Clear();
            if (_editor != null)
            {
                Object.DestroyImmediate(_editor);
            }
            _editor = Editor.CreateEditor(nodeView.storyNode);


            Toggle isDebugToggle = new Toggle("Is Debug");
            isDebugToggle.value = StoryGraphEditorWindow.isDebug;
            isDebugToggle.RegisterValueChangedCallback(evt =>
            {
                StoryGraphEditorWindow.isDebug = evt.newValue;
                UpdateSelection(nodeView);
            });
            Add(isDebugToggle);

            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (_editor.target)
                {
                    _editor.OnInspectorGUI();
                    nodeView.UpdateDescriptionText(nodeView.storyNode);
                }
            });
            Add(container);

            Add(_editor.CreateInspectorGUI());
        }
    }
}
