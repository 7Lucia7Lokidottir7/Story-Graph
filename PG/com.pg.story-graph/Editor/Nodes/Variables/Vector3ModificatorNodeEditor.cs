using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(Vector3ModificatorNode), true)]
    public class Vector3ModificatorNodeEditor : StoryNodeEditor
    {
        private Vector3ModificatorNode _vector3ModificatorNode;
        protected override void Init()
        {
            base.Init();
            _vector3ModificatorNode = target as Vector3ModificatorNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.choices = variables;
            dropdownField.value = _vector3ModificatorNode.storyVariableNameID;
            dropdownField.OnValueChanged += evt => {
                EditorUtility.SetDirty(_vector3ModificatorNode);
                _vector3ModificatorNode.storyVariableNameID = evt;
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _vector3ModificatorNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.choices = variables;
            dropdownField2.value = _vector3ModificatorNode.storyVariable2NameID;
            dropdownField2.OnValueChanged += evt => {
                EditorUtility.SetDirty(_vector3ModificatorNode);
                _vector3ModificatorNode.storyVariable2NameID = evt;
            };
            root.Add(dropdownField2);

            Vector3Field vectorField = new Vector3Field();
            vectorField.value = _vector3ModificatorNode.dataVector2;
            vectorField.RegisterValueChangedCallback(evt => {
                EditorUtility.SetDirty(_vector3ModificatorNode);
                _vector3ModificatorNode.dataVector2 = evt.newValue;
            });
            root.Add(vectorField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_vector3ModificatorNode);
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

            action?.Invoke(_vector3ModificatorNode.isActiveVariable2);
        }
    }
}