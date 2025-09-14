
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{

    [CustomNodeView(typeof(RestartToNode))]
    public class RestartToNodeView : StoryNodeView
    {
        public RestartToNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            
        }
        protected override void CreateOutputPorts()
        {
        }
    }
}