using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnCollisionStayedByObjectNode : CollisionByObjectNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onCollisionStayed -= CheckCollision;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onCollisionStayed += CheckCollision;
        }
    }
}
