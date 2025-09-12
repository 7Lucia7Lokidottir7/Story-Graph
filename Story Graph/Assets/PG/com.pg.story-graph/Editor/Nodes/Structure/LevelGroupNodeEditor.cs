using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;

    [CustomEditor(typeof(LevelGroupNode), true)]
    internal class LevelGroupNodeEditor : StoryNodeEditor
    {
        private LevelGroupNode _levelGroupNode;
        private SceneAsset _sceneAsset;

        public VisualTreeAsset visualTreeAsset;
        protected override void Init()
        {
            base.Init();
            _levelGroupNode = target as LevelGroupNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            ObjectField sceneField = new ObjectField("Scene Asset");
            sceneField.objectType = typeof(SceneAsset);
            sceneField.RegisterValueChangedCallback(evt => {
                if (evt.newValue != null)
                {
                    _levelGroupNode.levelName = evt.newValue.name;
                }
                EditorUtility.SetDirty(_levelGroupNode);
                sceneField.value = null;
            });
            root.Add(sceneField);
        }
    }
}