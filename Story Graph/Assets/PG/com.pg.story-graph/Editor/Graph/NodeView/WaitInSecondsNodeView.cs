using UnityEngine;
namespace PG.StorySystem
{
    using Nodes;
    [CustomNodeView(typeof(WaitInSecondsNode))]
    public class WaitInSecondsNodeView : StoryNodeView
    {
        public WaitInSecondsNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            SetupClasses();
            WaitInSecondsNode waitInSecondsNode = node as WaitInSecondsNode;
            updateNode += (node) =>
            {
                title = node.GetName() + $"({waitInSecondsNode.duration})";
            };
            title = node.GetName() + $"({waitInSecondsNode.duration})";
        }
    }
}
