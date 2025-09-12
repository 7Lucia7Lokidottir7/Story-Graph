namespace PG.StorySystem.Nodes
{
    public class ExclusiveOrConditionNode : LogicalOperatorNode
    {
        public override bool GetResult()
        {
            return condition.GetResult() ^ condition2.GetResult();
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
