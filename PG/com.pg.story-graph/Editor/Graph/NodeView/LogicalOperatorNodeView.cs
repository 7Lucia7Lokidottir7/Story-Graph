
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;
    using UnityEngine;

    [CustomNodeView(typeof(LogicalOperatorNode))]
    public class LogicalOperatorNodeView : ConditionNodeView
    {
        public Port variableInput;
        public Port variableInput2;
        public LogicalOperatorNodeView(StoryNode node) : base(node)
        {
        }
        protected override void CreateVariableInputNode()
        {
            VisualElement variableContainer = this.Q<VisualElement>(_variableInputContainerName);

            variableInput = PortFactory.CreateHorizontalPort(this, "In Condition", Direction.Input, Port.Capacity.Single, Color.yellow);
            variableContainer.Add(variableInput);
            variableInput2 = PortFactory.CreateHorizontalPort(this, "In Condition 2", Direction.Input, Port.Capacity.Single, Color.yellow);
            variableContainer.Add(variableInput2);
        }
        protected override void CreateVariableOuputNode()
        {
            VisualElement variableContainer = this.Q<VisualElement>(_variableOutputContainerName);

            variableOutput = PortFactory.CreateHorizontalPort(this, "Out Condition", Direction.Output, Port.Capacity.Single, Color.yellow);
            variableContainer.Add(variableOutput);
        }
        public override void ConnectNodes(StoryGraphView storyGraphView)
        {
            LogicalOperatorNode logicalOperatorNode = storyNode as LogicalOperatorNode;
            if (logicalOperatorNode.conditionID > -1)
            {
                ConditionNode child = default;
                child = storyNode.storyGraph.GetNodeByID(logicalOperatorNode.conditionID, groupNode) as ConditionNode;
                ConditionNodeView conditionNodeView = storyGraphView.FindNodeView(child) as ConditionNodeView;

                Edge edge = variableInput.ConnectTo(conditionNodeView.variableOutput);
                storyGraphView.AddElement(edge);
            }
            if (logicalOperatorNode.condition2ID > -1)
            {
                ConditionNode child = default;
                child = storyNode.storyGraph.GetNodeByID(logicalOperatorNode.condition2ID, groupNode) as ConditionNode;
                ConditionNodeView conditionNodeView = storyGraphView.FindNodeView(child) as ConditionNodeView;

                Edge edge = variableInput2.ConnectTo(conditionNodeView.variableOutput);
                storyGraphView.AddElement(edge);
            }
        }
        public override void ConnectToInputNode(Edge edge)
        {
            ConditionNodeView conditionNodeView = edge.output.node as ConditionNodeView;
            ConditionNode conditionNode = conditionNodeView.storyNode as ConditionNode;
            LogicalOperatorNode logicalOperatorNode = storyNode as LogicalOperatorNode;
            if (edge.input == variableInput)
            {
                logicalOperatorNode.conditionID = conditionNode.id;
            }
            else if (edge.input == variableInput2)
            {
                logicalOperatorNode.condition2ID = conditionNode.id;
            }
        }

        public override void UnConnectToInputNode(Edge edge)
        {
            ConditionNodeView conditionNodeView = edge.output.node as ConditionNodeView;
            ConditionNode conditionNode = conditionNodeView.storyNode as ConditionNode;
            LogicalOperatorNode logicalOperatorNode = storyNode as LogicalOperatorNode;
            if (edge.input == variableInput)
            {
                logicalOperatorNode.conditionID = -1;
            }
            else if (edge.output == variableInput2)
            {
                logicalOperatorNode.condition2ID = -1;
            }
        }
    }
}