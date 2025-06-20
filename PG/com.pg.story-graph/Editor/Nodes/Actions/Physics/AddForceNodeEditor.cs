using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(AddForceNode), true)]
    internal class AddForceNodeEditor : ActionNodeEditor
    {
        private AddForceNode _addForceNode;
        protected override void Init()
        {
            base.Init();
            _addForceNode = target as AddForceNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Use Direction From Object");
            toggle.value = _addForceNode.useDirectionFromObject;
            toggle.RegisterValueChangedCallback(evt => { 
                _addForceNode.useDirectionFromObject = evt.newValue;
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            Vector3Field directionField = new Vector3Field();
            directionField.value = _addForceNode.direction;
            directionField.RegisterValueChangedCallback(evt => {
                _addForceNode.direction = evt.newValue;
            });
            root.Add(directionField);

            root.Add(new Label("Target Object"));
            SearchableDropdownField targetObjectField = new SearchableDropdownField();
            targetObjectField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            targetObjectField.choices = objects;
            targetObjectField.value = _addForceNode.targetObjectNameID;
            targetObjectField.OnValueChanged += evt => {
                _addForceNode.targetObjectNameID = evt;
            };
            root.Add(targetObjectField);

            FloatField powerField = new FloatField("Power");
            powerField.value = _addForceNode.power;
            powerField.RegisterValueChangedCallback(evt => {
                _addForceNode.power = evt.newValue;
            });
            root.Add(powerField);


            action = v => {
                if (v)
                {
                    directionField.style.display = DisplayStyle.None;
                    targetObjectField.style.display = DisplayStyle.Flex;
                }
                else
                {
                    directionField.style.display = DisplayStyle.Flex;
                    targetObjectField.style.display = DisplayStyle.None;
                }
            };

            action?.Invoke(_addForceNode.useDirectionFromObject);
        }
    }
}