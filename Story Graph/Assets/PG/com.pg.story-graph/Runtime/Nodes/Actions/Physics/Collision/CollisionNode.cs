using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class CollisionNode : PhysicsNode
    {
        protected ObjectElement _objectElement;
        [SerializeField] protected bool _isInvertTarget;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(objectNameID, out _objectElement);
        }
        public virtual void CheckCollision(Collision collision)
        {

        }
    }
}
