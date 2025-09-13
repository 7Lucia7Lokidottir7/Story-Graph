using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class SetSpawnpointNode : ActionNode
    {
        private VRPlayerPositioning _player;
        private Transform _spawnPoint;
        [SerializeField] private bool _useRotation;
        [SerializeField] private bool _useScale;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void Init(StoryGraph storyGraph)
        {
            _player = FindAnyObjectByType<VRPlayerPositioning>(FindObjectsInactive.Include);
            storyGraph.GetObject(objectNameID, out _spawnPoint);
        }
        protected override void OnStart(StoryGraph storyGraph)
        {

            if (_useRotation)
            {
                _player.ResetPosition(_spawnPoint.position, _spawnPoint.rotation);
            }
            else
            {
                _player.ResetPosition(_spawnPoint.position, _player.originTransform.rotation);
            }
            if (_useScale)
            {
                _player.transform.localScale = _spawnPoint.lossyScale;
            }

            TransitionToNextNodes(storyGraph);
        }
    }
}
