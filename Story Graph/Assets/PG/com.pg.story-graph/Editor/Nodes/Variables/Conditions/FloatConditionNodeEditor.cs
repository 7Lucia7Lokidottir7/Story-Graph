using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(FloatConditionNode), true)]
    public class FloatConditionNodeEditor : StoryNodeEditor
    {
        private FloatConditionNode _floatConditionNode;
        protected override void Init()
        {
            base.Init();
            _floatConditionNode = target as FloatConditionNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Variable"));
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
            dropdownField.choices = variables;
            dropdownField.value = _floatConditionNode.variable1NameID;
            dropdownField.OnValueChanged += evt => {
                EditorUtility.SetDirty(_floatConditionNode);
                _floatConditionNode.variable1NameID = evt;
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _floatConditionNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                _floatConditionNode.isActiveVariable2 = evt.newValue;
                EditorUtility.SetDirty(_floatConditionNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
            dropdownField2.choices = variables;
            dropdownField2.value = _floatConditionNode.variable2NameID;
            dropdownField2.OnValueChanged += evt => {
                EditorUtility.SetDirty(_floatConditionNode);
                _floatConditionNode.variable2NameID = evt;
            };
            root.Add(dropdownField2);

            FloatField floatField = new FloatField();
            floatField.value = _floatConditionNode.data2;
            floatField.RegisterValueChangedCallback(evt => {
                EditorUtility.SetDirty(_floatConditionNode);
                _floatConditionNode.data2 = evt.newValue;
            });
            root.Add(floatField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_floatConditionNode);
                if (active)
                {
                    floatField.style.display = DisplayStyle.None;
                    dropdownField2.style.display = DisplayStyle.Flex;
                }
                else
                {
                    floatField.style.display = DisplayStyle.Flex;
                    dropdownField2.style.display = DisplayStyle.None;
                }
            };

            action?.Invoke(_floatConditionNode.isActiveVariable2);

        }
    }
}
