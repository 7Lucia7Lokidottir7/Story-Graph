using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(IntConditionNode),true)]
    public class IntConditionNodeEditor : StoryNodeEditor
    {
        private IntConditionNode _intConditionNode;
        protected override void Init()
        {
            base.Init();
            _intConditionNode = (IntConditionNode)target;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Variable"));
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
            dropdownField.choices = variables;
            dropdownField.value = _intConditionNode.intVariableNameID;
            dropdownField.OnValueChanged += evt => {
                EditorUtility.SetDirty(_intConditionNode);
                _intConditionNode.intVariableNameID = evt;
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _intConditionNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                _intConditionNode.isActiveVariable2 = evt.newValue;
                EditorUtility.SetDirty(_intConditionNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
            dropdownField2.choices = variables;
            dropdownField2.value = _intConditionNode.intVariableNameID;
            dropdownField2.OnValueChanged += evt => {
                EditorUtility.SetDirty(_intConditionNode);
                _intConditionNode.intVariable2NameID = evt;
            };
            root.Add(dropdownField2);

            IntegerField intField = new IntegerField();
            intField.value = _intConditionNode.data2;
            intField.RegisterValueChangedCallback(evt => {
                EditorUtility.SetDirty(_intConditionNode);
                _intConditionNode.data2 = evt.newValue;
            });
            root.Add(intField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_intConditionNode);
                if (active)
                {
                    intField.style.display = DisplayStyle.None;
                    dropdownField2.style.display = DisplayStyle.Flex;
                }
                else
                {
                    intField.style.display = DisplayStyle.Flex;
                    dropdownField2.style.display = DisplayStyle.None;
                }
            };

            action?.Invoke(_intConditionNode.isActiveVariable2);

        }
    }
}
