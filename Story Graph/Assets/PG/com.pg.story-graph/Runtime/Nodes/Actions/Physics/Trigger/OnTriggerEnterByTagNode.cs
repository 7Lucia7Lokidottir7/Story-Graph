using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnTriggerEnterByTagNode : TriggerNode
    {
        [SerializeField] private string[] _tags;
        public override void CheckTrigger(Collider other)
        {
            for (int i = 0; i < _tags.Length; i++)
            {
                bool value = _isInvertTarget ? !other.CompareTag(_tags[i]) : other.CompareTag(_tags[i]);
                if (value)
                {
                    TransitionToNextNodes(storyGraph);
                }
            }
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _objectElement.onTriggerEntered -= CheckTrigger;
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _objectElement.onTriggerEntered += CheckTrigger;
        }
    }
}
