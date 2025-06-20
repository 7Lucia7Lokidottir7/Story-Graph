using UnityEditor;
using UnityEngine.UIElements;
namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;

    [CustomEditor(typeof(CheckDistanceObjectsConditionNode), true)]
    public class CheckDistanceObjectsConditionNodeEditor : StoryNodeEditor
    {
        private CheckDistanceObjectsConditionNode _checkDistanceObjectsNode;

        protected override void Init()
        {
            base.Init();
            _checkDistanceObjectsNode = target as CheckDistanceObjectsConditionNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Object"));
            SearchableDropdownField targetObject = new SearchableDropdownField();
            targetObject.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            targetObject.choices = objects;
            targetObject.value = _checkDistanceObjectsNode.objectNameID;

            targetObject.OnValueChanged += evt => { 
                _checkDistanceObjectsNode.objectNameID = evt; 
            };
            root.Add(targetObject);

            Toggle toggle = new Toggle();
            toggle.text = "Use Target Object";
            toggle.value = _checkDistanceObjectsNode.useTargetObject;

            System.Action action = default;

            toggle.RegisterValueChangedCallback((evt) =>
            {
                _checkDistanceObjectsNode.useTargetObject = evt.newValue;
                action?.Invoke();
            });
            root.Add(toggle);

            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.choices = objects;
            dropdownField.value = _checkDistanceObjectsNode.targetObjectNameID;
            dropdownField.OnValueChanged += evt =>
            {
                _checkDistanceObjectsNode.targetObjectNameID = evt;
            };
            root.Add(dropdownField);

            Vector3Field vector3Field = new Vector3Field();
            vector3Field.value = _checkDistanceObjectsNode.targetVector;
            vector3Field.RegisterValueChangedCallback(evt =>
            {
                _checkDistanceObjectsNode.targetVector = evt.newValue;
            });
            root.Add(vector3Field);


            action = () =>
            {
                if (_checkDistanceObjectsNode.useTargetObject)
                {
                    dropdownField.style.display = DisplayStyle.Flex;
                    vector3Field.style.display = DisplayStyle.None;
                }
                else
                {
                    dropdownField.style.display = DisplayStyle.None;
                    vector3Field.style.display = DisplayStyle.Flex;
                }
            };

            action?.Invoke();

        }
    }
}
