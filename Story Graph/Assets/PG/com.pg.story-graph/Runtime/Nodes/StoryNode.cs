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

        // Безопасная проверка на null, чтобы редактор не падал при выделении ассета
        public bool isStarted => state != null && state.isStarted;
        public bool isEnded => state != null && state.isEnded;

        [HideInInspector, System.NonSerialized]
        public StoryNode baseNode;

        [HideInInspector] public int id;
        [HideInInspector] public string guid;
        [HideInInspector] public Vector2 nodePosition;
        [HideInInspector] public StoryGraph storyGraph;

        public string nameNode;
        [HideInInspector] public List<int> childrenID = new List<int>();

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
            [System.NonSerialized]
            public List<NodeState> currentNodes = new List<NodeState>();
        }

        // Исправленный метод: Создает стейт чисто и жестко
        public NodeState CreateState()
        {
            state = new NodeState();
            state.storyNode = this;
            state.isStarted = false;
            state.isEnded = false;
            // Гарантируем, что список инициализирован
            if (state.currentNodes == null) state.currentNodes = new List<NodeState>();
            return state;
        }

        public virtual void RestartNode(StoryGraph storyGraph)
        {
        }

        public void StartNode(StoryGraph storyGraph, BaseGroupNode groupNode = null)
        {
            this.storyGraph = storyGraph;
            _groupNode = groupNode;

            // 1. ЖЕСТКИЙ СБРОС КОРУТИНЫ
            // Если мы загружаемся или перезапускаемся, старая корутина — это яд.
            // Она может всё ещё пытаться работать со старым (уже затертым) стейтом.
            if (_updateCoroutine != null)
            {
                storyGraph.StopCoroutine(_updateCoroutine);
                _updateCoroutine = null;
            }

            // 2. ПЕРЕЗАПИСЬ СТЕЙТА
            // Теперь CreateState() гарантированно создает новый объект
            state = CreateState();

            // 3. ЛОГИКА ЗАПУСКА
            if (!isStarted || isEnded)
            {
                state.isEnded = false;
                state.isStarted = true;

                if (state.storyNode == null) state.storyNode = this;

                started?.Invoke();

                // Определяем список, куда нужно добавить ноду
                List<NodeState> currentNodesList = (_groupNode != null) ? _groupNode.state.currentNodes : storyGraph.currentNodes;

                if (!currentNodesList.Contains(state))
                {
                    currentNodesList.Add(state);
                }

                Init(storyGraph);
                OnStart(storyGraph);
            }
            else if (isStarted && !isEnded)
            {
                OnNotFirstStart(storyGraph);
            }

            // 4. ГАРАНТИРОВАННЫЙ ЗАПУСК ОБНОВЛЕНИЯ
            // Теперь, когда мы обнулили корутину в начале метода, это условие сработает железно.
            if (_updateCoroutine == null && useUpdate)
            {
                _updateCoroutine = storyGraph.StartCoroutine(OnUpdate(storyGraph));
            }
        }


        protected virtual void Init(StoryGraph storyGraph) { }
        protected virtual void OnNotFirstStart(StoryGraph storyGraph) { }

        public void EndNode(StoryGraph storyGraph)
        {
            if (!isEnded)
            {
                // Обязательно проверяем на null перед записью
                if (state != null)
                {
                    state.isEnded = true;
                    state.isStarted = false;
                }

                ended?.Invoke();
                OnEnd(storyGraph);

                if (_updateCoroutine != null)
                {
                    storyGraph.StopCoroutine(_updateCoroutine);
                    _updateCoroutine = null;
                }

                var list = _groupNode != null ? _groupNode.state.currentNodes : storyGraph.currentNodes;

                if (state != null && list.Contains(state))
                {
                    list.Remove(state);
                }
            }
        }

        public virtual void OnDublicate() { childrenID.Clear(); }

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
                if (childInstance != null) // Проверка на null
                    childInstance.StartNode(storyGraph, _groupNode);
            }
        }

        protected abstract void OnStart(StoryGraph storyGraph);
        protected virtual void OnEnd(StoryGraph storyGraph) { }
        protected virtual IEnumerator OnUpdate(StoryGraph storyGraph) { yield break; }
    }
}