namespace PG.StorySystem
{
    public class HighlightEnableNode : HighlightNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _controller.isEnable = true;
            TransitionToNextNodes(storyGraph);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}