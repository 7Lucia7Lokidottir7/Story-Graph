
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.StorySystem.Nodes
{
    public class LoadLevelNode : LevelLoaderNode
    {
        [SerializeField] private bool _isAdditiveLoad = true;
        [SerializeField] private bool _setActiveScene = true;

        protected override void OnStart(StoryGraph storyGraph)
        {
            // Если мы зашли в OnStart, значит граф хочет выполнить эту ноду.
            // Даже если там висит старая ссылка на корутину, она скорее всего от мертвого раннера.
            // Принудительно сбрасываем или останавливаем старую (если раннер тот же) и запускаем новую.

            if (SceneManager.GetSceneByName(_levelName).isLoaded)
            {
                TransitionToNextNodes(storyGraph);
                return;
            }

            if (_coroutine != null)
            {
                // На случай, если вызов дублируется в том же раннере
                storyGraph.runner.StopCoroutine(_coroutine);
                _coroutine = null;
            }

            _coroutine = storyGraph.runner.StartCoroutine(LoadingScene());
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