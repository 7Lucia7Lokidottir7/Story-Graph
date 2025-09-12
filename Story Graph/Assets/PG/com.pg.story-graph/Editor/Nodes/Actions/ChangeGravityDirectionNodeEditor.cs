using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(ChangeGravityDirectionNode), true)]
    internal class ChangeGravityDirectionNodeEditor : ActionNodeEditor
    {
        private ChangeGravityDirectionNode _changeGravityDirectionNode;
        protected override void Init()
        {
            _changeGravityDirectionNode = target as ChangeGravityDirectionNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
        }
    }
}