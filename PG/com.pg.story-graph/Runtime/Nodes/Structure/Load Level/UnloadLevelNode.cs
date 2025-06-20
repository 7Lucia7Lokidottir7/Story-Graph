using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.StorySystem.Nodes
{
    public class UnloadLevelNode : LevelLoaderNode
    {
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override IEnumerator LoadingScene()
        {
            if (SceneManager.GetSceneByName(_levelName).isLoaded)
            {
                AsyncOperation operation = SceneManager.UnloadSceneAsync(_levelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                yield return new WaitUntil(() => operation.isDone);

            }
            yield return new WaitForSecondsRealtime(2f);
            OnTransitionToNextNode(storyGraph);
            _coroutine = null;
        }
    }
}
