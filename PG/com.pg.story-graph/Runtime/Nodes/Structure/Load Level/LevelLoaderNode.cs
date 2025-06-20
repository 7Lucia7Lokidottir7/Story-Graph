using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace PG.StorySystem.Nodes
{
    public abstract class LevelLoaderNode : StructureNode
    {
        [SerializeField] 
        protected string _levelName;
        public string levelName => _levelName;
        public override string classGUI => "load-level";
        protected Coroutine _coroutine;
        public void SetLevelName(string levelName) => _levelName = levelName;
        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_coroutine == null)
            {
                _coroutine = storyGraph.runner.StartCoroutine(LoadingScene());
            }
        }
        protected virtual IEnumerator LoadingScene()
        {
            yield return new WaitForSecondsRealtime(2f);

            OnTransitionToNextNode(storyGraph);
            _coroutine = null;
        }

    }
}
