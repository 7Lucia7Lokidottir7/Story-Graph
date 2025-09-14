
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEngine.UIElements;

    [CustomNodeView(typeof(ReturnNode))]
    public class ReturnNodeView : StoryNodeView
    {
        public ReturnNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            

            VisualElement buttonContainer = this.Q<VisualElement>("button-container");
            Button setMainButton = new Button();
            setMainButton.text = "Set Main";
            setMainButton.clicked += () =>
            {
                if (StoryGraphEditorWindow.graphView.currentGroupNode != null)
                {
                    StoryGraphEditorWindow.graphView.currentGroupNode.returnNode = storyNode as ReturnNode;
                }
            };
            buttonContainer.Add(setMainButton);
        }
        protected override void CreateOutputPorts()
        {
        }
    }
}