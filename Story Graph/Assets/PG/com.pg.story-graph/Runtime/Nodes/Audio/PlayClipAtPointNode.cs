using UnityEngine;
using UnityEngine.Audio;
namespace PG.StorySystem.Nodes
{
    public class PlayClipAtPointNode : AudioNode
    {
        public AudioResource resource;

        public enum PositionType
        {
            Object, Vector3
        }
        [HideInInspector] public PositionType positionType;

        [HideInInspector] public Vector3 position;
        [HideInInspector] public int transformIndex;
        [HideInInspector] public string transformNameIndex;
        private GameObject _transformObject;
        protected override void Initialize(StoryGraph storyGraph)
        {
            storyGraph.runner.gameObjects.TryGetValue(storyGraph.objects[transformIndex], out _transformObject);
            _audioResource = resource;
        }
        public override void PlayAudioResource(AudioResource resource)
        {
            switch (positionType)
            {
                case PositionType.Object:
                    AudioSource.PlayClipAtPoint((AudioClip)resource, _transformObject.transform.position, _volume);
                    break;
                case PositionType.Vector3:
                    AudioSource.PlayClipAtPoint((AudioClip)resource, position, _volume);
                    break;
            }
        }
    }
}
