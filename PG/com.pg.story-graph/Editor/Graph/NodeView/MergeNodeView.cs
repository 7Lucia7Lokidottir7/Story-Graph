
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEditor.Experimental.GraphView;
    using UnityEngine;
    using UnityEditor;
    using System.Linq;

    [CustomNodeView(typeof(MergeNode))]
    public class MergeNodeView : StoryNodeView
    {
        public MergeNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            SetupClasses();
        }
        protected override void CreateInputPorts()
        {
            input = PortFactory.CreateVerticalPort(this, "In", Direction.Input, Port.Capacity.Multi, Color.white);
            if (input != null)
            {
                inputContainer.Add(input);
            }
            // откладываем на следующий «кадр» в редакторе
            EditorApplication.delayCall += () =>
            {
                if (storyNode is AndMergeNode m)
                    m.previousNodesCount = input.connections.Count();
            };
        }
        public override void UnConnectToInputNode(Edge edge)
        {
            base.UnConnectToInputNode(edge);
            // откладываем на следующий «кадр» в редакторе
            EditorApplication.delayCall += () =>
            {
                if (storyNode is AndMergeNode m)
                    m.previousNodesCount = input.connections.Count();
            };
        }
        public override void ConnectToInputNode(Edge edge)
        {
            base.ConnectToInputNode(edge);
            // откладываем на следующий «кадр» в редакторе
            EditorApplication.delayCall += () =>
            {
                if (storyNode is AndMergeNode m)
                    m.previousNodesCount = input.connections.Count();
            };
        }
        public override void ConnectNodes(StoryGraphView storyGraphView)
        {
            base.ConnectNodes(storyGraphView);
        }
    }
}