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