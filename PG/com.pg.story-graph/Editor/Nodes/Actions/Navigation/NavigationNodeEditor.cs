using UnityEditor;
namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEngine.UIElements;
    [CustomEditor(typeof(NavigationNode), true)]
    internal class NavigationNodeEditor : ActionNodeEditor
    {
        private NavigationNode _navigationNode;
        protected override void Init()
        {
            base.Init();
            _navigationNode = target as NavigationNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            root.Add(new Label("Target Point"));
            SearchableDropdownField targetDropdownField = new SearchableDropdownField();
            targetDropdownField.value = _navigationNode.targetObjectNameID;
            targetDropdownField.choices = objects;
            targetDropdownField.OnValueChanged += evt => {
                _navigationNode.targetObjectNameID = evt;
                EditorUtility.SetDirty(_navigationNode);
            };
            root.Add(targetDropdownField);
        }
    }
}
