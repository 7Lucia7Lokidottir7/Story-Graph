using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class EqualsVector3Node : Vector3ConditionNode
    {

        public override bool GetResult()
        {
            bool result = false;

            if (isActiveVariable2)
            {
                result = variable1.vector3Value == variable2.vector3Value;
            }
            else
            {
                result = variable1.vector3Value == data2;
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
