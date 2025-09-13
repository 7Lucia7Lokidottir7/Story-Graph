using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.StorySystem.Nodes
{
    public class LevelGroupNode : BaseGroupNode
    {
        public string levelName;
        public bool setActiveScene = true;
        private Scene _scene;
        protected override void OnStart(StoryGraph storyGraph)
        {
            base.OnStart(storyGraph);
            if (!_scene.isLoaded)
            {
                SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive).completed += operation =>
                {
                    _scene = SceneManager.GetSceneByName(levelName);
                    if (setActiveScene)
                    {
                        SceneManager.SetActiveScene(_scene);
                    }
                };
            }
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            base.OnEnd(storyGraph);
            SceneManager.UnloadSceneAsync(_scene);

        }
    }
}
