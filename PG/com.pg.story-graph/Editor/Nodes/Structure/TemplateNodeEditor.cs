using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEditor.UIElements;
    using UnityEngine;

    [CustomEditor(typeof(TemplateNode))]
    public class TemplateNodeEditor : StoryNodeEditor
    {
        private TemplateNode _templateNode;
        protected override void Init()
        {
            base.Init();
            _templateNode = target as TemplateNode;
        }
        public override void OnCustomElement(VisualElement root)
        {
            base.OnCustomElement(root);
            Button importButton = new Button();
            importButton.text = "Import";
            importButton.clicked += OnImport;
            root.Add(importButton);


            Foldout foldout = new Foldout();
            foldout.text = "Nodes";
            ApplyStyleFoldout(foldout);

            foldout.value = true;
            foldout.RegisterValueChangedCallback(evt =>
            {
                // игнорируем события от вложенных Foldout-ов
                if (evt.target != foldout) return;
                if (!evt.newValue) return; // например, только на открытие

                foldout.contentContainer.Clear();
                OnVisibleNodes(foldout, _templateNode.storyNodes);
            });
            root.Add(foldout);
            OnVisibleNodes(foldout, _templateNode.storyNodes);
        }
        public void OnVisibleNodes(VisualElement root, List<StoryNode> storyNodes)
        {
            List<StoryNode> nodes = new List<StoryNode>();
            foreach (var item in storyNodes)
            {
                if (item.isVisibleInTemplate)
                {
                    nodes.Add(item);
                }
            }

            foreach (var item in nodes)
            {
                Foldout foldout = new Foldout();
                root.Add(foldout);
                foldout.text = item.GetType().Name;
                foldout.value = false;
                foldout.Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;


                Editor editor = CreateEditor(item);
                IMGUIContainer iMGUIContainer = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
                foldout.Add(iMGUIContainer);

                foldout.Add(editor.CreateInspectorGUI());
            }
        }
        void ApplyStyleFoldout(Foldout foldout)
        {
            foldout.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f);
            foldout.style.borderBottomLeftRadius = 5f;
            foldout.style.borderBottomRightRadius = 5f;
            foldout.style.borderTopLeftRadius = 5f;
            foldout.style.borderTopRightRadius = 5f;
            foldout.Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;


            foldout.style.paddingLeft = 4;
            foldout.style.paddingRight = 4;
            foldout.style.paddingTop = 4;
            foldout.style.paddingBottom = 4;
            foldout.style.marginBottom = 4;
            foldout.style.marginTop = 4;
        }
        void OnImport()
        {
            PasteOperation();
            if (_templateNode.importObjects)
            {
                var newList = _templateNode.template.objects.Where(o => !_templateNode.storyGraph.objects.Contains(o));
                _templateNode.storyGraph.objects.AddRange(newList);
            }
            if (_templateNode.importVariables)
            {
                List<StoryVariable> newList = _templateNode.template.variables.Where(v => !_templateNode.storyGraph.variables.Contains(v)) as List<StoryVariable>;
                for (int i = 0; i < newList.Count(); i++)
                {
                    newList[i] = Instantiate(newList[i]);
                    AssetDatabase.AddObjectToAsset(newList[i], _templateNode.storyGraph);
                }
                _templateNode.storyGraph.variables.AddRange(newList);
            }
        }
        private void PasteOperation()
        {

            Dictionary<int, int> idMapping = new Dictionary<int, int>(); // Соответствие старых и новых ID
            List<StoryNode> newNodes = new List<StoryNode>(); // Хранение всех новых нод для последующей обработки связей

            for (int i = 1; i < _templateNode.template.nodes.Count; i++)
            {
                StoryNode originalNode = _templateNode.template.nodes[i];


                StoryNode newNode = ScriptableObject.Instantiate(originalNode);

                // Создаем новый уникальный ID и сохраняем соответствие
                newNode.guid = GUID.Generate().ToString();
                newNode.nodePosition += new Vector2(25, 40);

                _templateNode.storyGraph.nextId++;
                int oldId = originalNode.id;
                int newId = _templateNode.storyGraph.nextId;
                newNode.id = newId;
                idMapping[oldId] = newId;

                // Отсоединяем от старых детей
                newNode.childrenID = new List<int>();

                // Добавляем новую ноду в AssetDatabase
                AssetDatabase.AddObjectToAsset(newNode, _templateNode.storyGraph);
                if (newNode is BaseGroupNode groupNode1)
                {
                    CopyGroup(groupNode1);
                }
                Undo.RegisterCreatedObjectUndo(newNode, "Story Graph (Copied Node)");

                // Добавляем новую ноду в граф
                Undo.RecordObject(_templateNode.storyGraph, "Story Graph (Copied Node)");
                _templateNode.storyNodes.Add(newNode);

                newNodes.Add(newNode); // Сохраняем для последующей обработки
            }

            // Восстанавливаем связи между скопированными нодами
            foreach (StoryNode newNode in newNodes)
            {
                StoryNode originalNode = _templateNode.template.nodes.FirstOrDefault(node => node.id == idMapping.FirstOrDefault(pair => pair.Value == newNode.id).Key);

                if (originalNode != null)
                {
                    newNode.OnDublicateChildren(idMapping, originalNode);
                }
            }

            StoryGraphEditorWindow.graphView.PopulateView(_templateNode.storyGraph);
            AssetDatabase.SaveAssets();
        }

        void CopyGroup(BaseGroupNode groupNode)
        {
            List<StoryNode> originalNodes = new List<StoryNode>(groupNode.storyNodes); // Сохраняем исходные ссылки
            List<StoryNode> newNodes = new List<StoryNode>(); // Для хранения новых скопированных нод

            Dictionary<int, int> idMapping = new Dictionary<int, int>(); // Для сохранения старых и новых ID

            // Копируем каждую ноду
            foreach (StoryNode originalNode in originalNodes)
            {
                StoryNode newNode = ScriptableObject.Instantiate(originalNode);

                // Создаем новый уникальный ID
                newNode.guid = GUID.Generate().ToString();
                newNode.nodePosition += new Vector2(25, 40);

                _templateNode.storyGraph.nextId++;
                int oldId = originalNode.id;
                int newId = _templateNode.storyGraph.nextId;
                newNode.id = newId;

                // Сохраняем соответствие старого и нового ID
                idMapping[oldId] = newId;

                // Если нода — это вложенная группа, вызываем CopyGroup рекурсивно
                if (newNode is BaseGroupNode childGroupNode)
                {
                    CopyGroup(childGroupNode);
                }

                // Отсоединяем от старых связей
                newNode.childrenID = new List<int>();

                // Добавляем новую ноду в AssetDatabase
                AssetDatabase.AddObjectToAsset(newNode, _templateNode.storyGraph);
                Undo.RegisterCreatedObjectUndo(newNode, "Story Graph (Copied Node)");

                newNodes.Add(newNode);
            }

            // Восстанавливаем связи между скопированными нодами
            foreach (StoryNode newNode in newNodes)
            {
                StoryNode originalNode = originalNodes.FirstOrDefault(node => node.id == idMapping.FirstOrDefault(pair => pair.Value == newNode.id).Key);

                if (originalNode != null)
                {
                    foreach (int oldChildId in originalNode.childrenID)
                    {
                        if (idMapping.TryGetValue(oldChildId, out int newChildId))
                        {
                            newNode.childrenID.Add(newChildId); // Устанавливаем связи с новыми нодами
                        }
                    }
                }
            }

            // Заменяем старые ноды в группе новыми
            groupNode.storyNodes.Clear();
            groupNode.storyNodes.AddRange(newNodes);

            Undo.RecordObject(_templateNode.storyGraph, "Story Graph (Copied Node)");
        }
    }
}
