using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.Graph
{
    using System.IO;
    using System.Linq;

    public class NodeScriptGenerator : EditorWindow
    {
        private string nodeName = "NewNode";
        private int nodeTypeIndex = 0;
        private readonly string[] nodeTypes = { "StoryNode", "ActionNode", "StructureNode", "InteractNode", "AudioNode", "WaitNode", "AnimationNode" }; // Можно добавить свои

        [MenuItem("Tools/PG/Create Story Node Script")]
        public static void ShowWindow()
        {
            var window = GetWindow<NodeScriptGenerator>("Create Node Script");
            window.minSize = new Vector2(window.minSize.x, 100);
            window.maxSize = new Vector2(window.maxSize.x, 100);
        }
        private void CreateGUI()
        {
            TextField nodeNameTextField = new TextField("Node Name");
            nodeNameTextField.value = nodeName;
            nodeNameTextField.RegisterValueChangedCallback(evt => nodeName = evt.newValue);
            rootVisualElement.Add(nodeNameTextField);

            PopupField<string> nodeTypePopupField = new PopupField<string>("Base Type");
            nodeTypePopupField.choices = nodeTypes.ToList();
            nodeTypePopupField.index = nodeTypeIndex;
            nodeTypePopupField.RegisterValueChangedCallback(evt => nodeTypeIndex = nodeTypes.ToList().IndexOf(evt.newValue));
            rootVisualElement.Add(nodeTypePopupField);

            Button createButton = new Button(CreateNodeScript);
            createButton.text = "Create Node Script";
            rootVisualElement.Add(createButton);
        }

        private void CreateNodeScript()
        {
            string parentType = nodeTypes[nodeTypeIndex];

            string scriptPath = EditorUtility.SaveFilePanel("Create Node Script", Application.dataPath, nodeName, "cs");

            if (string.IsNullOrEmpty(scriptPath))
            {
                Close();
                return;
            }

            // Получаем имя файла без расширения
            string fileName = Path.GetFileNameWithoutExtension(scriptPath);

            // Проверка: имя не пустое и подходит под правила C#
            if (string.IsNullOrWhiteSpace(fileName) || fileName.Any(ch => !char.IsLetterOrDigit(ch) && ch != '_'))
            {
                EditorUtility.DisplayDialog("Ошибка", "Имя файла некорректно! Используйте только буквы, цифры и подчёркивания.", "OK");
                return;
            }

            string script =
    $@"using UnityEngine;
using PG.StorySystem;

namespace PG.StorySystem.Nodes
{{
    public class {fileName} : {parentType}
    {{
        protected override void OnStart(StoryGraph storyGraph) {{}}
        protected override void OnEnd(StoryGraph storyGraph) {{}}
        protected override void OnUpdate(StoryGraph storyGraph) {{}}
    }}
}}
";

            if (string.IsNullOrEmpty(scriptPath))
            {
                Close();
                return;
            }

            File.WriteAllText(scriptPath, script);
            AssetDatabase.Refresh();
            Debug.Log($"Node script created at {scriptPath}");
            Close();
        }
    }
}
