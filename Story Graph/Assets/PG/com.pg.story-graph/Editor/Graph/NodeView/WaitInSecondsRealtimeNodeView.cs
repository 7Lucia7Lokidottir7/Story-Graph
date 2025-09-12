namespace PG.StorySystem
{
    using Nodes;

    [CustomNodeView(typeof(WaitInSecondsRealtimeNode))]
    public class WaitInSecondsRealtimeNodeView : StoryNodeView
    {
        public WaitInSecondsRealtimeNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            SetupClasses();
            WaitInSecondsRealtimeNode waitInSecondsNode = node as WaitInSecondsRealtimeNode;
            updateNode += (node) =>
            {
                title = node.GetName() + $"({waitInSecondsNode.duration})";
            };
            title = node.GetName() + $"({waitInSecondsNode.duration})";
        }
    }
}
