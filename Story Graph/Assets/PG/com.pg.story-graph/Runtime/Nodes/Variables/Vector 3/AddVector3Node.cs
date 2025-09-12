using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class AddVector3Node : Vector3ModificatorNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isActiveVariable2)
            {
                variable1.vector3Value += variable2.vector3Value;
            }
            else
            {
                variable1.vector3Value += dataVector2;
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
