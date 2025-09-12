using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class SetAsChildObjectNode : TransformNode
    {
        private Transform _transform;
        private Transform _transformParent;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            storyGraph.GetObject(targetObjectNameID, out _transformParent);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_transform != null && _transformParent != null)
            {
                _transform.parent = _transformParent;
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
