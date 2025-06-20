using UnityEngine;

namespace PG.StorySystem
{
    [System.Serializable]
    public class Vector3Variable : StoryVariable
    {
        private Vector3 _vector3Value;
        public Vector3 vector3Value
        {
            get => _vector3Value;
            set
            {
                _vector3Value = value;
                OnValueChangedInvoke((Vector2)value);
            }
        }
        public override object GetValue()
        {
            return vector3Value;
        }
        public override string GetTypeName()
        {
            return "Vector 3";
        }
        public override void SetValue(object value)
        {
            vector3Value = (Vector3)value;
            OnValueChangedInvoke((Vector3)value);
        }
        public override string GetUndoText()
        {
            return "Story Graph (Create Vector 3 Variable)";
        }
    }
}
