
using PG.StorySystem.Nodes;
namespace PG.StorySystem
{
    using UnityEditor.Experimental.GraphView;
    using UnityEngine;
    using System.Collections.Generic;
    using UnityEngine.UIElements;

    [CustomNodeView(typeof(AnswerSelectorNode))]
    public class AnswerSelectorNodeView : StoryNodeView
    {
        private List<Port> _outputs = new List<Port>();
        public AnswerSelectorNodeView(StoryNode node) : base(node)
        {
            MainStyle(node);
            SetupClasses();
            AnswerSelectorNode answerSelectorNode = node as AnswerSelectorNode;
            VisualElement buttonContainer = this.Q<VisualElement>("button-container");
            Button addButton = new Button();
            addButton.text = "+";
            addButton.clicked += () =>
            {
                answerSelectorNode.answers.Add(new AnswerSelectorNode.Answer());
                Port port = PortFactory.CreateVerticalPort(this, $"Out Answer {answerSelectorNode.answers.Count + 1}", Direction.Output, Port.Capacity.Multi, Color.white);
                _outputs.Add(port);
                outputContainer.Add(port);
                style.maxWidth = new StyleLength(style.maxWidth.value.value + 250);
            };
            buttonContainer.Add(addButton);
            Button removeButton = new Button();
            removeButton.text = "-";
            removeButton.clicked += () =>
            {
                if (_outputs.Count > 0)
                {
                    answerSelectorNode.answers.RemoveAt(answerSelectorNode.answers.Count - 1);
                    _outputs.RemoveAt(_outputs.Count - 1);
                    outputContainer.RemoveAt(outputContainer.childCount - 1);
                    style.maxWidth = new StyleLength(style.maxWidth.value.value - 250);
                }
            };
            buttonContainer.Add(removeButton);
        }
        protected override void CreateOutputPorts()
        {
            output = PortFactory.CreateVerticalPort(this, "Out Answer 1", Direction.Output, Port.Capacity.Multi, Color.white);
            if (output != null)
            {
                outputContainer.Add(output);
            }
            AnswerSelectorNode answerSelectorNode = storyNode as AnswerSelectorNode;
            if (answerSelectorNode != null)
            {
                if (answerSelectorNode.answers.Count > 0)
                {
                    for (int i = 0; i < answerSelectorNode.answers.Count; i++)
                    {
                        Port port = PortFactory.CreateVerticalPort(this, $"Out Answer {i + 2}", Direction.Output, Port.Capacity.Multi, Color.white);
                        _outputs.Add(port);
                        outputContainer.Add(port);
                        style.maxWidth = new StyleLength(style.maxWidth.value.value + 250);
                    }
                }
            }
        }

        public override List<int> GetChildrenList(int childID)
        {
            AnswerSelectorNode answerSelectorNode = storyNode as AnswerSelectorNode;
            for (int i = 0; i < answerSelectorNode.answers.Count; i++)
            {
                if (answerSelectorNode.answers[i].childrenID.Contains(childID))
                {
                    return answerSelectorNode.answers[i].childrenID;
                }
            }
            if (answerSelectorNode.childrenID.Contains(childID))
            {
                return answerSelectorNode.childrenID;
            }
            return null;
        }
        public override void ConnectNodes(StoryGraphView storyGraphView)
        {
            base.ConnectNodes(storyGraphView);

            AnswerSelectorNode answerSelectorNode = storyNode as AnswerSelectorNode;
            if (answerSelectorNode.answers.Count > 0 && _outputs.Count > 0)
            {
                for (int i = 0; i < _outputs.Count; i++)
                {
                    foreach (var item in answerSelectorNode.answers[i].childrenID)
                    {
                        StoryNodeView child = storyGraphView.FindNodeView(storyNode.storyGraph.GetNodeByID(item, groupNode));
                        Edge edge = _outputs[i].ConnectTo(child.input);
                        storyGraphView.AddElement(edge);
                    }
                }
            }
        }
        public override void ConnectToOutputNode(Edge edge)
        {
            AnswerSelectorNode answerSelectorNode = storyNode as AnswerSelectorNode;
            if (edge.output.portName == "Out Answer 1")
            {
                base.ConnectToOutputNode(edge);
            }
            else
            {
                if (answerSelectorNode.answers.Count > 0)
                {
                    StoryNodeView child = edge.input.node as StoryNodeView;
                    answerSelectorNode.answers[_outputs.IndexOf(edge.output)].childrenID.Add(child.storyNode.id);
                }
            }
        }
        public override void UnConnectToOutputNode(Edge edge)
        {
            AnswerSelectorNode answerSelectorNode = storyNode as AnswerSelectorNode;
            if (edge.output.portName == "Out Answer 1")
            {
                base.UnConnectToOutputNode(edge);
            }
            else
            {
                if (answerSelectorNode.answers.Count > 0)
                {
                    StoryNodeView child = edge.input.node as StoryNodeView;
                    answerSelectorNode.answers[_outputs.IndexOf(edge.output)].childrenID.Remove(child.storyNode.id);
                }
            }
        }
    }
}