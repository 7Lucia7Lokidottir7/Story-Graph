using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public abstract class StoryNode : ScriptableObject
    {
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

        public async void Initialize(StoryGraph storyGraph, BaseGroupNode groupNode = null, bool useToCurrentNodes = true)
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
                await Task.Delay(10);
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

        public void End(StoryGraph storyGraph)
        {
            if (!isEnded)
            {
                isEnded = true;
                isStarted = false;
                ended?.Invoke();
                OnEnd(storyGraph);
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

        public virtual void OnTransitionToNextNode(StoryGraph storyGraph)
        {
            End(storyGraph);
            // Удаляем ноду из списка текущих
            if (_groupNode != null)
            {
                _groupNode.currentNodes.Remove(this);
            }
            else
            {
                storyGraph.currentNodes.Remove(this);
            }

            OnNextNode(storyGraph);
        }

        public virtual void OnNextNode(StoryGraph storyGraph)
        {
            End(storyGraph);
            if (_groupNode != null)
            {
                foreach (var childID in childrenID)
                {
                    StoryNode childInstance = storyGraph.GetNodeByID(childID, _groupNode);
                    List<StoryNode> currentNodes = _groupNode.currentNodes;
                    if (!currentNodes.Contains(childInstance))
                        currentNodes.Add(childInstance);
                    childInstance.Initialize(storyGraph, _groupNode);
                }
            }
            else
            {
                foreach (var childID in childrenID)
                {
                    StoryNode childInstance = storyGraph.GetNodeByID(childID);
                    List<StoryNode> currentNodes = storyGraph.currentNodes;
                    if (!currentNodes.Contains(childInstance))
                        currentNodes.Add(childInstance);
                    childInstance.Initialize(storyGraph);
                }
            }
        }

        protected abstract void OnStart(StoryGraph storyGraph);
        protected abstract void OnEnd(StoryGraph storyGraph);
        protected abstract void OnUpdate(StoryGraph storyGraph);
    }
}
