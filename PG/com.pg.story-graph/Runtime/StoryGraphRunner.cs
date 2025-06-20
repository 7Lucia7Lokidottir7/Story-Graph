using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PG.StorySystem
{
    using Nodes;
    using System.Collections;
    using System.IO;
    using System.Threading.Tasks;

    public sealed class StoryGraphRunner : MonoBehaviour
    {
        private StoryGraph _storyGraph;
        public StoryGraph currentStoryGraph => _storyGraph;
        public StoryGraph storyGraph => baseStoryGraph;
        [field:SerializeField] public StoryGraph baseStoryGraph { get; private set; }

        internal Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();
        public Dictionary<string, StoryVariable> variables = new Dictionary<string, StoryVariable>();

        [SerializeField] private bool _isStartGraphOnAwake = true;
        ObjectElement[] objectElements;

        private string _filename = "Story.json";
        private string _filepath => Path.Combine(Application.persistentDataPath, _filename); 
        private SaveStoryGraph _saveStoryGraph = new SaveStoryGraph();

        [System.Serializable]
        public class SaveStoryGraph
        {
            public List<SaveElement> elements = new List<SaveElement>();
            public List<SaveVaribale> variables = new List<SaveVaribale>();
            [System.Serializable]
            public class SaveElement 
            {
                public int id;
                public bool insideGroup;
                public int groupId;
            }
            [System.Serializable]
            public class SaveVaribale
            {
                public string name;
                public object value;
            }
        }
        private void Awake()
        {
            _storyGraph = baseStoryGraph;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (_isStartGraphOnAwake && isActiveAndEnabled)
            {
                InitializeGraphRunner();
            }
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(WaitAndInitializeObjects());
        }

        private IEnumerator WaitAndInitializeObjects()
        {
            yield return null; // Ждем один кадр, чтобы сцена успела инициализироваться
            InitializeObjects();
        }

        public void InitializeGraphRunner()
        {
            if (_storyGraph)
            {
                InitializeObjects();
                _storyGraph = Instantiate(_storyGraph);
                variables.Clear();
                for (int i = 0; i < _storyGraph.variables.Count; i++)
                {
                    _storyGraph.variables[i] = Instantiate(_storyGraph.variables[i]);
                    variables.Add(_storyGraph.variables[i].variableName, _storyGraph.variables[i]);
                }
                _storyGraph.runner = this;
                //StoryNode rootNode = Instantiate(_storyGraph.rootNode);

                //rootNode.Initialize(_storyGraph);
                _storyGraph = _storyGraph.CloneGraph();

                Load();


                _storyGraph.Initialize();
            }
        }


        private void InitializeObjects()
        {
            objectElements = FindObjectsByType<ObjectElement>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            gameObjects.Clear();
            for (int i = 0; i < objectElements.Length; i++)
            {
                string key = objectElements[i].objectNameID;
                if (!gameObjects.ContainsKey(key))
                {
                    gameObjects.Add(key, objectElements[i].gameObject);
                }
                else
                {
                    Debug.LogWarning($"Duplicate key detected: {key}. Skipping duplicate entry.");
                }
            }
        }
        /// <summary>
        /// Рекурсивно собирает в список SAVE-элементы: сначала саму группу, затем всех её детей (включая вложенные группы).
        /// </summary>
        private List<SaveStoryGraph.SaveElement> SaveGroup(BaseGroupNode group)
        {
            var elements = new List<SaveStoryGraph.SaveElement>();

            // 1) сохраняем саму группу
            var groupElement = new SaveStoryGraph.SaveElement
            {
                id = group.id,
                insideGroup = group.groupNode != null,
                groupId = group.groupNode != null ? group.groupNode.id : -1
            };
            elements.Add(groupElement);

            // 2) для каждого ребёнка внутри этой группы
            foreach (StoryNode child in group.currentNodes)
            {
                if (child is BaseGroupNode childGroup)
                {
                    // рекурсивно сохраняем всю вложенную группу
                    elements.AddRange(SaveGroup(childGroup));
                }
                else
                {
                    // сохраняем простой узел (не группа)
                    var element = new SaveStoryGraph.SaveElement
                    {
                        id = child.id,
                        insideGroup = child.groupNode != null,
                        groupId = child.groupNode != null ? child.groupNode.id : -1
                    };
                    elements.Add(element);
                }
            }

            return elements;
        }

        /// <summary>
        /// Сохранение всего графа, включая вложенные группы.
        /// </summary>
        public void Save()
        {
            _saveStoryGraph.elements.Clear();

            // Проходим по всем узлам верхнего уровня в _storyGraph.currentNodes
            foreach (StoryNode node in _storyGraph.currentNodes)
            {
                if (node is BaseGroupNode topGroup)
                {
                    // рекурсивно сохранить эту группу (вместе с её детьми)
                    _saveStoryGraph.elements.AddRange(SaveGroup(topGroup));
                }
                else
                {
                    // обычный узел верхнего уровня
                    var element = new SaveStoryGraph.SaveElement
                    {
                        id = node.id,
                        insideGroup = node.groupNode != null,
                        groupId = node.groupNode != null ? node.groupNode.id : -1
                    };
                    _saveStoryGraph.elements.Add(element);
                }
            }


            _saveStoryGraph.variables.Clear();
            for (int i = 0; i < _storyGraph.variables.Count; i++)
            {
                _saveStoryGraph.variables.Add(new SaveStoryGraph.SaveVaribale
                {
                    name = _storyGraph.variables[i].variableName,
                    value = _storyGraph.variables[i].GetValue()
                });
            }




            // Сериализуем в JSON и записываем в файл
            string json = JsonUtility.ToJson(_saveStoryGraph, prettyPrint: true);
            File.WriteAllText(_filepath, json);
            Debug.Log($"StoryGraph сохранён в {_filepath}");
        }

        /// <summary>
        /// Загрузка и восстановление иерархии узлов. 
        /// </summary>
        public void Load()
        {
            if (!File.Exists(_filepath))
            {
                Debug.LogWarning($"Файл {_filepath} не найден.");
                return;
            }

            // 1) Считываем JSON в _saveStoryGraph
            string json = File.ReadAllText(_filepath);
            JsonUtility.FromJsonOverwrite(json, _saveStoryGraph);

            // 2) Очищаем текущее состояние: 
            //    - Очищаем верхний уровень _storyGraph.currentNodes
            //    - Очищаем списки currentNodes у всех групп
            // Чтобы найти все группы, пробежимся по сохранённому списку элементов и возьмём только те, чьи узлы являются группами.
            foreach (var element in _saveStoryGraph.elements)
            {
                StoryNode rawNode = _storyGraph.GetNodeByID(element.id);
                if (rawNode is BaseGroupNode groupNode)
                {
                    groupNode.currentNodes.Clear();
                }
            }
            _storyGraph.currentNodes.Clear();

            // 3) Проходим по каждому сохранённому элементу в том порядке, в котором они сохранились.
            //    Благодаря тому, что мы при сохранении записывали сначала родительскую группу, а потом её детей, при загрузке
            //    каждый раз, когда встречаем элемент, соответствующая BaseGroupNode (если это она) уже добавлена туда, куда нужно.
            foreach (var element in _saveStoryGraph.elements)
            {
                StoryNode nodeToRestore = _storyGraph.GetNodeByID(element.id);
                if (nodeToRestore == null)
                {
                    Debug.LogError($"Не удалось найти узел с ID = {element.id} при загрузке.");
                    continue;
                }

                if (element.insideGroup)
                {
                    // найдём родитель-группу по сохранённому groupId
                    BaseGroupNode parentGroup = _storyGraph.GetNodeByID(element.groupId) as BaseGroupNode;
                    if (parentGroup == null)
                    {
                        Debug.LogError($"Не удалось найти группу с ID = {element.groupId} для узла ID = {element.id}");
                        continue;
                    }
                    parentGroup.currentNodes.Add(nodeToRestore);
                }
                else
                {
                    // узел верхнего уровня
                    _storyGraph.currentNodes.Add(nodeToRestore);
                }
            }



            for (int i = 0; i < _saveStoryGraph.variables.Count; i++)
            {
                for (int a = 0; a < _storyGraph.variables.Count; a++)
                {
                    if (_storyGraph.variables[a].variableName == _saveStoryGraph.variables[i].name)
                    {
                        _storyGraph.variables[a].SetValue(_saveStoryGraph.variables[i].value);
                    }
                }
            }

            Debug.Log("StoryGraph загружен и восстановлена иерархия узлов.");
        }
        public void TransitionToNextNode(string nameNode)
        {
            _storyGraph.FindCurrentNode(nameNode).OnTransitionToNextNode(_storyGraph);
        }
        public void StopNode(string nameNode)
        {
            StoryNode storyNode = _storyGraph.FindCurrentNode(nameNode);
            storyNode.End(_storyGraph);
            _storyGraph.currentNodes.Remove(storyNode);
        }


        // Update is called once per frame
        void Update()
        {
            if (_storyGraph)
            {
                _storyGraph.OnUpdate();
            }
        }
        #region Variables
        public void SetVariableValue(string name, object value)
        {
            variables.TryGetValue(name, out StoryVariable variable);
            variable.SetValue(value);
        }
        public object GetVariableValue(string name)
        {
            variables.TryGetValue(name, out StoryVariable variable);
            return variable.GetValue();
        }
    }
    #endregion
}
