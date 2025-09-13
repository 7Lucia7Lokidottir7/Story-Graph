using UnityEngine;
using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
    [CustomEditor(typeof(InteractNode), true)]
    public class InteractNodeEditor : StoryNodeEditor
    {
        private InteractNode _interactNode;
        protected override void Init()
        {
            base.Init();
            _interactNode = target as InteractNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            Label label = new Label("Object:");
            SearchableDropdownField searchableDropdownField = new SearchableDropdownField(objects, _interactNode.objectNameID);
            searchableDropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            searchableDropdownField.OnValueChanged += evt =>
            {
                _interactNode.objectNameID = evt;
                EditorUtility.SetDirty(_interactNode);
            };

            root.Add(label);
            root.Add(searchableDropdownField);

        }
    }
}