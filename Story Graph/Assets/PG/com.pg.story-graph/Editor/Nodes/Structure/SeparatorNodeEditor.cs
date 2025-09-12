using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using UnityEditor.UIElements;
    using UnityEngine;

    [CustomEditor(typeof(SeparatorNode))]
    public class SeparatorNodeEditor : StoryNodeEditor
    {
        private SeparatorNode _separatorNode;
        protected override bool _useDefaultInspector => false;
        protected override void Init()
        {
            base.Init();
            _separatorNode = target as SeparatorNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            ObjectField objectField = new ObjectField();
            objectField.objectType = typeof(ScriptableObject);
            objectField.value = _separatorNode;
            objectField.allowSceneObjects = false;
            objectField.SetEnabled(false);
            root.Add(objectField);
        }
    }
}
