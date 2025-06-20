using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class MultiplyVector3Node : Vector3ModificatorNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (isActiveVariable2)
            {
                variable1.vector3Value = new Vector3(
                    variable1.vector3Value.x * variable2.vector3Value.x,
                    variable1.vector3Value.y * variable2.vector3Value.y,
                    variable1.vector3Value.z * variable2.vector3Value.z
                    );
            }
            else if (!isActiveVariable2)
            {
                variable1.vector3Value = new Vector3(
                    variable1.vector3Value.x * dataVector2.x,
                    variable1.vector3Value.y * dataVector2.y,
                    variable1.vector3Value.z * dataVector2.z
                    );
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
