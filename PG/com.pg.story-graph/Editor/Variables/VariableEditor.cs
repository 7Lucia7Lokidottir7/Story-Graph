using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem
{
    [CustomEditor(typeof(StoryVariable), true)]
    public class VariableEditor : Editor
    {
        private StoryVariable _variable;
        public virtual VisualTreeAsset visualTreeAsset => default;
        private void OnEnable()
        {
            _variable = target as StoryVariable;
            OnEnableData();
        }
        protected virtual void OnEnableData()
        {

        }
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = visualTreeAsset.CloneTree();
            SubData(root);
            return root;
        }
        protected virtual void SubData(VisualElement root)
        {
            Foldout foldout = root.Q<Foldout>();
            foldout.text = $"{_variable.variableName} ({_variable.GetType().Name})";

            TextField nameField = root.Q<TextField>();
            nameField.value = _variable.variableName;
            nameField.RegisterValueChangedCallback(evt => 
            { 
                _variable.variableName = evt.newValue;
                foldout.text = $"{_variable.variableName} ({_variable.GetType().Name})";
            });
        }
    }
}
