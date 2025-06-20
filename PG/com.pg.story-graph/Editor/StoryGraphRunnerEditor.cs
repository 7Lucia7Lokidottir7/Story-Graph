using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem
{
    [CustomEditor(typeof(StoryGraphRunner))]
    public class StoryGraphRunnerEditor : Editor
    {
        private StoryGraphRunner _storyGraphRunner;
        private void OnEnable()
        {
            _storyGraphRunner = (StoryGraphRunner)target;
        }
        public override VisualElement CreateInspectorGUI()
        {
            // Создаем корневой элемент для пользовательского интерфейса
            VisualElement root = new VisualElement();

            // Создаем стандартный инспектор через SerializedObject
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // Добавляем кнопку в корневой элемент
            Button button = new Button(() => {
                if (Application.isPlaying)
                {
                    StoryGraphEditorWindow.OpenGraph(_storyGraphRunner.currentStoryGraph);
                }
                else
                {
                    StoryGraphEditorWindow.OpenGraph(_storyGraphRunner.baseStoryGraph);
                }
            });
            button.text = "Open Graph";

            root.Add(button);

            return root;
        }
    }
}
