using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(ObjectToSocketInteractorNode), true)]
    internal class ObjectToSocketInteractorNodeEditor : InteractNodeEditor
    {
        private ObjectToSocketInteractorNode _objectToSocketInteractorNode;
        protected override void Init()
        {
            base.Init();
            _objectToSocketInteractorNode = target as ObjectToSocketInteractorNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            Label label = new Label("SocketInteractor Object:");
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";

            dropdownField.choices = objects;
            dropdownField.value = _objectToSocketInteractorNode.objectNameID;
            dropdownField.OnValueChanged += evt =>
            {
                _objectToSocketInteractorNode.objectNameID = evt;
                EditorUtility.SetDirty(_objectToSocketInteractorNode);
            };

            Label label2 = new Label("Object:");
            SearchableDropdownField dropdownField2 = new SearchableDropdownField();
            dropdownField2.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";

            dropdownField2.choices = objects;
            dropdownField2.value = _objectToSocketInteractorNode.targetObjectNameID;
            dropdownField2.OnValueChanged += evt =>
            {
                _objectToSocketInteractorNode.targetObjectNameID = evt;
                EditorUtility.SetDirty(_objectToSocketInteractorNode);
            };



            root.Add(label);
            root.Add(dropdownField);
            root.Add(label2);
            root.Add(dropdownField2);
        }
    }

}