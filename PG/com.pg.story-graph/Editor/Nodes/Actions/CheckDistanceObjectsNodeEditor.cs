using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(CheckDistanceObjectsNode), true)]
    internal class CheckDistanceObjectsNodeEditor : ActionNodeEditor
    {
        private CheckDistanceObjectsNode _checkDistanceObjectsNode;
        protected override void Init()
        {
            base.Init();
            _checkDistanceObjectsNode = target as CheckDistanceObjectsNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Target Object"));
            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField2.choices = objects;
            dropdownField2.value = _checkDistanceObjectsNode.targetObjectNameID;
            dropdownField2.OnValueChanged += evt =>
            {
                _checkDistanceObjectsNode.targetObjectNameID = evt;
                EditorUtility.SetDirty(_checkDistanceObjectsNode);
            };

            root.Add(dropdownField2);
        }
    }
}
