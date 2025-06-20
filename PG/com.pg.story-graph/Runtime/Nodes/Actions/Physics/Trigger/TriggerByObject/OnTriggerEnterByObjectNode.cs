using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnTriggerEnterByObjectNode : TriggerByObjectNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onTriggerEntered -= CheckTrigger;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onTriggerEntered += CheckTrigger;
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
