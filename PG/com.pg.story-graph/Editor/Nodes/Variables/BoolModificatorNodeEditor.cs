using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(BoolModificatorNode), true)]
    public class BoolModificatorNodeEditor : StoryNodeEditor
    {
        private BoolModificatorNode _boolModificatorNode;
        protected override void Init()
        {
            base.Init();
            _boolModificatorNode = target as BoolModificatorNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.choices = variables;
            dropdownField.value = _boolModificatorNode.storyVariableNameID;
            dropdownField.OnValueChanged += evt => {
                _boolModificatorNode.storyVariableNameID = evt;
                EditorUtility.SetDirty(_boolModificatorNode);
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _boolModificatorNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                EditorUtility.SetDirty(_boolModificatorNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.choices = variables;
            dropdownField2.value = _boolModificatorNode.storyVariable2NameID;
            dropdownField2.OnValueChanged += evt => {
                _boolModificatorNode.storyVariable2NameID = evt;
                EditorUtility.SetDirty(_boolModificatorNode);
            };
            root.Add(dropdownField2);

            Toggle vectorField = new Toggle();
            vectorField.value = _boolModificatorNode.dataBool2;
            vectorField.RegisterValueChangedCallback(evt => { _boolModificatorNode.dataBool2 = evt.newValue; });
            root.Add(vectorField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_boolModificatorNode);
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

            action?.Invoke(_boolModificatorNode.isActiveVariable2);
        }
    }
}