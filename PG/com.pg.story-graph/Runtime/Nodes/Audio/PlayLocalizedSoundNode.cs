using UnityEngine;
using UnityEngine.Audio;
using PG.LocalizationManagement;
namespace PG.StorySystem.Nodes
{
    public class PlayLocalizedSoundNode : AudioNode
    {
        [HideInInspector] public string audioSourceNameIndex;
        private GameObject _gameObject;
        private AudioSource _audioSource;

        private float _standardVolume;
        [SerializeField] private bool _playOneShot;

        [SerializeField] private AudioLanguage[] _languages;
        public class AudioLanguage
        {
            public string language;
            public AudioClip audioResource;
        }
        protected override void Initialize(StoryGraph storyGraph)
        {
            storyGraph.GetObject(audioSourceNameIndex, out _audioSource);

            foreach (var language in _languages)
            {
                if (language.language == CSVLocalizationManager.Instance.currentLanguage)
                {
                    _audioResource = language.audioResource;
                }
            }

            _standardVolume = _audioSource.volume;
        }
        public override void PlayAudioResource(AudioResource resource)
        {
            if (_playOneShot)
            {
                _audioSource.PlayOneShot(resource as AudioClip);
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
