using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class TriggerNode : PhysicsNode
    {
        protected ObjectElement _objectElement;
        [SerializeField] protected bool _isInvertTarget;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(objectNameID, out _objectElement);
        }
        public virtual void CheckTrigger(Collider other)
        {

        }
    }
}
