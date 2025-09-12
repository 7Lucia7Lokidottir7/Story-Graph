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
            // ������� �������� ������� ��� ����������������� ����������
            VisualElement root = new VisualElement();

            // ������� ����������� ��������� ����� SerializedObject
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // ��������� ������ � �������� �������
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
