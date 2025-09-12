using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnCollisionEnterByObjectNode : CollisionByObjectNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onCollisionEntered -= CheckCollision;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onCollisionEntered += CheckCollision;
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
