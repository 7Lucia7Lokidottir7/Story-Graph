using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    [CustomEditor(typeof(BoolVariable), true)]
    public class BoolVariableEditor : VariableEditor
    {
        public override VisualTreeAsset visualTreeAsset => (VisualTreeAsset)Resources.Load("StoryGraph/Variable/BoolVariable");
        private BoolVariable _boolVariable;
        protected override void OnEnableData()
        {
            _boolVariable = target as BoolVariable;
        }
        protected override void SubData(VisualElement root)
        {
            base.SubData(root);
            Toggle toggle = root.Q<Toggle>("BoolToggle");
            toggle.value = _boolVariable.boolValue;
            toggle.RegisterValueChangedCallback(evt => { _boolVariable.boolValue = evt.newValue; });
        }
    }
}
