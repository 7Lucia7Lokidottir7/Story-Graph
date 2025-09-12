using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace PG.StorySystem.Nodes
{
    public class BranchNode : PropertyNode
    {
        [HideInInspector]
        public int conditionID = -1;

        [HideInInspector]
        public List<int> falseChildrenID = new List<int>();

        protected override void OnEnd(StoryGraph storyGraph)
        {
            // ���� ��� ������ ����������, ��������� ����� ������.
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            ConditionNode conditionNode = storyGraph.GetNodeByID(conditionID, _groupNode) as ConditionNode;
            if (conditionNode == null)
            {
                Debug.LogError($"BranchNode OnStart: ConditionNode � ID = {conditionID} �� ������.");
                return;
            }

            conditionNode.storyGraph = storyGraph;
            conditionNode.InitializeCondition();

            // ��������� ������� �������� ����� ��� ����� ���������
            if (childrenID.Count > 0 && falseChildrenID.Count > 0)
            {
                TransitionToNextNodes(storyGraph);
            }
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            // ���� ��� �������������� �������� �����, �� ��������� �������
            if (falseChildrenID.Count == 0)
            {
                ConditionNode conditionNode = storyGraph.GetNodeByID(conditionID, _groupNode) as ConditionNode;
                if (conditionNode == null)
                {
                    Debug.LogError($"BranchNode OnUpdate: ConditionNode � ID = {conditionID} �� ������.");
                    return;
                }
                if (conditionNode.GetResult())
                {
                    TransitionToNextNodes(storyGraph);
                }
            }
        }

        public override void OnDublicateChildren(Dictionary<int, int> idMapping, StoryNode originalNode)
        {
            base.OnDublicateChildren(idMapping, originalNode);

            BranchNode branchNode = originalNode as BranchNode;
            falseChildrenID.Clear();
            foreach (int oldChildId in branchNode.falseChildrenID)
            {
                if (idMapping.TryGetValue(oldChildId, out int newChildId))
                {
                    falseChildrenID.Add(newChildId); // ��������� ����� ����� � ������������� �����
                }
            }

            if (idMapping.TryGetValue(branchNode.conditionID, out int newConditionChildId))
            {
                conditionID = newConditionChildId; // ��������� ����� � �������� �����
            }
        }

        public override void OnNextNode(StoryGraph storyGraph)
        {
            // �������� ������� ConditionNode � ��������� �� null
            ConditionNode conditionNode = storyGraph.GetNodeByID(conditionID, _groupNode) as ConditionNode;
            if (conditionNode == null)
            {
                Debug.LogError($"BranchNode OnNextNode: ConditionNode � ID = {conditionID} �� ������.");
                return;
            }


            if (conditionNode.GetResult())
            {
                foreach (var child in childrenID)
                {
                    StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);
                    childInstance.StartNode(storyGraph, _groupNode);
                }
            }
            else
            {
                foreach (var child in falseChildrenID)
                {
                    StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);
                    childInstance.StartNode(storyGraph, _groupNode);
                }
            }
        }
    }
}