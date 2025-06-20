using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AddFloatNode : FloatModificatorNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isActiveVariable2)
            {
                variable1.floatValue += variable2.floatValue;
            }
            else if (!isActiveVariable2)
            {
                variable1.floatValue += dataFloat2;
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
