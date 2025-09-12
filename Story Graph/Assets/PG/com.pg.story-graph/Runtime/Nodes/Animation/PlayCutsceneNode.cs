using UnityEngine;
using UnityEngine.Playables;

namespace PG.StorySystem.Nodes
{
    public class PlayCutsceneNode : ActionNode
    {
        private PlayableDirector _playableDirector;
        [SerializeField] private PlayableAsset _asset;
        [SerializeField] private bool _isWaitEndTimeline;
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
            if (!_isWaitEndTimeline)
            {
                TransitionToNextNodes(storyGraph);
            }
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
            if (_isWaitEndTimeline)
            {
                if (_playableDirector.duration >= _asset.duration)
                {
                    TransitionToNextNodes(storyGraph);
                }
            }
        }
    }
}
