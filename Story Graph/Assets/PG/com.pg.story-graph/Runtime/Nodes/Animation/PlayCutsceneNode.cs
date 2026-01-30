using System.Collections;
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
        protected override bool useUpdate => true;
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(objectNameID, out _playableDirector);
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            _playableDirector.Play(_asset);
            if (!_isWaitEndTimeline)
            {
                TransitionToNextNodes(storyGraph);
            }
        }
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            float time = Time.time;
            while (true)
            {
                if (_isWaitEndTimeline && _playableDirector.playableAsset == _asset)
                {
                    if (Time.time >= time + _asset.duration)
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
                yield return null;
            }
        }
    }
}
