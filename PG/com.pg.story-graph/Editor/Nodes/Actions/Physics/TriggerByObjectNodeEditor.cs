using UnityEngine;
using UnityEditor;
namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(TriggerByObjectNode), true)]
    internal class TriggerByObjectNodeEditor : ActionNodeEditor
    {
        private TriggerByObjectNode _triggerByObjectNode;
        protected override void Init()
        {
            base.Init();
            _triggerByObjectNode = target as TriggerByObjectNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Target Object"));
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.choices = objects;
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.value = _triggerByObjectNode.targetObjectNameID;
            dropdownField.OnValueChanged += evt => {
                _triggerByObjectNode.targetObjectNameID = evt; 
            };
            root.Add(dropdownField);
        }
    }
}
