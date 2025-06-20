using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(IntModificatorNode), true)]
    public class IntModificatorNodeEditor : StoryNodeEditor
    {
        private IntModificatorNode _intModificatorNode;
        protected override void Init()
        {
            base.Init();
            _intModificatorNode = target as IntModificatorNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.choices = variables;
            dropdownField.value = _intModificatorNode.storyVariableNameID;
            dropdownField.OnValueChanged += evt => {
                _intModificatorNode.storyVariableNameID = evt;
                EditorUtility.SetDirty(_intModificatorNode);
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _intModificatorNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                EditorUtility.SetDirty(_intModificatorNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.choices = variables;
            dropdownField2.value = _intModificatorNode.storyVariable2NameID;
            dropdownField2.OnValueChanged += evt => {
                _intModificatorNode.storyVariable2NameID = evt;
                EditorUtility.SetDirty(_intModificatorNode);
            };
            root.Add(dropdownField2);

            IntegerField intField = new IntegerField();
            intField.value = _intModificatorNode.dataInt2;
            intField.RegisterValueChangedCallback(evt => {
                _intModificatorNode.dataInt2 = evt.newValue;
                EditorUtility.SetDirty(_intModificatorNode);
            });
            root.Add(intField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_intModificatorNode);
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

            action?.Invoke(_intModificatorNode.isActiveVariable2);

        }
    }
}