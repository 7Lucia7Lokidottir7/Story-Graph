using UnityEngine;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    public class ObjectElement : MonoBehaviour
    {
        [HideInInspector] public StoryGraph graph;
        [HideInInspector] public string objectNameID;

        public event System.Action<Collider> onTriggerEntered;
        public event System.Action<Collider> onTriggerExited;
        public event System.Action<Collider> onTriggerStayed;

        public event System.Action<Collision> onCollisionEntered;
        public event System.Action<Collision> onCollisionExited;
        public event System.Action<Collision> onCollisionStayed;

        private void OnTriggerEnter(Collider other)
        {
            onTriggerEntered?.Invoke(other);
        }
        private void OnTriggerExit(Collider other)
        {
            onTriggerExited?.Invoke(other);
        }
        private void OnTriggerStay(Collider other)
        {
            onTriggerStayed?.Invoke(other);
        }
        private void OnCollisionEnter(Collision collision)
        {
            onCollisionEntered?.Invoke(collision);
        }
        private void OnCollisionExit(Collision collision)
        {
            onCollisionExited?.Invoke(collision);
        }
        private void OnCollisionStay(Collision collision)
        {
            onCollisionStayed?.Invoke(collision);
        }
    }
}

