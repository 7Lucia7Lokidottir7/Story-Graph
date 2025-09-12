namespace PG.StorySystem.Nodes
{
    public class GreaterEqualsIntNode : IntConditionNode
    {
        public override bool GetResult()
        {
            bool result = false;

            if (isActiveVariable2)
            {
                result = variable1.intValue >= variable2.intValue;
            }
            else
            {
                result = variable1.intValue >= data2;
            }
            return result;
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
