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
            // Если нет логики завершения, оставляем метод пустым.
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            ConditionNode conditionNode = storyGraph.GetNodeByID(conditionID, _groupNode) as ConditionNode;
            if (conditionNode == null)
            {
                Debug.LogError($"BranchNode OnStart: ConditionNode с ID = {conditionID} не найден.");
                return;
            }

            conditionNode.storyGraph = storyGraph;
            conditionNode.InitializeCondition();

            // Проверяем наличие дочерних узлов для обоих вариантов
            if (childrenID.Count > 0 && falseChildrenID.Count > 0)
            {
                OnTransitionToNextNode(storyGraph);
            }
        }

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            // Если нет альтернативных дочерних узлов, то проверяем условие
            if (falseChildrenID.Count == 0)
            {
                ConditionNode conditionNode = storyGraph.GetNodeByID(conditionID, _groupNode) as ConditionNode;
                if (conditionNode == null)
                {
                    Debug.LogError($"BranchNode OnUpdate: ConditionNode с ID = {conditionID} не найден.");
                    return;
                }
                if (conditionNode.GetResult())
                {
                    OnTransitionToNextNode(storyGraph);
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
                    falseChildrenID.Add(newChildId); // Добавляем новую связь к скопированным нодам
                }
            }

            if (idMapping.TryGetValue(branchNode.conditionID, out int newConditionChildId))
            {
                conditionID = newConditionChildId; // Обновляем связь с условным узлом
            }
        }

        public override void OnNextNode(StoryGraph storyGraph)
        {
            End(storyGraph);

            // Получаем текущий ConditionNode с проверкой на null
            ConditionNode conditionNode = storyGraph.GetNodeByID(conditionID, _groupNode) as ConditionNode;
            if (conditionNode == null)
            {
                Debug.LogError($"BranchNode OnNextNode: ConditionNode с ID = {conditionID} не найден.");
                return;
            }

            // Если группа определена, работаем с ней
            if (_groupNode != null)
            {
                if (conditionNode.GetResult())
                {
                    foreach (var child in childrenID)
                    {
                        StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);
                        if (childInstance == null)
                        {
                            Debug.LogWarning($"BranchNode: Дочерний узел с ID = {child} не найден.");
                            continue;
                        }

                        if (!_groupNode.currentNodes.Contains(childInstance))
                        {
                            _groupNode.currentNodes.Add(childInstance);
                        }

                        childInstance.Initialize(storyGraph, _groupNode);
                    }
                }
                else
                {
                    foreach (var child in falseChildrenID)
                    {
                        StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);
                        if (childInstance == null)
                        {
                            Debug.LogWarning($"BranchNode: Альтернативный дочерний узел с ID = {child} не найден.");
                            continue;
                        }

                        if (!_groupNode.currentNodes.Contains(childInstance))
                        {
                            _groupNode.currentNodes.Add(childInstance);
                        }

                        childInstance.Initialize(storyGraph, _groupNode);
                    }
                }
            }
            // Если группа не определена, работаем через общий список узлов графа
            else
            {
                if (conditionNode.GetResult())
                {
                    foreach (var child in childrenID)
                    {
                        StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);
                        if (childInstance == null)
                        {
                            Debug.LogWarning($"BranchNode: Дочерний узел с ID = {child} не найден.");
                            continue;
                        }

                        if (!storyGraph.currentNodes.Contains(childInstance))
                        {
                            storyGraph.currentNodes.Add(childInstance);
                        }

                        childInstance.Initialize(storyGraph);
                    }
                }
                else
                {
                    foreach (var child in falseChildrenID)
                    {
                        StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);
                        if (childInstance == null)
                        {
                            Debug.LogWarning($"BranchNode: Альтернативный дочерний узел с ID = {child} не найден.");
                            continue;
                        }

                        if (!storyGraph.currentNodes.Contains(childInstance))
                        {
                            storyGraph.currentNodes.Add(childInstance);
                        }

                        childInstance.Initialize(storyGraph);
                    }
                }
            }
        }
    }
}