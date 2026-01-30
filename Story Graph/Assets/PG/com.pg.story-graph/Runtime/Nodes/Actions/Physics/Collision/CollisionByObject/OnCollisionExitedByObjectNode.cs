using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnCollisionExitedByObjectNode : CollisionByObjectNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onCollisionExited -= CheckCollision;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onCollisionExited += CheckCollision;
        }
    }
}
