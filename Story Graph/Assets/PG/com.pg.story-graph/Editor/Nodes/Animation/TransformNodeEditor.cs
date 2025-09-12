using UnityEngine;
using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(TransformNode), true)]
    internal class TransformNodeEditor : ActionNodeEditor
    {
        private TransformNode _transformNode;
        protected virtual string _targetObjectLabel => "Target Object";
        protected override void Init()
        {
            base.Init();
            _transformNode = target as TransformNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            Label targetObjetctLabel = new Label(_targetObjectLabel);
            root.Add(targetObjetctLabel);
            SearchableDropdownField targetDropdownField = new SearchableDropdownField();
            targetDropdownField.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            targetDropdownField.choices = objects;
            targetDropdownField.value = _transformNode.targetObjectNameID;
            targetDropdownField.OnValueChanged += evt =>
            {
                _transformNode.targetObjectNameID = evt;
                EditorUtility.SetDirty(_transformNode);
            };
            root.Add(targetDropdownField);
        }
    }
}
