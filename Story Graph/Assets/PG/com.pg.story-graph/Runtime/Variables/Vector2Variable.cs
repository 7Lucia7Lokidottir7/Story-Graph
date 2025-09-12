using UnityEngine;

namespace PG.StorySystem
{
    [System.Serializable]
    public class Vector2Variable : StoryVariable
    {
        private Vector2 _vector2Value;
        public Vector2 vector2Value
        {
            get => _vector2Value;
            set
            {
                _vector2Value = value;
                OnValueChangedInvoke((Vector2)value);
            }
        }
        public override object GetValue()
        {
            return vector2Value;
        }
        public override string GetTypeName()
        {
            return "Vector 2";
        }
        public override void SetValue(object value)
        {
            vector2Value = (Vector2)value;
            OnValueChangedInvoke((Vector2)value);
        }
        public override string GetUndoText()
        {
            return "Story Graph (Create Vector 2 Variable)";
        }
    }
}
