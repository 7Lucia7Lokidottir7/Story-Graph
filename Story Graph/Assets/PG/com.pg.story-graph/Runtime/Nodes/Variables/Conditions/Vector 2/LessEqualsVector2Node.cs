using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class LessEqualsVector2Node : Vector2ConditionNode
    {

        public override bool GetResult()
        {
            bool result = false;


            if (isActiveVariable2)
            {
                result = variable1.vector2Value.magnitude <= variable2.vector2Value.magnitude;
            }
            else
            {
                result = variable1.vector2Value.magnitude <= data2.magnitude;
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
