namespace PG.StorySystem
{
    [System.Serializable]
    public class BoolVariable : StoryVariable
    {
        private bool _boolValue;
        public bool boolValue
        {
            get => _boolValue;
            set
            {
                _boolValue = value;
                OnValueChangedInvoke((bool)value);
            }
        }
        public override object GetValue()
        {
            return boolValue;
        }
        public override string GetTypeName()
        {
            return "Bool";
        }
        public override void SetValue(object value)
        {
            boolValue = (bool)value;
            OnValueChangedInvoke((bool)value);
        }
        public override string GetUndoText()
        {
            return "Story Graph (Create Bool Variable)";
        }
    }
}
