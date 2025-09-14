namespace PG.StorySystem
{
    using Nodes;

    [CustomNodeView(typeof(LoadLevelNode))]
    public class LoadLevelNodeView : StoryNodeView
    {
        public LoadLevelNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            
            LoadLevelNode loadLevelNode = node as LoadLevelNode;
            updateNode += (node) =>
            {
                title = node.GetName() + $"({loadLevelNode.levelName})";
            };
            title = node.GetName() + $"({loadLevelNode.levelName})";
        }
    }
}
