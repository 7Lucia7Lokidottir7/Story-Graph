using UnityEngine;
using UnityEngine.Playables;

namespace PG.StorySystem.Nodes
{
    public class PlayCutsceneNode : ActionNode
    {
        public override Color colorNode => Color.rebeccaPurple;
        private PlayableDirector _playableDirector;
        [SerializeField] private PlayableAsset _asset;
        [SerializeField] private bool _isWaitEndTimeline;
        private float _time;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(objectNameID, out _playableDirector);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _playableDirector.Play(_asset);
            _time = Time.time;
            if (!_isWaitEndTimeline)
            {
                TransitionToNextNodes(storyGraph);
            }
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_isWaitEndTimeline && _playableDirector.playableAsset == _asset)
            {
                if (Time.time >= _time + _asset.duration)
                {
                    TransitionToNextNodes(storyGraph);
                }
                else
                {
                    if (_playableDirector.state != PlayState.Playing)
                    {
                        TransitionToNextNodes(storyGraph);
                    }
                }
            }
        }
    }
}
