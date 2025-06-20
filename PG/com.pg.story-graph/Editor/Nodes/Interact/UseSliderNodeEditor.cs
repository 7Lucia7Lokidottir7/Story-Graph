using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;

    [CustomEditor(typeof(UseSliderNode), true)]
    internal class UseSliderNodeEditor : InteractNodeEditor
    {
        private UseSliderNode _useSliderNode;
        protected override void Init()
        {
            base.Init();
            _useSliderNode = target as UseSliderNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);


            Slider minSlider = new Slider("Target Min Value");
            minSlider.lowValue = 0f;
            minSlider.highValue = 1f;
            minSlider.value = _useSliderNode.minValue;

            Slider maxSlider = new Slider("Target Max Value");
            maxSlider.lowValue = 0f;
            maxSlider.highValue = 1f;
            maxSlider.value = _useSliderNode.maxValue;


            minSlider.RegisterValueChangedCallback(evt => {
                if (_useSliderNode.maxValue < evt.newValue)
                {
                    minSlider.value = _useSliderNode.maxValue;
                }
                _useSliderNode.minValue = evt.newValue;
                EditorUtility.SetDirty(_useSliderNode);
            });
            root.Add(minSlider);

            maxSlider.RegisterValueChangedCallback(evt => {
                if (_useSliderNode.minValue > evt.newValue)
                {
                    maxSlider.value = _useSliderNode.minValue;
                }
                _useSliderNode.maxValue = evt.newValue;
                EditorUtility.SetDirty(_useSliderNode);
            });
            root.Add(maxSlider);
        }
    }
}