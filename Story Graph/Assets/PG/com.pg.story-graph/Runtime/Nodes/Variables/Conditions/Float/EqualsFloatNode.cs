using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class EqualsFloatNode : FloatConditionNode
    {
        public override bool GetResult()
        {
            bool result = false;

            if (isActiveVariable2)
            {
                result = Mathf.Approximately(variable1.floatValue, variable2.floatValue);
            }
            else
            {
                result = Mathf.Approximately(variable1.floatValue, data2);
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
