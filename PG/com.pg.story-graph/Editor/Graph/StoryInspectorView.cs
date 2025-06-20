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

        if (StoryGraphEditorWindow.dataDropdown.index == 0)
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
        else if (StoryGraphEditorWindow.dataDropdown.index == 1)
        {

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
        }
        else if (StoryGraphEditorWindow.dataDropdown.index == 2)
        {

            _variablesContainer = Resources.Load("StoryGraph/Variable/VariablesPanel") as VisualTreeAsset;
            VisualElement variablePanel = _variablesContainer.CloneTree();
            OnVariablesPanel(storyGraph, variablePanel);
            Add(variablePanel);

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
                    storyGraph.objects[index] = evt.newValue;
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
            storyGraph.objects.RemoveAt(index);
            RenderObjectList(storyGraph); // Перерисовываем список только после удаления
        }
    }

    void AddVariable(StoryGraph storyGraph, StoryVariable variable)
    {

        Undo.RecordObject(storyGraph, variable.GetUndoText());
        storyGraph.variables.Add(variable);

        AssetDatabase.AddObjectToAsset(variable, storyGraph);
        Undo.RegisterCreatedObjectUndo(variable, variable.GetUndoText());

        AssetDatabase.SaveAssets();
    }
    void RemoveVariable(int index, StoryGraph storyGraph)
    {
        if (index >= 0 && index < storyGraph.variables.Count)
        {
            Undo.RecordObject(storyGraph, "Story Graph (Delete Variable)");
            StoryVariable scenarioVariable = storyGraph.variables[index];
            storyGraph.variables.RemoveAt(index);

            Undo.DestroyObjectImmediate(scenarioVariable);
            AssetDatabase.SaveAssets();
        }
    }

    void OnVariablesPanel(StoryGraph storyGraph, VisualElement variablePanel)
    {
        CreateVaribale(storyGraph, variablePanel);
        RenderVariables(storyGraph, variablePanel.Q<VisualElement>("Container"));

    }
    void RenderVariables(StoryGraph storyGraph, VisualElement variablePanel)
    {
        variablePanel.Clear();  // Очищаем контейнер перед перерисовкой
        if (storyGraph.variables.Count > 0)
        {
            for (int i = 0; i < storyGraph.variables.Count; i++)
            {
                int index = i; // Локальная копия индекса для использования в лямбде

                Editor editor = Editor.CreateEditor(storyGraph.variables[i]);

                VisualElement variableElement = editor.CreateInspectorGUI();

                variablePanel.Add(variableElement);

                Button removeButton = variableElement.Q<Button>();
                removeButton.clicked += () => { 
                    RemoveVariable(index, storyGraph);
                    OnVariablesPanel(storyGraph, variablePanel);
                };

            }
        }
    }

    private void CreateVaribale(StoryGraph storyGraph, VisualElement variablePanel)
    {
        Foldout createFoldout = variablePanel.Q<Foldout>("CreateFoldout");
        createFoldout.RegisterValueChangedCallback(evt =>
        {
            if (evt.newValue)
            {
                createFoldout.Clear();
                var types = TypeCache.GetTypesDerivedFrom<StoryVariable>();
                foreach (var type in types)
                {
                    Button button = new Button();
                    StoryVariable variable = ScriptableObject.CreateInstance(type) as StoryVariable;
                    button.text = variable.GetTypeName();
                    button.clicked += () =>
                    {
                        AddVariable(storyGraph, variable);
                        createFoldout.value = false;
                        OnVariablesPanel(storyGraph, variablePanel);
                    };
                    createFoldout.Add(button);
                }
            }
        });
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
