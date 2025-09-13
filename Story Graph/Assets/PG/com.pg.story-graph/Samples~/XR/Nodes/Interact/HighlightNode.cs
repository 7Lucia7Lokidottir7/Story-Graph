namespace PG.StorySystem
{
    using Nodes;

    public abstract class HighlightNode : InteractNode
    {
        protected HighlightObjectController _controller;
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _controller);
        }
    }
}