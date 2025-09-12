using UnityEngine;
#if UNITY_EDITOR
#endif
namespace PG.StorySystem
{
    [System.Serializable]
    public class StoryVariable : ScriptableObject
    {
        public string variableName;
        public event System.Action<object> onValueChanged;
        protected void OnValueChangedInvoke(object value)
        {
            onValueChanged?.Invoke(value);
        }
        public virtual object GetValue()
        {
            return null;
        }
        public virtual void SetValue(object value)
        {
            onValueChanged?.Invoke(value);
        }

        public virtual string GetTypeName()
        {
            return "";
        }
        public virtual string GetUndoText()
        {
            return "";
        }
    }
}
