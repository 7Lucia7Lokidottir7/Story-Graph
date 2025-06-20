using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(BoolConditionNode), true)]
    public class BoolConditionNodeEditor : StoryNodeEditor
    {
        private BoolConditionNode _boolConditionNode;
        protected override void Init()
        {
            base.Init();
            _boolConditionNode = target as BoolConditionNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Variable"));
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
            dropdownField.choices = variables;
            dropdownField.value = _boolConditionNode.boolVariableNameID;
            dropdownField.OnValueChanged += evt => {
                EditorUtility.SetDirty(_boolConditionNode);
                _boolConditionNode.boolVariableNameID = evt;
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _boolConditionNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                _boolConditionNode.isActiveVariable2 = evt.newValue;
                EditorUtility.SetDirty(_boolConditionNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
            dropdownField2.choices = variables;
            dropdownField2.value = _boolConditionNode.boolVariable2NameID;
            dropdownField2.OnValueChanged += evt => {
                EditorUtility.SetDirty(_boolConditionNode);
                _boolConditionNode.boolVariable2NameID = evt;
            };
            root.Add(dropdownField2);

            Toggle toggleField = new Toggle();
            toggleField.value = _boolConditionNode.data2;
            toggleField.RegisterValueChangedCallback(evt => {
                EditorUtility.SetDirty(_boolConditionNode);
                _boolConditionNode.data2 = evt.newValue;
            });
            root.Add(toggleField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_boolConditionNode);
                if (active)
                {
                    toggleField.style.display = DisplayStyle.None;
                    dropdownField2.style.display = DisplayStyle.Flex;
                }
                else
                {
                    toggleField.style.display = DisplayStyle.Flex;
                    dropdownField2.style.display = DisplayStyle.None;
                }
            };

            action?.Invoke(_boolConditionNode.isActiveVariable2);

        }
    }
}
