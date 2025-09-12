using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class CollisionByObjectNode : CollisionNode
    {
        [HideInInspector] public string targetObjectNameID;
        protected Transform _targetTransform;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(targetObjectNameID, out _targetTransform);
        }
        public override void CheckCollision(Collision collision)
        {
            bool value = _isInvertTarget ? collision.transform != _targetTransform : collision.transform == _targetTransform;
            if (value)
            {
                TransitionToNextNodes(storyGraph);
            }
        }
    }
}
