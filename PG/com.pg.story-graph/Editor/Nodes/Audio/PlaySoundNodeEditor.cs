using UnityEditor;
using UnityEngine;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(PlaySoundNode))]
    public class PlaySoundNodeEditor : StoryNodeEditor
    {
        private PlaySoundNode _playSound;
        protected override void Init()
        {
            base.Init();
            _playSound = target as PlaySoundNode;
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
                EditorUtility.SetDirty(_playSound);
                _playSound.audioSourceNameIndex = evt;
            };

            root.Add(label);
            root.Add(dropdownField);
        }
    }
}