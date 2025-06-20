using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnTriggerStayByTagNode : TriggerNode
    {
        [SerializeField] private string[] _tags;
        public override void CheckTrigger(Collider other)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                bool value = _isInvertTarget ? !other.CompareTag(_tags[i]) : other.CompareTag(_tags[i]);
                if (value)
                {
                    OnTransitionToNextNode(storyGraph);
                }
            }
        }
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
