using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace PG.StorySystem
{

    [CustomEditor(typeof(StoryGraph))]
    public class StoryGraphEditor : Editor
    {
        private StoryGraph _storyGraph;
        private void OnEnable()
        {
            _storyGraph = (StoryGraph)target;
        }
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();
            InspectorElement.FillDefaultInspector(root, new SerializedObject(_storyGraph), this);

            Button button = new Button(() => StoryGraphEditorWindow.OpenGraph(_storyGraph));
            button.text = "Open Graph";

            root.Add(button);

            return root;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open Graph"))
            {
                StoryGraphEditorWindow.OpenGraph(_storyGraph);
            }
        }
        [OnOpenAsset]
        public static bool OpenAsset(int instanceID)
        {
            Object obj = EditorUtility.EntityIdToObject(instanceID);
            if (obj is StoryGraph)
            {
                StoryGraph storyGraph = (StoryGraph)obj;
                StoryGraphEditorWindow.OpenGraph(storyGraph);
                return true;
            }
            return false;
        }
    }
}
