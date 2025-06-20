using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(Vector3ConditionNode), true)]
    public partial class Vector3ConditionNodeEditor : StoryNodeEditor
    {
        private Vector3ConditionNode _vector3ConditionNode;
        protected override void Init()
        {
            base.Init();
            _vector3ConditionNode = target as Vector3ConditionNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Variable"));
            SearchableDropdownField  dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
            dropdownField.choices = variables;
            dropdownField.value = _vector3ConditionNode.variable1NameID;
            dropdownField.OnValueChanged += evt => {
                _vector3ConditionNode.variable1NameID = evt;
                EditorUtility.SetDirty(_vector3ConditionNode);
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _vector3ConditionNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                _vector3ConditionNode.isActiveVariable2 = evt.newValue;
                EditorUtility.SetDirty(_vector3ConditionNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.choices = variables;
            dropdownField2.value = _vector3ConditionNode.variable2NameID;
            dropdownField2.OnValueChanged += evt => {
                _vector3ConditionNode.variable2NameID = evt;
                EditorUtility.SetDirty(_vector3ConditionNode);
            };
            root.Add(dropdownField2);

            Vector3Field vectorField = new Vector3Field();
            vectorField.value = _vector3ConditionNode.data2;
            vectorField.RegisterValueChangedCallback(evt => {
                _vector3ConditionNode.data2 = evt.newValue;
                EditorUtility.SetDirty(_vector3ConditionNode);
            });
            root.Add(vectorField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_vector3ConditionNode);
                if (active)
                {
                    vectorField.style.display = DisplayStyle.None;
                    dropdownField2.style.display = DisplayStyle.Flex;
                }
                else
                {
                    vectorField.style.display = DisplayStyle.Flex;
                    dropdownField2.style.display = DisplayStyle.None;
                }
            };

            action?.Invoke(_vector3ConditionNode.isActiveVariable2);

        }
    }
}
