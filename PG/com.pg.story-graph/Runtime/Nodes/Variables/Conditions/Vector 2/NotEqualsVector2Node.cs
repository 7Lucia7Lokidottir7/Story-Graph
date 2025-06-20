using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class NotEqualsVector2Node : Vector2ConditionNode
    {
        public override bool GetResult()
        {
            bool result = false;


            if (isActiveVariable2)
            {
                result = variable1.vector2Value != variable2.vector2Value;
            }
            else
            {
                result = variable1.vector2Value != data2;
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
