using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
namespace PG.StorySystem.Nodes
{
    public abstract class AudioNode : StoryNode
    {
        [SerializeField] protected bool _isEndWithClip;
        [SerializeField][Range(0,1)] protected float _volume = 1f;

        protected AudioResource _audioResource;

        public override string classGUI => "audio";
        protected override void OnStart(StoryGraph storyGraph)
        {
            Initialize(storyGraph);
            Play(storyGraph);
        }
        virtual protected void Initialize(StoryGraph storyGraph)
        {
        }
        IEnumerator OnPlaying(AudioResource resource, StoryGraph storyGraph)
        {
            if (resource is AudioClip clip)
            {
                yield return new WaitForSecondsRealtime(clip.length);
            }
            else
            {
                yield return new WaitForSecondsRealtime(10f);
            }
            OnTransitionToNextNode(storyGraph);
        }
        public void Play(StoryGraph storyGraph)
        {
            PlayAudioResource(_audioResource);

            if (_isEndWithClip)
            {
                storyGraph.runner.StartCoroutine(OnPlaying(_audioResource, storyGraph));
            }
            else
            {
                OnTransitionToNextNode(storyGraph);
            }
        }
        public abstract void PlayAudioResource(AudioResource resource);
    }
}
