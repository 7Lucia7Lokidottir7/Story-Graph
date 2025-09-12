using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class DeactivateObjectsNode : BaseInteractionWithObjectsNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            Debug.Log(_objects);
            Debug.Log(_objects.Count);
            for(int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] != null)
                {
                    Debug.Log(_objects[i].name, _objects[i]);
                    _objects[i].gameObject.SetActive(false);
                }
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}