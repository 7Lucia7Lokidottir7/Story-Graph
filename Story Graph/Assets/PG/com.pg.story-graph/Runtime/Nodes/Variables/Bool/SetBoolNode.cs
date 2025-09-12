using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetBoolNode : BoolModificatorNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isActiveVariable2)
            {
                variable1.boolValue = variable2.boolValue;
            }
            else
            {
                variable1.boolValue = dataBool2;
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
