using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class StoryNode : ScriptableObject
    {
        public virtual bool reinitNodeOnStart => true;

        [HideInInspector] public bool isVisibleInTemplate;
        [HideInInspector, System.NonSerialized]
        public bool isStarted = false, isEnded = false;
        [HideInInspector, System.NonSerialized]
        public StoryNode baseNode;

        [HideInInspector]
        public int id;
        [HideInInspector]
        public string guid;
        [HideInInspector]
        public Vector2 nodePosition;
        [HideInInspector]
        public StoryGraph storyGraph;

        public string nameNode;
        [HideInInspector]
        public List<int> childrenID = new List<int>();

        public event System.Action started, ended;

        public virtual string classGUI => "";

        [HideInInspector, TextArea(3, 10)]
        public string description;

        protected BaseGroupNode _groupNode;
        public BaseGroupNode groupNode => _groupNode;

        public void StandardUpdate(StoryGraph storyGraph)
        {
            OnUpdate(storyGraph);
        }

        // Метод для переопределения логики перезапуска в потомках, если потребуется
        public virtual void RestartNode(StoryGraph storyGraph)
        {
        }

        public void StartNode(StoryGraph storyGraph, BaseGroupNode groupNode = null)
        {
            this.storyGraph = storyGraph;
            _groupNode = groupNode;

            // Если нода не запущена или уже завершена, запускаем её заново
            if (!isStarted || isEnded)
            {
                isEnded = false;
                isStarted = true;
                started?.Invoke();

                // Добавление ноды в список текущих
                List<StoryNode> currentNodes = (_groupNode != null) ? _groupNode.currentNodes : storyGraph.currentNodes;
                if (!currentNodes.Contains(this))
                    currentNodes.Add(this);

                Init(storyGraph);
                OnStart(storyGraph);
            }
            else if (isStarted && !isEnded)
            {
                OnNotFirstStart(storyGraph);
            }
        }

        protected virtual void Init(StoryGraph storyGraph)
        {
            // Базовая инициализация ноды
        }

        protected virtual void OnNotFirstStart(StoryGraph storyGraph)
        {
            // Логика, если нода уже запущена и происходит повторный вход
        }

        public void EndNode(StoryGraph storyGraph)
        {
            if (!isEnded)
            {
                isEnded = true;
                isStarted = false;
                ended?.Invoke();
                OnEnd(storyGraph);

                // Удаляем ноду из списка текущих
                var list = _groupNode != null ? _groupNode.currentNodes : storyGraph.currentNodes;
                if (list.Contains(this))
                {
                    list.Remove(this);
                }
            }
        }

        public virtual void OnDublicate()
        {
            childrenID.Clear();
        }

        public virtual void OnDublicateChildren(Dictionary<int, int> idMapping, StoryNode originalNode)
        {
            childrenID.Clear();
            foreach (int oldChildId in originalNode.childrenID)
            {
                if (idMapping.TryGetValue(oldChildId, out int newChildId))
                {
                    childrenID.Add(newChildId);
                }
            }
        }

        public virtual void TransitionToNextNodes(StoryGraph storyGraph)
        {
            EndNode(storyGraph);

            OnNextNode(storyGraph);
        }

        public virtual void OnNextNode(StoryGraph storyGraph)
        {
            foreach (var childID in childrenID)
            {
                StoryNode childInstance = storyGraph.GetNodeByID(childID, _groupNode);
                childInstance.StartNode(storyGraph, _groupNode);
            }
        }

        protected abstract void OnStart(StoryGraph storyGraph);
        protected abstract void OnEnd(StoryGraph storyGraph);
        protected abstract void OnUpdate(StoryGraph storyGraph);
    }
}
