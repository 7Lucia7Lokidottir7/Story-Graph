using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(AnimationNode), true)]
    internal class AnimationNodeEditor : StoryNodeEditor
    {
        private AnimationNode _animationNode;
        protected override void Init()
        {
            base.Init();
            _animationNode = target as AnimationNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.choices = objects;
            dropdownField.value = _animationNode.objectNameID;
            dropdownField.OnValueChanged += evt => {
                _animationNode.objectNameID = evt;

                EditorUtility.SetDirty(_animationNode);
            };
            root.Add(dropdownField);

            root.Add(new Label("Parameter"));
            TextField textField = new TextField();
            textField.value = _animationNode.parameter;
            textField.RegisterValueChangedCallback(evt => { _animationNode.parameter = evt.newValue; });
            root.Add(textField);

            root.Add(new Label("Type"));
            EnumField enumField = new EnumField(AnimationNode.AnimatorParameterType.Trigger);
            enumField.dataSourceType = typeof(AnimationNode.AnimatorParameterType);
            enumField.value = _animationNode.parameterType;
            root.Add(enumField);

            Toggle toggle = new Toggle();
            toggle.value = _animationNode.boolValue;
            toggle.RegisterValueChangedCallback(evt => { _animationNode.boolValue = evt.newValue; });
            root.Add(toggle);

            FloatField floatField = new FloatField();
            floatField.value = _animationNode.floatValue;
            floatField.RegisterValueChangedCallback(evt => { _animationNode.floatValue = evt.newValue; });
            root.Add(floatField);

            IntegerField intField = new IntegerField();
            intField.value = _animationNode.intValue;
            intField.RegisterValueChangedCallback(evt => { _animationNode.intValue = evt.newValue; });
            root.Add(intField);

            System.Action<AnimationNode.AnimatorParameterType> action = default;

            enumField.RegisterValueChangedCallback(evt => {
                _animationNode.parameterType = (AnimationNode.AnimatorParameterType)evt.newValue;
                EditorUtility.SetDirty(_animationNode);
                action?.Invoke((AnimationNode.AnimatorParameterType)evt.newValue);
            });

            action = evt =>
            {
                switch (evt)
                {
                    case AnimationNode.AnimatorParameterType.Trigger:
                        floatField.style.display = DisplayStyle.None;
                        intField.style.display = DisplayStyle.None;
                        toggle.style.display = DisplayStyle.None;
                        break;
                    case AnimationNode.AnimatorParameterType.Float:
                        floatField.style.display = DisplayStyle.Flex;
                        intField.style.display = DisplayStyle.None;
                        toggle.style.display = DisplayStyle.None;
                        break;
                    case AnimationNode.AnimatorParameterType.Int:
                        floatField.style.display = DisplayStyle.None;
                        intField.style.display = DisplayStyle.Flex;
                        toggle.style.display = DisplayStyle.None;
                        break;
                    case AnimationNode.AnimatorParameterType.Bool:
                        floatField.style.display = DisplayStyle.None;
                        intField.style.display = DisplayStyle.None;
                        toggle.style.display = DisplayStyle.Flex;
                        break;
                }
            };

            action?.Invoke(_animationNode.parameterType);
        }
    }
}