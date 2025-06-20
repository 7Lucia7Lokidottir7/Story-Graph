using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(AlignByInternalPointsNode), true)]
    internal class AlignByInternalPointsNodeEditor : TransformNodeEditor
    {
        private AlignByInternalPointsNode _alignByInternalPointsNode;
        protected override string _targetObjectLabel => "Point On Target Object";
        protected override void Init()
        {
            base.Init();
            _alignByInternalPointsNode = target as AlignByInternalPointsNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            Label targetObjetctLabel2 = new Label("Aligment Point");
            root.Add(targetObjetctLabel2);
            SearchableDropdownField targetDropdownField2 = new SearchableDropdownField();
            targetDropdownField2.tooltip = "An object is selected from a list of objects that can be created or deleted in the Objects panel. \n\n To install an object on the scene, set the 'ObjectElement' component on the object and set the desired 'StoryGraph' inside, and then select the object from the list of objects.";
            targetDropdownField2.choices = objects;
            targetDropdownField2.value = _alignByInternalPointsNode.aligmentObjectNameID;
            targetDropdownField2.OnValueChanged += evt =>
            {
                _alignByInternalPointsNode.aligmentObjectNameID = evt;
                EditorUtility.SetDirty(_alignByInternalPointsNode);
            };
            root.Add(targetDropdownField2);
        }
    }
}
