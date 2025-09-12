namespace PG.StorySystem
{
    [System.Serializable]
    public class FloatVariable : StoryVariable
    {
        private float _floatValue;
        public float floatValue
        {
            get => _floatValue;
            set
            {
                _floatValue = value;
                OnValueChangedInvoke((float)value);
            }
        }
        public override object GetValue()
        {
            return floatValue;
        }
        public override string GetTypeName()
        {
            return "Float";
        }

        public override void SetValue(object value)
        {
            floatValue = (float)value;
            OnValueChangedInvoke((float)value);
        }
        public override string GetUndoText()
        {
            return "Story Graph (Create Float Variable)";
        }
    }
}
