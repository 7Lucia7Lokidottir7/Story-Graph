using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PG.StorySystem.Nodes
{
    public class MultiplyIntNode : IntModificatorNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isActiveVariable2)
            {
                variable1.intValue *= variable2.intValue;
            }
            else
            {
                variable1.intValue *= dataInt2;
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
