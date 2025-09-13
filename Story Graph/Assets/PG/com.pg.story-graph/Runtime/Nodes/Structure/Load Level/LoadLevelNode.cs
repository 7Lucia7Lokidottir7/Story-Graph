
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.StorySystem.Nodes
{
    public class LoadLevelNode : LevelLoaderNode
    {
        [SerializeField] private bool _isAdditiveLoad = true;
        [SerializeField] private bool _setActiveScene = true;
        protected override void OnEnd(StoryGraph storyGraph)
        {
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        protected override IEnumerator LoadingScene()
        {
            if (!SceneManager.GetSceneByName(_levelName).isLoaded)
            {
                AsyncOperation operation = SceneManager.LoadSceneAsync(_levelName, _isAdditiveLoad ? LoadSceneMode.Additive : LoadSceneMode.Single);
                yield return new WaitUntil(() => operation.isDone);

                if (_setActiveScene)
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(_levelName));
                }
            }
            // Дополнительный кадр для завершения инициализации объектов
            yield return null;
            // Переход к следующей ноде только один раз
            TransitionToNextNodes(storyGraph);
            _coroutine = null;
        }

    }
}