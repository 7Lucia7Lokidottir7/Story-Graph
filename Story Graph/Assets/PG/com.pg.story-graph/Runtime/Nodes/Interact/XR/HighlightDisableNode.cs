namespace PG.StorySystem
{
    public class HighlightDisableNode : HighlightNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _controller.SetHighlightInactive();
            _controller.isEnable = false;
            TransitionToNextNodes(storyGraph);
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {

        }
    }
}