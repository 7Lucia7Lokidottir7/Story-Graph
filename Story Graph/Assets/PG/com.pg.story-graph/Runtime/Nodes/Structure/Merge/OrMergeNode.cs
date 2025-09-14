namespace PG.StorySystem.Nodes
{
    public class OrMergeNode : MergeNode
    {
        public bool isFinished;
        protected override void Init(StoryGraph storyGraph)
        {
            isFinished = false;
        }
        public override void RestartNode(StoryGraph storyGraph)
        {
            base.RestartNode(storyGraph);
            isFinished = false;
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isFinished)
            {
                return;
            }
            isFinished = true;
            TransitionToNextNodes(storyGraph);
        }
    }
}
