using UnityEditor;
using UnityEngine;

namespace PG.StorySystem
{
    using Nodes;
    using UnityEngine.UIElements;
    using UnityEditor.Experimental.GraphView;
    using System.Collections.Generic;
    public class StoryNodeView : Node
    {
        private const string _variableContainerName = "variable-container";
        public System.Action<StoryNodeView> onNodeSelected;
        [System.NonSerialized] public StoryNode storyNode;
        [System.NonSerialized] public string guid;
        [System.NonSerialized] public Port input;

        private Label _nameNodeLabel;
        private Label _descriptionLabel;

        public BaseGroupNode groupNode => StoryGraphEditorWindow.graphView.currentGroupNode;

        [System.NonSerialized] public Port output;

        public event System.Action<StoryNode> updateNode;
        
        public StoryNodeView(StoryNode node) : base(AssetDatabase.GetAssetPath(Resources.Load("StoryGraph/NodeView")))
        {
            MainStyle(node);

            CreateInputPorts();
            CreateOutputPorts();
        }
        protected void MainStyle(StoryNode node)
        {
            if (node == null)
            {
                return;
            }

            storyNode = node;

            ApplyTextData(node);
            //title = node.GetName() + " " + GetType().Name;
            guid = node.guid;
            viewDataKey = guid;
            TopNodeColorStyle();

        }
        protected void ApplyTextData(StoryNode node)
        {
            var field = AttributeHelper.GetFieldWithNodeTitleAttribute(node);
            if (field != null)
            {
                var value = field.GetValue(node);
                title = node.GetName() + $"({value})";
            }
            else
            {
                title = node.GetName();
            }

            _descriptionLabel = this.Q<Label>("description");
            _descriptionLabel.text = node.description;
            _nameNodeLabel = this.Q<Label>("nameNode-label");
            _nameNodeLabel.text = node.nameNode;
        }
        protected void TopNodeColorStyle()
        {
            Color color = storyNode.colorNode;

            // Преобразуем в HSV, понижаем насыщенность, обратно в RGB:
            Color.RGBToHSV(color, out float h, out float s, out float v);

            s *= 0.6f; // Вот, понижаем насыщенность в 2 раза! Можешь поставить 0.3f, если хочешь еще тусклее
            v *= 0.8f;

            Color lessSaturated = Color.HSVToRGB(h, s, v);
            this.Q<VisualElement>("divider-style").style.backgroundColor = lessSaturated;
        }

        public virtual List<int> GetChildrenList(int childID)
        {
            if (storyNode.childrenID.Contains(childID))
            {
                return storyNode.childrenID;
            }
            return null;
        }
        public void UpdateCurrentState()
        {
            const string classRunName = "running";
            const string classCompleteName = "complete";

            RemoveFromClassList(classRunName);
            RemoveFromClassList(classCompleteName);
            if (Application.isPlaying && storyNode.isStarted)
            {
                AddToClassList(classRunName);
            }
            if (Application.isPlaying && storyNode.isEnded)
            {
                AddToClassList(classCompleteName);
            }
        }
        public virtual Port GetOutputPort(int portID)
        {
            return null;
        }
        public virtual Port GetInputPort(int portID)
        {
            return null;
        }
        protected virtual void CreateVariableInputNode()
        {
        }
        public void UpdateDescriptionText(StoryNode node)
        {
            _nameNodeLabel.text = node.nameNode;
            _descriptionLabel.text = node.description;
            updateNode?.Invoke(node);
        }

        protected virtual void CreateInputPorts()
        {
            input = PortFactory.CreateVerticalPort(this, "In", Direction.Input, Port.Capacity.Single, Color.white);
            if (input != null)
            {
                inputContainer.Add(input);
            }
        }

        protected virtual void CreateOutputPorts()
        {
            output = PortFactory.CreateVerticalPort(this, "Out", Direction.Output, Port.Capacity.Multi, Color.white);
            if (output != null)
            {
                outputContainer.Add(output);
            }
        }

        public virtual void ConnectNodes(StoryGraphView storyGraphView)
        {
            if (StoryGraphEditorWindow.graphView.currentGroupNode != null)
            {
                BaseGroupNode groupNode = StoryGraphEditorWindow.graphView.currentGroupNode;
                storyNode.childrenID.ForEach(childId =>
                {
                    StoryNodeView childNode = storyGraphView.FindNodeView(groupNode.storyGraph.GetNodeByID(childId, groupNode));
                    if (childNode != null && output != null && childNode.input != null)
                    {
                        Edge edge = output.ConnectTo(childNode.input);
                        storyGraphView.AddElement(edge);
                    }
                    else
                    {
                        Debug.LogError("Parent or child node is null. Check if nodes are correctly instantiated.");
                    }
                });
            }
            else
            {
                List<int> childrenCopy = new List<int>(storyNode.childrenID);
                childrenCopy.ForEach(childId =>
                {
                    StoryNodeView childNode = storyGraphView.FindNodeView(storyNode.storyGraph.GetNodeByID(childId, groupNode));
                    if (childNode == null)
                    {
                        // Если нода не найдена, удаляем её из оригинального списка
                        storyNode.childrenID.Remove(childId);
                    }
                    else if (output != null && childNode.input != null)
                    {
                        Edge edge = output.ConnectTo(childNode.input);
                        storyGraphView.AddElement(edge);
                    }
                    else
                    {
                        Debug.LogError("Parent or child node is null. Check if nodes are correctly instantiated.");
                    }
                });

            }

        }
        public virtual void ConnectToInputNode(Edge edge)
        {
        }
        public virtual void ConnectFromOutputPort(Edge edge)
        {
            StoryNodeView parent = edge.output.node as StoryNodeView;
            StoryNodeView child = edge.input.node as StoryNodeView;

            storyNode.storyGraph.AddChild(parent.storyNode, child.storyNode);
        }
        public virtual void DisconnectFromInputPort(Edge edge)
        {
        }
        public virtual void DisconnectFromOutputPort(Edge edge)
        {
            if (edge != null)
            {
                StoryNodeView parent = edge.output.node as StoryNodeView;
                StoryNodeView child = edge.input.node as StoryNodeView;

                if (parent == null || child == null)
                {
                    Debug.LogError("Parent or child node is null.");
                    return;
                }

                // Удаляем основное дочернее отношение
                storyNode.storyGraph.RemoveChild(parent.storyNode, child.storyNode);

                EditorUtility.SetDirty(storyNode.storyGraph);
                
            }
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(storyNode, "Story Graph (Set Position Node)");
            storyNode.nodePosition.x = newPos.xMin;
            storyNode.nodePosition.y = newPos.yMin;
            EditorUtility.SetDirty(storyNode);
        }
        public override void OnSelected()
        {
            base.OnSelected();
            onNodeSelected?.Invoke(this);
        }
    }
}
