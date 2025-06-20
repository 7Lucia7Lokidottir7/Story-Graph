using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class RestartToNode : StructureNode
    {
        [SerializeField] private string[] _targetNameNodes;

        protected override void OnUpdate(StoryGraph storyGraph)
        {
            // Этот узел не требует логики обновления
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            // При запуске сразу переходим к целевым узлам
            OnTransitionToNextNode(storyGraph);
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
            // Дополнительные действия при завершении данного узла (если нужны)
        }

        public override void OnNextNode(StoryGraph storyGraph)
        {
            // Завершаем текущую ноду
            End(storyGraph);

            if (_groupNode != null)
            {
                for (int i = 0; i < _targetNameNodes.Length; i++)
                {
                    StoryNode childInstance = _groupNode.FindNode(_targetNameNodes[i]);
                    if (childInstance == null)
                    {
                        Debug.LogWarning($"Child node with name {_targetNameNodes[i]} not found in group {_groupNode.name}");
                        continue;
                    }

                    // Сбрасываем состояние целевой ноды для корректного перезапуска
                    childInstance.RestartNode(storyGraph);
                    
                    if (!_groupNode.currentNodes.Contains(childInstance))
                    {
                        _groupNode.currentNodes.Add(childInstance);
                    }
                    childInstance.Initialize(storyGraph, _groupNode);
                }
            }
            else
            {
                for (int i = 0; i < _targetNameNodes.Length; i++)
                {
                    StoryNode childInstance = storyGraph.FindNode(_targetNameNodes[i]);
                    if (childInstance == null)
                    {
                        Debug.LogWarning($"Child node with name {_targetNameNodes[i]} not found in scenario graph.");
                        continue;
                    }

                    childInstance.RestartNode(storyGraph);

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
