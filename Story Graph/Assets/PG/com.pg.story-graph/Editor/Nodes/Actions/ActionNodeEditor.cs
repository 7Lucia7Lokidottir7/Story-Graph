using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(ActionNode), true)]
    internal class ActionNodeEditor : StoryNodeEditor
    {
        private ActionNode _actionNode;
        protected virtual string _objectLabel => "Object:";
        protected override void Init()
        {
            base.Init();
            _actionNode = target as ActionNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
        }
    }
}