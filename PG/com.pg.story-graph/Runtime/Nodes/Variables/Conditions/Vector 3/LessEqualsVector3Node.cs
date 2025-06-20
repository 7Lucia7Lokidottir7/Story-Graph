using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class LessEqualsVector3Node : Vector3ConditionNode
    {

        public override bool GetResult()
        {
            bool result = false;

            if (isActiveVariable2)
            {
                result = variable1.vector3Value.magnitude <= variable2.vector3Value.magnitude;
            }
            else
            {
                result = variable1.vector3Value.magnitude <= data2.magnitude;
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
