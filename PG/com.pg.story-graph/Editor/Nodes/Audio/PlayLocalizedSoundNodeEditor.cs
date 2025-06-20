using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(PlayLocalizedSoundNode))]
    public class PlayLocalizedSoundNodeEditor : StoryNodeEditor
    {
        private PlayLocalizedSoundNode _playSound;
        protected override void Init()
        {
            base.Init();
            _playSound = target as PlayLocalizedSoundNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            Label label = new Label("AudioSource Object");

            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.choices = objects;
            dropdownField.value = _playSound.audioSourceNameIndex;
            dropdownField.OnValueChanged += evt => {
                _playSound.audioSourceNameIndex = evt;
                EditorUtility.SetDirty(_playSound);
            };

            root.Add(label);
            root.Add(dropdownField);

            SerializedObject newSerializedObject = new SerializedObject(_playSound);
            PropertyField propertyField = new PropertyField(newSerializedObject.FindProperty("resources"));
            root.Add(propertyField);
        }
    }
}