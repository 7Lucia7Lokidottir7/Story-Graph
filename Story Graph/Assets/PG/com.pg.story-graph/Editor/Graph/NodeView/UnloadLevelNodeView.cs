namespace PG.StorySystem
{
    using Nodes;

    [CustomNodeView(typeof(UnloadLevelNode))]
    public class UnloadLevelNodeView : StoryNodeView
    {
        public UnloadLevelNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            
            UnloadLevelNode unloadLevelNode = node as UnloadLevelNode;
            updateNode += (node) =>
            {
                title = node.GetName() + $"({unloadLevelNode.levelName})";
            };
            title = node.GetName() + $"({unloadLevelNode.levelName})";
        }
    }
}
