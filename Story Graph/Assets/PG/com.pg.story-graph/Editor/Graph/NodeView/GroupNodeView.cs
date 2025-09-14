
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEngine.UIElements;
    using UnityEngine;
    using UnityEditor;

    [CustomNodeView(typeof(BaseGroupNode))]
    public class GroupNodeView : StoryNodeView
    {
        private Label _titleLabel = new Label();
        public GroupNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            
            OpenButton(node);
            updateNode += (n) =>
            {
                BaseGroupNode groupNode = (BaseGroupNode)n;
                _titleLabel.text = groupNode.nameGroup;
            };
        }
        public void OpenButton(StoryNode node)
        {
            BaseGroupNode groupNode = node as BaseGroupNode;
            VisualElement buttonContainer = this.Q<VisualElement>("button-container");

            buttonContainer.Add(_titleLabel);

            _titleLabel.style.fontSize = 13;
            _titleLabel.style.unityFontStyleAndWeight = FontStyle.BoldAndItalic;
            _titleLabel.style.unityTextAlign = TextAnchor.MiddleCenter;
            _titleLabel.text = groupNode.nameGroup;

            Button button = new Button(() =>
            {
                StoryGraphEditorWindow.graphView.AddGroupLayer(groupNode);
                StoryGraphEditorWindow.graphView.LoadGraph(node.storyGraph, groupNode);
                //Debug.Log(EditorJsonUtility.ToJson(node, true));
            });
            button.text = "Open";
            buttonContainer.Add(button);
        }
    }
}