using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.StorySystem
{
    using Nodes;
    using System.Collections;

    public sealed class StoryGraphRunner : MonoBehaviour
    {
        private StoryGraph _storyGraph;
        public StoryGraph currentStoryGraph => _storyGraph;
        public StoryGraph storyGraph => baseStoryGraph;
        [field: SerializeField] public StoryGraph baseStoryGraph { get; private set; }

        internal Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();

        [SerializeField] private bool _isStartGraphOnAwake = true;
        ObjectElement[] objectElements;
        private void Awake()
        {
            _storyGraph = baseStoryGraph;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (_isStartGraphOnAwake && isActiveAndEnabled)
            {
                InitializeGraphRunner();
            }
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitAndInitializeObjects());
        }

        private IEnumerator WaitAndInitializeObjects()
        {
            yield return null; // ∆дем один кадр, чтобы сцена успела инициализироватьс€
            InitializeObjects();
        }

        public void InitializeGraphRunner()
        {
            if (_storyGraph)
            {
                InitializeObjects();
                _storyGraph.runner = this;

                _storyGraph.Initialize();
            }
        }


        private void InitializeObjects()
        {
            objectElements = FindObjectsByType<ObjectElement>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            gameObjects.Clear();
            for (int i = 0; i < objectElements.Length; i++)
            {
                string key = objectElements[i].objectNameID;
                if (!gameObjects.ContainsKey(key))
                {
                    gameObjects.Add(key, objectElements[i].gameObject);
                }
                else
                {
                    Debug.LogWarning($"Duplicate key detected: {key}. Skipping duplicate entry.");
                }
            }
        }
    }
}
