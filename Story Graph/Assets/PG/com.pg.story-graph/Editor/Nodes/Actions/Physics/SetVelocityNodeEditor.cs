using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    [CustomEditor(typeof(SetVelocityNode), true)]
    internal class SetVelocityNodeEditor : ActionNodeEditor
    {
        private SetVelocityNode _setVelocityNode;
        protected override void Init()
        {
            base.Init();
            _setVelocityNode = target as SetVelocityNode;
        }
        public override void OnCustomElement(VisualElement root)
        {

            System.Action<bool> action = default;

            Toggle toggle = new Toggle("Use Direction From Object");
            toggle.value = _setVelocityNode.useDirectionFromObject;
            toggle.RegisterValueChangedCallback(evt => { 
                _setVelocityNode.useDirectionFromObject = evt.newValue;
                action?.Invoke(evt.newValue);
            });
            root.Add(toggle);

            Vector3Field velocityField = new Vector3Field();
            velocityField.value = _setVelocityNode.velocity;
            velocityField.RegisterValueChangedCallback(evt => {
                _setVelocityNode.velocity = evt.newValue;
            });
            root.Add(velocityField);

            root.Add(new Label("Target Object"));
            SearchableDropdownField targetObjectField = new SearchableDropdownField();
            targetObjectField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            targetObjectField.choices = objects;
            targetObjectField.value = _setVelocityNode.targetObjectNameID;
            targetObjectField.OnValueChanged += evt => { 
                _setVelocityNode.targetObjectID = objects.IndexOf(evt);
                _setVelocityNode.targetObjectNameID = evt;
            };
            root.Add(targetObjectField);

            FloatField speedField = new FloatField("Speed");
            speedField.value = _setVelocityNode.speed;
            speedField.RegisterValueChangedCallback(evt => {
                _setVelocityNode.speed = evt.newValue;
            });
            root.Add(speedField);


            action = v => {
                if (v)
                {
                    velocityField.style.display = DisplayStyle.None;
                    targetObjectField.style.display = DisplayStyle.Flex;
                    speedField.style.display = DisplayStyle.Flex;
                }
                else
                {
                    velocityField.style.display = DisplayStyle.Flex;
                    targetObjectField.style.display = DisplayStyle.None;
                    speedField.style.display = DisplayStyle.None;
                }
            };

            action?.Invoke(_setVelocityNode.useDirectionFromObject);
        }
    }
}