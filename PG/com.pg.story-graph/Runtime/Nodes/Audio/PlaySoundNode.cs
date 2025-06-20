using UnityEngine;
using UnityEngine.Audio;
namespace PG.StorySystem.Nodes
{
    public class PlaySoundNode : AudioNode
    {
        public AudioResource audioClip;
        [HideInInspector]public string audioSourceNameIndex;
        private GameObject _gameObject;
        private AudioSource _audioSource;
        private float _standardVolume;
        [SerializeField] private bool _playOneShot;
        protected override void Initialize(StoryGraph storyGraph)
        {
            base.Initialize(storyGraph);
            storyGraph.GetObject(audioSourceNameIndex, out _audioSource);

            _audioResource = audioClip;
            _standardVolume = _audioSource.volume;
        }
        public override void PlayAudioResource(AudioResource resource)
        {
            if (_playOneShot)
            {
                _audioSource.PlayOneShot(resource as AudioClip, _volume);
            }
            else
            {
                _audioSource.resource = resource;
                _audioSource.volume = _volume;
                _audioSource.Play();
            }
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _audioSource.volume = _standardVolume;
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
    }
}
