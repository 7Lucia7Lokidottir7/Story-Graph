using UnityEngine;
using UnityEditor;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
    [CustomEditor(typeof(InteractNode), true)]
    public class InteractNodeEditor : StoryNodeEditor
    {
        private InteractNode _interactNode;
        protected override void Init()
        {
            base.Init();
            _interactNode = target as InteractNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
        }
    }
}