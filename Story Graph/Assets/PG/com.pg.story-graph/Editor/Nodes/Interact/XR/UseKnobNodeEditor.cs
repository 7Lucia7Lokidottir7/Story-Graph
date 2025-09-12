using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;

    [CustomEditor(typeof(UseKnobNode), true)]
    internal class UseKnobNodeEditor : InteractNodeEditor
    {
        private UseKnobNode _useKnobNode;
        protected override void Init()
        {
            base.Init();
            _useKnobNode = target as UseKnobNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);


            Slider minSlider = new Slider("Target Min Value");
            minSlider.lowValue = 0f;
            minSlider.highValue = 1f;
            minSlider.value = _useKnobNode.minValue;

            Slider maxSlider = new Slider("Target Max Value");
            maxSlider.lowValue = 0f;
            maxSlider.highValue = 1f;
            maxSlider.value = _useKnobNode.maxValue;


            minSlider.RegisterValueChangedCallback(evt => {
                if (_useKnobNode.maxValue < evt.newValue)
                {
                    minSlider.value = _useKnobNode.maxValue;
                }
                _useKnobNode.minValue = evt.newValue;
                EditorUtility.SetDirty(_useKnobNode);
            });
            root.Add(minSlider);

            maxSlider.RegisterValueChangedCallback(evt => {
                if (_useKnobNode.minValue > evt.newValue)
                {
                    maxSlider.value = _useKnobNode.minValue;
                }
                _useKnobNode.maxValue = evt.newValue;
                EditorUtility.SetDirty(_useKnobNode);
            });
            root.Add(maxSlider);
        }
    }
}