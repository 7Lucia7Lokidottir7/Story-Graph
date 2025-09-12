using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    [CustomEditor(typeof(Vector3Variable), true)]
    public class Vector3VariableEditor : VariableEditor
    {
        public override VisualTreeAsset visualTreeAsset => (VisualTreeAsset)Resources.Load("StoryGraph/Variable/Vector3Variable");
        private Vector3Variable _vector3Variable;
        protected override void OnEnableData()
        {
            _vector3Variable = target as Vector3Variable;
        }
        protected override void SubData(VisualElement root)
        {
            base.SubData(root);
            Vector3Field vector3Field = root.Q<Vector3Field>();
            vector3Field.value = _vector3Variable.vector3Value;
            vector3Field.RegisterValueChangedCallback(evt => { _vector3Variable.vector3Value = evt.newValue; });
        }
    }
}
