
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEditor.Experimental.GraphView;
    using UnityEngine.UIElements;
    using UnityEngine;
    using System.Linq;

    [CustomNodeView(typeof(ConditionNode))]
    public class ConditionNodeView : PropertyNodeView
    {
        public Port variableOutput;
        protected const string _variableOutputContainerName = "variable-output";
        protected const string _variableInputContainerName = "variable-input";
        public ConditionNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            
            CreateVariableInputNode();
            CreateVariableOuputNode();
            PropertyNode propertyNode = node as PropertyNode;
            updateNode += (node) =>
            {
                UpdateTitle(propertyNode, StoryGraphEditorWindow.storyGraph);
            };
            UpdateTitle(propertyNode, StoryGraphEditorWindow.storyGraph);
        }
        void UpdateTitle(PropertyNode propertyNode, StoryGraph graph)
        {
            title = propertyNode.GetName();

            // простая проверка и поиск по string
            if (graph != null && !string.IsNullOrEmpty(propertyNode.storyVariableNameID))
            {
                StoryVariable found = graph.variables
                                 .FirstOrDefault(v => v.variableName == propertyNode.storyVariableNameID) as StoryVariable;
                if (found != null)
                    title += $" ({found.variableName})";

                //Debug.Log(found);
            }
            //Debug.Log(propertyNode.scenarioVariableNameID);
        }
        protected override void CreateInputPorts()
        {
        }
        protected override void CreateVariableInputNode()
        {
        }
        protected override void CreateOutputPorts()
        {
        }
        protected virtual void CreateVariableOuputNode()
        {
            VisualElement variableContainer = this.Q<VisualElement>(_variableOutputContainerName);

            variableOutput = PortFactory.CreateHorizontalPort(this, "Out Condition", Direction.Output, Port.Capacity.Single, Color.yellow);
            variableContainer.Add(variableOutput);
        }
        public override void ConnectToInputNode(Edge edge)
        {
        }
        public override void UnConnectToInputNode(Edge edge)
        {
        }
        public override void ConnectToOutputNode(Edge edge)
        {
        }
        public override void UnConnectToOutputNode(Edge edge)
        {
        }
    }
}