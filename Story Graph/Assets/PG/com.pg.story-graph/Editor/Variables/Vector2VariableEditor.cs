using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    [CustomEditor(typeof(Vector2Variable), true)]
    public class Vector2VariableEditor : VariableEditor
    {
        public override VisualTreeAsset visualTreeAsset => (VisualTreeAsset)Resources.Load("StoryGraph/Variable/Vector2Variable");
        private Vector2Variable _vector2Variable;
        protected override void OnEnableData()
        {
            _vector2Variable = target as Vector2Variable;
        }
        protected override void SubData(VisualElement root)
        {
            base.SubData(root);
            Vector2Field integerField = root.Q<Vector2Field>();
            integerField.value = _vector2Variable.vector2Value;
            integerField.RegisterValueChangedCallback(evt => { _vector2Variable.vector2Value = evt.newValue; });
        }
    }
}
