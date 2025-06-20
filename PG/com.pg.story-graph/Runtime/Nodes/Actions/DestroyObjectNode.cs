using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class DestroyObjectNode : ActionNode
    {
        private Transform _transform;

        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _transform);
            if (_transform != null)
            {
                Destroy(_transform.gameObject);
                storyGraph.runner.gameObjects.Remove(objectNameID);
            }
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
