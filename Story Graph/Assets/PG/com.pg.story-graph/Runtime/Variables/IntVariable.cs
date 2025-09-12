using UnityEngine;

namespace PG.StorySystem
{
    [System.Serializable]
    public class IntVariable : StoryVariable
    {
        private int _intValue;
        public int intValue
         { get => _intValue;
            set
            {
                _intValue = value;

                OnValueChangedInvoke((int)value);
            }
        }
        public override object GetValue()
        {
            return intValue;
        }
        public override string GetTypeName()
        {
            return "Int";
        }
        public override void SetValue(object value)
        {
            intValue = (int)value;
            OnValueChangedInvoke((int)value);
        }
        public override string GetUndoText()
        {
            return "Story Graph (Create Int Variable)";
        }
    }
}
