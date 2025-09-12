using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(LevelLoaderNode), true)]
    internal class LevelLoaderNodeEditor : StoryNodeEditor
    {
        private LevelLoaderNode _levelLoaderNode;

        public VisualTreeAsset visualTreeAsset;
        protected override void Init()
        {
            base.Init();
            _levelLoaderNode = target as LevelLoaderNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            ObjectField sceneField = new ObjectField("Scene Asset");
            sceneField.objectType = typeof(SceneAsset);
            sceneField.RegisterValueChangedCallback(evt => {
                if (evt.newValue != null)
                {
                    _levelLoaderNode.SetLevelName(evt.newValue.name);
                }
                sceneField.value = null;
                EditorUtility.SetDirty(_levelLoaderNode);
            });
            root.Add(sceneField);
        }
    }
}