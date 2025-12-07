
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;
    using UnityEngine;
    using System.Collections.Generic;
    [CustomNodeView(typeof(BranchNode))]
    public class BranchNodeView : StoryNodeView
    {
        public Port variableInput;
        private const string _variableInputContainerName = "variable-input";
        private Port falseOutput;
        public BranchNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            
            CreateVariableInputNode();
        }
        protected override void CreateVariableInputNode()
        {
            VisualElement variableContainer = this.Q<VisualElement>(_variableInputContainerName);

            variableInput = PortFactory.CreateHorizontalPort(this, "In Condition", Direction.Input, Port.Capacity.Single, Color.yellow);
            variableContainer.Add(variableInput);
        }
        protected override void CreateOutputPorts()
        {
            output = PortFactory.CreateVerticalPort(this, "Out True", Direction.Output, Port.Capacity.Multi, Color.white);
            if (output != null)
            {
                outputContainer.Add(output);
            }
            falseOutput = PortFactory.CreateVerticalPort(this, "Out False", Direction.Output, Port.Capacity.Multi, Color.white);
            if (falseOutput != null)
            {
                outputContainer.Add(falseOutput);
            }
        }

        public override List<int> GetChildrenList(int childID)
        {
            BranchNode branchNode = storyNode as BranchNode;
            if (branchNode.falseChildrenID.Contains(childID))
            {
                return branchNode.falseChildrenID;
            }
            if (branchNode.childrenID.Contains(childID))
            {
                return branchNode.childrenID;
            }
            return null;
        }
        public override void ConnectNodes(StoryGraphView storyGraphView)
        {
            base.ConnectNodes(storyGraphView);

            BranchNode branchNode = storyNode as BranchNode;
            foreach (var item in branchNode.falseChildrenID)
            {
                StoryNodeView child = storyGraphView.FindNodeView(storyNode.storyGraph.GetNodeByID(item, groupNode));
                if (child == null)
                {
                    continue;
                }
                Edge edge = falseOutput.ConnectTo(child.input);
                storyGraphView.AddElement(edge);
            }
            if (branchNode.conditionID > -1)
            {
                ConditionNodeView child = storyGraphView.FindNodeView(storyNode.storyGraph.GetNodeByID(branchNode.conditionID, groupNode)) as ConditionNodeView;
                if (child == null)
                {
                    return;
                }
                Edge edge = variableInput.ConnectTo(child.variableOutput);
                storyGraphView.AddElement(edge);
            }
        }
        public override void ConnectToInputNode(Edge edge)
        {
            BranchNode branchNode = storyNode as BranchNode;

            if (edge.input == variableInput)
            {
                StoryNodeView child = edge.output.node as StoryNodeView;
                if (branchNode == null)
                {
                    Debug.LogError("BranchNode is null. Check the connection.");
                    return;
                }

                ConditionNode conditionNode = child.storyNode as ConditionNode;
                if (conditionNode == null)
                {
                    Debug.LogError("ConditionNode is null. Check the connection.");
                    return;
                }
                branchNode.conditionID = conditionNode.id;
            }
            else
            {
                base.ConnectToInputNode(edge);
            }
        }
        public override void DisconnectFromInputPort(Edge edge)
        {

            BranchNode branchNode = storyNode as BranchNode;
            if (edge.input == variableInput)
            {
                if (branchNode == null)
                {
                    Debug.LogError("BranchNode is null. Check the connection.");
                    return;
                }
                branchNode.conditionID = -1;
            }
            else
            {
                base.DisconnectFromInputPort(edge);
            }
        }
        public override void ConnectFromOutputPort(Edge edge)
        {
            BranchNode branchNode = storyNode as BranchNode;
            StoryNodeView child = edge.input.node as StoryNodeView;
            if (edge.output.portName == "Out False")
            {
                branchNode.falseChildrenID.Add(child.storyNode.id);
            }
            else if (edge.output.portName == "Out True")
            {
                base.ConnectFromOutputPort(edge);
            }
        }
        public override void DisconnectFromOutputPort(Edge edge)
        {
            BranchNode branchNode = storyNode as BranchNode;
            if (edge.output.portName == "Out False")
            {
                StoryNodeView child = edge.input.node as StoryNodeView;
                branchNode.falseChildrenID.Remove(child.storyNode.id);
            }
            else if (edge.output.portName == "Out True")
            {
                base.DisconnectFromOutputPort(edge);
            }
        }
    }
}