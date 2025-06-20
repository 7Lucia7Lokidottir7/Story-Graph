using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public abstract class TriggerByObjectNode : TriggerNode
    {
        [HideInInspector] public string targetObjectNameID;
        protected Transform _targetTransform;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(targetObjectNameID, out _targetTransform);
        }
        public override void CheckTrigger(Collider other)
        {
            bool value = _isInvertTarget ? other.transform != _targetTransform : other.transform == _targetTransform;
            if (value)
            {
                OnTransitionToNextNode(storyGraph);
            }
        }
    }
}
