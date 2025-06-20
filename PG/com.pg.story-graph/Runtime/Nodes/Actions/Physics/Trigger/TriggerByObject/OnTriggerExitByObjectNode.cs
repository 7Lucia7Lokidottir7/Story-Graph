using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnTriggerExitByObjectNode : TriggerByObjectNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onTriggerExited -= CheckTrigger;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onTriggerExited += CheckTrigger;
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
