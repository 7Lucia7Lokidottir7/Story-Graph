using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    [CustomEditor(typeof(FloatVariable), true)]
    public class FloatVariableEditor : VariableEditor
    {
        public override VisualTreeAsset visualTreeAsset => (VisualTreeAsset)Resources.Load("StoryGraph/Variable/FloatVariable");
        private FloatVariable _floatVariable;
        protected override void OnEnableData()
        {
            _floatVariable = target as FloatVariable;
        }
        protected override void SubData(VisualElement root)
        {
            base.SubData(root);
            FloatField floatField = root.Q<FloatField>();
            floatField.value = _floatVariable.floatValue;
            floatField.RegisterValueChangedCallback(evt => { _floatVariable.floatValue = evt.newValue; });
        }
    }
}
