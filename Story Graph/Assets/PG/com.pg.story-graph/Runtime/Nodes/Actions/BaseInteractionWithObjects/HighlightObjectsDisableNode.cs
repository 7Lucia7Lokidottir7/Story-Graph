using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class HighlightObjectsDisableNode : BaseInteractionWithObjectsNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            Debug.Log(_objects);
            Debug.Log(_objects.Count);
            for (int i = 0; i < _objects.Count; i++)
            {
                if (_objects[i] != null && _objects[i].TryGetComponent(out HighlightObjectController highlightObjectController))
                {
                    Debug.Log(_objects[i].name, _objects[i]);
                    highlightObjectController.SetHighlightInactive();
                    highlightObjectController.isEnable = true;
                }
            }
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}