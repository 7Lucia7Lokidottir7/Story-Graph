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
            TransitionToNextNodes(storyGraph);
        }

        protected override void OnEnd(StoryGraph storyGraph)
        {
            // Дополнительные действия при завершении данного узла (если нужны)
        }

        public override void OnNextNode(StoryGraph storyGraph)
        {
            for (int i = 0; i < _targetNameNodes.Length; i++)
            {
                StoryNode childInstance = storyGraph.FindNode(_targetNameNodes[i]);
                if (childInstance == null)
                {
                    Debug.LogWarning($"Child node with name {_targetNameNodes[i]} not found in group {_groupNode.name}");
                    continue;
                }

                // Сбрасываем состояние целевой ноды для корректного перезапуска
                childInstance.RestartNode(storyGraph);

                childInstance.StartNode(storyGraph, _groupNode);
            }
        }
    }
}
