using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(ActionNode), true)]
    internal class ActionNodeEditor : StoryNodeEditor
    {
        private ActionNode _actionNode;
        protected virtual string _objectLabel => "Object:";
        protected override void Init()
        {
            base.Init();
            _actionNode = target as ActionNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            Label label = new Label(_objectLabel);
            SearchableDropdownField dropdownField = new SearchableDropdownField(objects, _actionNode.objectNameID);

            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.OnValueChanged += evt =>
            {
                Undo.RecordObject(_actionNode, "Changed Object");
                _actionNode.objectNameID = evt;
                EditorUtility.SetDirty(_actionNode);
            };



            root.Add(label);
            root.Add(dropdownField);
        }
    }
}