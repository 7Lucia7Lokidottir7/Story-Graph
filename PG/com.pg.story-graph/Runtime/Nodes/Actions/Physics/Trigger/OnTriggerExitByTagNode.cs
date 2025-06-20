using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class OnTriggerExitByTagNode : TriggerNode
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
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(objectNameID, out _objectElement);
        }
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
