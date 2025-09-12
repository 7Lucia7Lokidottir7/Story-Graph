
using PG.StorySystem.Nodes;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
namespace PG.StorySystem
{
    [CustomNodeView(typeof(SeparatorNode))]
    public class SeparatorNodeView : StoryNodeView
    {
        private SeparatorNode _separatorNode;
        public SeparatorNodeView(StoryNode node) : base(node)
        {
            SetupClasses();

            storyNode = node;
            guid = node.guid;
            viewDataKey = guid;

            title = "";

            style.width = 65;
            style.minWidth = 64;
            style.maxWidth = 66;
            _separatorNode = storyNode as SeparatorNode;
        }
        protected override void CreateInputPorts()
        {
            input = PortFactory.CreateVerticalPort(this, "In", Direction.Input, Port.Capacity.Multi, Color.white);
            if (input != null)
            {
                inputContainer.Add(input);
            }
        }
        public override void ConnectToInputNode(Edge edge)
        {
            base.ConnectToInputNode(edge);
            if (edge != null)
            {
                if (edge.input.node == this)
                {
                    _separatorNode.previousNodesCount++;
                    for (int i = 0; i < _separatorNode.childrenID.Count; i++)
                    {
                        if (_separatorNode.storyGraph.GetNodeByID(_separatorNode.childrenID[i]) is AndMergeNode andMergeNode)
                        {
                            andMergeNode.previousNodesCount++;
                        }
                    }
                }
            }
        }
        public override void UnConnectToInputNode(Edge edge)
        {
            base.UnConnectToInputNode(edge);
            if (edge != null)
            {
                if (edge.input.node == this)
                {
                    _separatorNode.previousNodesCount--;
                    for (int i = 0; i < _separatorNode.childrenID.Count; i++)
                    {
                        if (_separatorNode.storyGraph.GetNodeByID(_separatorNode.childrenID[i]) is AndMergeNode andMergeNode)
                        {
                            andMergeNode.previousNodesCount--;
                        }
                    }
                }
            }
        }
        public override void ConnectToOutputNode(Edge edge)
        {
            base.ConnectToOutputNode(edge);
            if (edge != null)
            {
                if (edge.input.node is MergeNodeView mergeNodeView)
                {
                    AndMergeNode andMergeNode = mergeNodeView.storyNode as AndMergeNode;
                    andMergeNode.previousNodesCount += _separatorNode.previousNodesCount - 1;
                }
            }
        }
        public override void UnConnectToOutputNode(Edge edge)
        {
            base.UnConnectToOutputNode(edge);
            if (edge != null)
            {
                if (edge.input.node is MergeNodeView mergeNodeView)
                {
                    AndMergeNode andMergeNode = mergeNodeView.storyNode as AndMergeNode;
                    andMergeNode.previousNodesCount -= _separatorNode.previousNodesCount - 1;
                }
            }
        }
    }
}