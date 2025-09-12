using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SubtractIntNode : IntModificatorNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isActiveVariable2)
            {
                variable1.intValue -= variable2.intValue;
            }
            else
            {
                variable1.intValue -= dataInt2;
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
