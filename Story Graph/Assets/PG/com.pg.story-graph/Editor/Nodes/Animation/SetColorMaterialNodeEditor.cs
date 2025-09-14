using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    [CustomEditor(typeof(SetColorMaterialNode), true)]
    public class SetColorMaterialNodeEditor : StoryNodeEditor
    {
        private SetColorMaterialNode _setColorMaterialNode;
        protected override void Init()
        {
            base.Init();
            _setColorMaterialNode = target as SetColorMaterialNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);


            Toggle useLerp = new Toggle("Use Lerp");

            FloatField duration = new FloatField("Duration");
            duration.value = _setColorMaterialNode.duration;
            duration.RegisterValueChangedCallback(c => {
                _setColorMaterialNode.duration = c.newValue;
                EditorUtility.SetDirty(_setColorMaterialNode);
            });

            useLerp.value = _setColorMaterialNode.useLerp;
            useLerp.RegisterValueChangedCallback((c) =>
            {
                _setColorMaterialNode.useLerp = c.newValue;
                duration.style.display = c.newValue ? DisplayStyle.Flex : DisplayStyle.None;
                EditorUtility.SetDirty(_setColorMaterialNode);

            });


            duration.value = _setColorMaterialNode.duration;
            duration.style.display = _setColorMaterialNode.useLerp ? DisplayStyle.Flex : DisplayStyle.None;


            root.Add(useLerp);
            root.Add(duration);
        }
    }
}