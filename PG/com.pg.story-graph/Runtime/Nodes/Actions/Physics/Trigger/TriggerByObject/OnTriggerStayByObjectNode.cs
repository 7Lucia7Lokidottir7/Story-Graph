using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnTriggerStayByObjectNode : TriggerByObjectNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onTriggerStayed -= CheckTrigger;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onTriggerStayed += CheckTrigger;
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
