using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    [CustomEditor(typeof(SetColorMaterialNode), true)]
    public class SetColorMaterialNodeEditor : StoryNodeEditor
    {
        private SetColorMaterialNode _setColorMaterialNode;
        protected override void Init()
        {
            base.Init();
            _setColorMaterialNode = target as SetColorMaterialNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);

            root.Add(new Label("Object"));
            SearchableDropdownField dropdownField = new SearchableDropdownField();
            dropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            dropdownField.choices = objects;
            dropdownField.value = _setColorMaterialNode.objectNameID;
            dropdownField.OnValueChanged += evt => {
                _setColorMaterialNode.objectNameID = evt;
                EditorUtility.SetDirty(_setColorMaterialNode);
            };
            root.Add(dropdownField);


            Toggle useLerp = new Toggle("Use Lerp");

            FloatField duration = new FloatField("Duration");
            duration.value = _setColorMaterialNode.duration;
            duration.RegisterValueChangedCallback(c => {
                _setColorMaterialNode.duration = c.newValue;
                EditorUtility.SetDirty(_setColorMaterialNode);
            });

            useLerp.value = _setColorMaterialNode.useLerp;
            useLerp.RegisterValueChangedCallback((c) =>
            {
                _setColorMaterialNode.useLerp = c.newValue;
                duration.style.display = c.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                EditorUtility.SetDirty(_setColorMaterialNode);

            });


            duration.value = _setColorMaterialNode.duration;
            duration.style.display = _setColorMaterialNode.useLerp ? DisplayStyle.Flex : DisplayStyle.None;


            root.Add(useLerp);
            root.Add(duration);
        }
    }
}