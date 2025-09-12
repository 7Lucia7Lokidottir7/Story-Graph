
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEngine.UIElements;

    [CustomNodeView(typeof(RootNode))]
    public class RootNodeView : StoryNodeView
    {
        public RootNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            SetupClasses();

            VisualElement buttonContainer = this.Q<VisualElement>("button-container");
            Button setMainButton = new Button();
            setMainButton.text = "Set Main";
            setMainButton.clicked += () =>
            {
                if (StoryGraphEditorWindow.graphView.currentGroupNode != null)
                {
                    StoryGraphEditorWindow.graphView.currentGroupNode.rootNode = storyNode as RootNode;
                }
                else
                {
                    storyNode.storyGraph.rootNode = node as RootNode;
                }
            };
            buttonContainer.Add(setMainButton);
        }
        protected override void CreateInputPorts()
        {
        }
    }
}