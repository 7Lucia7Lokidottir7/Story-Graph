using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class DestroyObjectNode : ActionNode
    {
        private Transform _transform;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_transform != null)
            {
                Destroy(_transform.gameObject);
                storyGraph.runner.gameObjects.Remove(objectNameID);
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
