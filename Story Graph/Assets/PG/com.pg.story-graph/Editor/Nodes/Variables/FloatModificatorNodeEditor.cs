using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(FloatModificatorNode), true)]
    public class FloatModificatorNodeEditor : StoryNodeEditor
    {
        private FloatModificatorNode _floatModificatorNode;
        protected override void Init()
        {
            base.Init();
            _floatModificatorNode = target as FloatModificatorNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            SearchableDropdownField  dropdownField = new SearchableDropdownField();
            dropdownField.choices = variables;
            dropdownField.value = _floatModificatorNode.storyVariableNameID;
            dropdownField.OnValueChanged += evt => {
                _floatModificatorNode.storyVariableNameID = evt;
                EditorUtility.SetDirty(_floatModificatorNode);
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _floatModificatorNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                EditorUtility.SetDirty(_floatModificatorNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.choices = variables;
            dropdownField2.value = _floatModificatorNode.storyVariable2NameID;
            dropdownField2.OnValueChanged += evt => {
                _floatModificatorNode.storyVariable2NameID = evt;
                EditorUtility.SetDirty(_floatModificatorNode);
            };
            root.Add(dropdownField2);

            FloatField floatField = new FloatField();
            floatField.value = _floatModificatorNode.dataFloat2;
            floatField.RegisterValueChangedCallback(evt => {
                EditorUtility.SetDirty(_floatModificatorNode);
                _floatModificatorNode.dataFloat2 = evt.newValue;
            });
            root.Add(floatField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_floatModificatorNode);
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

            action?.Invoke(_floatModificatorNode.isActiveVariable2);
        }
    }
}