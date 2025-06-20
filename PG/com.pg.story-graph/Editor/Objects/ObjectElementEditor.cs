using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    [CustomEditor(typeof(ObjectElement))]
    public class ObjectElementEditor : Editor
    {
        private ObjectElement _element;
        [SerializeField] private VisualTreeAsset _treeAsset;
        private void OnEnable()
        {
            _element = (ObjectElement)target;
        }
        public override VisualElement CreateInspectorGUI()
        {
            if (_treeAsset == null)
            {
                return base.CreateInspectorGUI();
            }
            VisualElement root = _treeAsset.CloneTree();


            SearchableDropdownField searchableDropdownField = root.Q<SearchableDropdownField>();
            searchableDropdownField.value = _element.objectNameID;
            searchableDropdownField.OnValueChanged += evt =>
            {
                EditorUtility.SetDirty(_element);
                _element.objectNameID = evt;
            };

            ObjectField objectField = root.Q<ObjectField>();
            objectField.objectType = typeof(StoryGraph);
            objectField.RegisterValueChangedCallback(evt =>
            {
                EditorUtility.SetDirty(_element);
                if (evt.newValue != null)
                {
                    searchableDropdownField.choices = _element.graph.objects;
                    searchableDropdownField.value = _element.objectNameID;
                } 
            });

            TextField textField = root.Q<TextField>();
            Button button = root.Q<Button>();
            Toggle toggle = root.Q<Toggle>("ClearTextAfterCreateToggle");
            Toggle applyNameToggle = root.Q<Toggle>("ApplyNameObjectToThisObjectToggle");

            button.clicked += () => CreateObject(textField, toggle, applyNameToggle, searchableDropdownField);

            return root;
        }
        void CreateObject(TextField textField, Toggle toggle, Toggle applyNameToggle, SearchableDropdownField searchableDropdownField)
        {
            if (_element.graph != null)
            {
                if (!string.IsNullOrEmpty(textField.value) && !string.IsNullOrWhiteSpace(textField.value))
                {
                    _element.graph.objects.Add(textField.value);
                    if (applyNameToggle.value)
                    {
                        string[] parts = textField.value.Split('/');
                        _element.objectNameID = parts[parts.Length-1].Trim();

                        searchableDropdownField.value = _element.objectNameID;
                    }
                    if (toggle.value)
                    {
                        textField.value = "";
                    }
                }
            }
        }
    }
}

