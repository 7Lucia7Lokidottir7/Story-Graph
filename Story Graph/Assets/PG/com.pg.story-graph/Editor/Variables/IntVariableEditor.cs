using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    [CustomEditor(typeof(IntVariable), true)]
    public class IntVariableEditor : VariableEditor
    {
        public override VisualTreeAsset visualTreeAsset => (VisualTreeAsset)Resources.Load("StoryGraph/Variable/IntVariable");
        private IntVariable _intVariable;
        protected override void OnEnableData()
        {
            _intVariable = target as IntVariable;
        }
        protected override void SubData(VisualElement root)
        {
            base.SubData(root);
            IntegerField integerField = root.Q<IntegerField>();
            integerField.value = _intVariable.intValue;
            integerField.RegisterValueChangedCallback(evt => { _intVariable.intValue = evt.newValue; });
        }
    }
}
