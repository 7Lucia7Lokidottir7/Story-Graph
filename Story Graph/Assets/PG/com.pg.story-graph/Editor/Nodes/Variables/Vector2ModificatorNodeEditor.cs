using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(Vector2ModificatorNode), true)]
    public class Vector2ModificatorNodeEditor : StoryNodeEditor
    {
        private Vector2ModificatorNode _vector2ModificatorNode;
        protected override void Init()
        {
            base.Init();
            _vector2ModificatorNode = target as Vector2ModificatorNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.choices = variables;
            dropdownField.value = _vector2ModificatorNode.storyVariableNameID;
            dropdownField.OnValueChanged += evt => {
                EditorUtility.SetDirty(_vector2ModificatorNode);
                _vector2ModificatorNode.storyVariableNameID = evt;
            };
            root.Add(dropdownField);


            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Is Active Variable 2");
            toggle.value = _vector2ModificatorNode.isActiveVariable2;
            toggle.RegisterValueChangedCallback(evt =>
            {
                EditorUtility.SetDirty(_vector2ModificatorNode);
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.choices = variables;
            dropdownField2.value = _vector2ModificatorNode.storyVariableNameID;
            dropdownField2.OnValueChanged += evt => {
                EditorUtility.SetDirty(_vector2ModificatorNode);
                _vector2ModificatorNode.storyVariable2NameID = evt;
            };
            root.Add(dropdownField2);

            Vector2Field vectorField = new Vector2Field();
            vectorField.value = _vector2ModificatorNode.dataVector2;
            vectorField.RegisterValueChangedCallback(evt => {
                EditorUtility.SetDirty(_vector2ModificatorNode);
                _vector2ModificatorNode.dataVector2 = evt.newValue;
            });
            root.Add(vectorField);


            action = (active) =>
            {
                EditorUtility.SetDirty(_vector2ModificatorNode);
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

            action?.Invoke(_vector2ModificatorNode.isActiveVariable2);

        }
    }
}