using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    public partial class Vector3ConditionNodeEditor
    {
        [CustomEditor(typeof(Vector2ConditionNode), true)]
        public class Vector2ConditionNodeEditor : StoryNodeEditor
        {
            private Vector2ConditionNode _vector2ConditionNode;
            protected override void Init()
            {
                base.Init();
                _vector2ConditionNode = (Vector2ConditionNode)target;
            }
            public override void OnCustomElement(VisualElement root)
            {
                base.OnCustomElement(root);

                root.Add(new Label("Variable"));
                SearchableDropdownField dropdownField = new SearchableDropdownField();
                dropdownField.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
                dropdownField.choices = variables;
                dropdownField.value = _vector2ConditionNode.variable1NameID;
                dropdownField.OnValueChanged += evt => {
                    _vector2ConditionNode.variable1NameID = evt;
                    EditorUtility.SetDirty(_vector2ConditionNode);
                };
                root.Add(dropdownField);


                System.Action<bool> action = default;

                Toggle toggle = new Toggle("Is Active Variable 2");
                toggle.value = _vector2ConditionNode.isActiveVariable2;
                toggle.RegisterValueChangedCallback(evt =>
                {
                    EditorUtility.SetDirty(_vector2ConditionNode);
                    _vector2ConditionNode.isActiveVariable2 = evt.newValue;
                    action?.Invoke(evt.newValue);
                });
                root.Add(toggle);

                SearchableDropdownField dropdownField2 = new SearchableDropdownField();
                dropdownField2.tooltip = "The variable is selected from a list of objects that can be created or deleted in the Variables panel.";
                dropdownField2.choices = variables;
                dropdownField2.value = _vector2ConditionNode.variable2NameID;
                dropdownField2.OnValueChanged += evt => {
                    _vector2ConditionNode.variable2NameID = evt;
                    EditorUtility.SetDirty(_vector2ConditionNode);
                };
                root.Add(dropdownField2);

                Vector2Field vectorField = new Vector2Field();
                vectorField.value = _vector2ConditionNode.data2;
                vectorField.RegisterValueChangedCallback(evt => {
                    EditorUtility.SetDirty(_vector2ConditionNode);
                    _vector2ConditionNode.data2 = evt.newValue;
                });
                root.Add(vectorField);


                action = (active) =>
                {
                    EditorUtility.SetDirty(_vector2ConditionNode);
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

                action?.Invoke(_vector2ConditionNode.isActiveVariable2);

            }
        }
    }
}
