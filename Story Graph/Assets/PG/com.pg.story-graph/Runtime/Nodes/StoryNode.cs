using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class StoryNode : ScriptableObject
    {
        public virtual Color colorNode => Color.indianRed;
        public virtual bool reinitNodeOnStart => true;

        [HideInInspector] public bool isVisibleInTemplate;
        [HideInInspector]
        public bool isStarted => state.isStarted; 
        public bool isEnded => state.isEnded;

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


        [HideInInspector, TextArea(3, 10)]
        public string description;

        protected BaseGroupNode _groupNode;
        public BaseGroupNode groupNode => _groupNode;

        protected virtual bool useUpdate => false;
        protected Coroutine _updateCoroutine;

        [System.NonSerialized]
        public NodeState state = new NodeState();

        [System.Serializable]
        public class NodeState
        {
            public StoryNode storyNode;
            public bool isStarted;
            public bool isEnded;
            // Инициализируем список сразу, чтобы избежать NullReference
            [System.NonSerialized]
            public List<NodeState> currentNodes = new List<NodeState>();
        }

        public NodeState CreateState()
        {
            state = new NodeState();
            state.storyNode = this;
            return state;
        }

        // Метод для переопределения логики перезапуска в потомках, если потребуется
        public virtual void RestartNode(StoryGraph storyGraph)
        {
        }

        public void StartNode(StoryGraph storyGraph, BaseGroupNode groupNode = null)
        {
            this.storyGraph = storyGraph;
            _groupNode = groupNode;
            state = CreateState();

            // Если нода не запущена или уже завершена, запускаем её заново
            if (!isStarted || isEnded)
            {
                state.isEnded = false;
                state.isStarted = true;

                // Важно: привязываем ссылку на саму ноду к состоянию, если она потерялась
                if (state.storyNode == null) state.storyNode = this;

                started?.Invoke();

                // Добавление ноды в список текущих
                List<NodeState> currentNodes = (_groupNode != null) ? _groupNode.state.currentNodes : storyGraph.currentNodes;

                // ИЗМЕНЕНИЕ 2: Так как это теперь class, проверка Contains работает по ссылке корректно
                if (!currentNodes.Contains(state))
                    currentNodes.Add(state);

                Init(storyGraph);
                OnStart(storyGraph);
            }
            else if (isStarted && !isEnded)
            {
                OnNotFirstStart(storyGraph);
            }
            if (_updateCoroutine == null && useUpdate)
            {
                _updateCoroutine = storyGraph.StartCoroutine(OnUpdate(storyGraph));
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
                state.isEnded = true;
                state.isStarted = false;
                ended?.Invoke();
                OnEnd(storyGraph);
                if (_updateCoroutine != null)
                {
                    storyGraph.StopCoroutine(_updateCoroutine);
                    _updateCoroutine = null;
                }

                // Удаляем ноду из списка текущих
                var list = _groupNode != null ? _groupNode.state.currentNodes : storyGraph.currentNodes;

                // Теперь Remove удалит именно этот экземпляр класса
                if (list.Contains(state))
                {
                    list.Remove(state);
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
        protected virtual void OnEnd(StoryGraph storyGraph) { }
        protected virtual IEnumerator OnUpdate(StoryGraph storyGraph) 
        {
            yield break;
        }

    }
}
