using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;

namespace PG.StorySystem.NodesEditor
{
    using Nodes;
    using System.Collections;
    using System.Linq;
    using System.Reflection;
    using UnityEditor.UIElements;
    using UnityEngine;

    [CustomEditor(typeof(StoryNode), true)]
    public class StoryNodeEditor : Editor
    {
        private StoryNode _storyNode;
        protected virtual bool _useDefaultInspector => true;
        private void OnEnable()
        {
            _storyNode = target as StoryNode;
            Init();
        }
        protected virtual void Init()
        {

        }
        public override void OnInspectorGUI()
        {
            GUILayout.Label($"Node ID: {_storyNode.id}");
            if (Application.isPlaying)
            {
                GUILayout.Label($"Node is started: {_storyNode.isStarted}");
                GUILayout.Label($"Node is Ended: {_storyNode.isEnded}");
            }
            base.OnInspectorGUI();
        }
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new VisualElement();


            Toggle visibleInTemplate = new Toggle("Visible In Template");
            visibleInTemplate.value = _storyNode.isVisibleInTemplate;
            visibleInTemplate.RegisterValueChangedCallback(evt => _storyNode.isVisibleInTemplate = evt.newValue);
            visibleInTemplate.style.paddingBottom = 10;
            visibleInTemplate.style.paddingTop = 10;
            root.Add(visibleInTemplate);



            if (_useDefaultInspector)
            {

                root.Add(new Label("Description"));
                TextField descriptionField = new TextField();
                descriptionField.value = _storyNode.description;
                descriptionField.RegisterValueChangedCallback(evt => _storyNode.description = evt.newValue);
                descriptionField.style.whiteSpace = WhiteSpace.Normal;
                descriptionField.style.flexGrow = 1;
                descriptionField.multiline = true;
                root.Add(descriptionField);

                InspectorElement.FillDefaultInspector(root, serializedObject, this);
            }
            OnCustomElement(root);



            if (StoryGraphEditorWindow.isDebug)
            {
                // 1) Корневой контейнер с отступами
                var debugRoot = new ScrollView
                {
                    style = {
                paddingLeft = 4,
                paddingRight = 4,
                paddingTop = 4,
                paddingBottom = 4,
            }
                };

                var debugFoldout = new Foldout { text = "Debug", value = true };
                debugFoldout.Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;

                debugFoldout.style.backgroundColor = new Color(0.25f, 0.25f, 0.25f);
                debugFoldout.style.borderBottomLeftRadius = 5f;
                debugFoldout.style.borderBottomRightRadius = 5f;
                debugFoldout.style.borderTopLeftRadius = 5f;
                debugFoldout.style.borderTopRightRadius = 5f;


                debugRoot.Add(debugFoldout);

                debugFoldout.Add(new IMGUIContainer(() =>
                {
                    if (target != null)
                    {
                        DrawDefaultInspector();
                    }
                }));

                if (target != null)
                {
                    var publicFoldout = new Foldout { text = "Public Fields", value = true };
                    var nonPublicFoldout = new Foldout { text = "Non-Public Fields", value = true };
                    publicFoldout.Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;
                    nonPublicFoldout.Q<Label>().style.unityFontStyleAndWeight = FontStyle.Bold;
                    debugFoldout.Add(publicFoldout);
                    debugFoldout.Add(nonPublicFoldout);

                    // 1) Собираем ВСЕ поля из класса и базовых типов
                    var obj = target;
                    var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                    var rawFields = new List<FieldInfo>();
                    for (var t = obj.GetType(); t != null && t != typeof(UnityEngine.Object); t = t.BaseType)
                        rawFields.AddRange(t.GetFields(flags)
                                             .Where(f => !f.IsStatic));

                    // 2) Убираем дубликаты по имени (берём первое вхождение – из самого наследника)
                    var uniqueFields = rawFields
                        .GroupBy(fi => fi.Name)
                        .Select(g => g.First())
                        .ToList();

                    // 3) Разделяем на публичные и непубличные
                    var pubFields = uniqueFields.Where(f => f.IsPublic).ToList();
                    var nonPubFields = uniqueFields.Where(f => !f.IsPublic).ToList();

                    // 4) Вспомогательный метод для группировки по типу и отображения
                    void AddGroupedFields(List<FieldInfo> list, Foldout foldout)
                    {
                        var groups = list
                            .GroupBy(fi => fi.FieldType.Name)
                            .OrderBy(g => g.Key);

                        foreach (var group in groups)
                        {
                            // Заголовок группы
                            var typeHeader = new Label(group.Key)
                            {
                                style = {
                        unityFontStyleAndWeight = FontStyle.Bold,
                        marginLeft = 8,
                        marginTop  = 8
                    }
                            };
                            foldout.Add(typeHeader);

                            foreach (var fi in group.OrderBy(f => f.Name))
                            {
                                var fieldType = fi.FieldType;
                                var val = fi.GetValue(obj);

                                // UnityEngine.Object → ObjectField
                                if (typeof(UnityEngine.Object).IsAssignableFrom(fieldType))
                                {
                                    var of = new ObjectField(fi.Name)
                                    {
                                        objectType = fieldType,
                                        value = val as UnityEngine.Object
                                    };
                                    of.SetEnabled(false);
                                    of.style.marginLeft = 16;
                                    foldout.Add(of);
                                }
                                // IList (List<> или Array) → Foldout со ссылками/значениями
                                else if (val is IList listValue)
                                {
                                    var listFold = new Foldout
                                    {
                                        text = $"{fi.Name} ({fieldType.Name}) [{listValue.Count}]",
                                        value = false,
                                        style = { marginLeft = 16 }
                                    };
                                    // тип элементов
                                    var elemType = fieldType.IsArray
                                        ? fieldType.GetElementType()
                                        : fieldType.GetGenericArguments().FirstOrDefault() ?? typeof(object);

                                    for (int i = 0; i < listValue.Count; i++)
                                    {
                                        var elem = listValue[i];
                                        if (elem is UnityEngine.Object ue)
                                        {
                                            var elemField = new ObjectField($"[{i}]")
                                            {
                                                objectType = elemType,
                                                value = ue
                                            };
                                            elemField.SetEnabled(false);
                                            elemField.style.marginLeft = 12;
                                            listFold.Add(elemField);
                                        }
                                        else
                                        {
                                            var lbl = new Label($"[{i}] = {elem}")
                                            {
                                                style = {
                                        marginLeft   = 12,
                                        marginBottom = 2
                                    }
                                            };
                                            listFold.Add(lbl);
                                        }
                                    }

                                    foldout.Add(listFold);
                                }
                                // Всё остальное → Label
                                else
                                {
                                    var lbl = new Label($"{fi.Name} = {val}")
                                    {
                                        style = {
                                marginLeft   = 16,
                                marginBottom = 2
                            }
                                    };
                                    foldout.Add(lbl);
                                }
                            }
                        }
                    }

                    AddGroupedFields(pubFields, publicFoldout);
                    AddGroupedFields(nonPubFields, nonPublicFoldout);

                }
                root.Add(debugRoot);
            }

            return root;
        }
        public virtual void OnCustomElement(VisualElement root)
        {

        }
        public List<string> objects => StoryGraphEditorWindow.storyGraph.objects;
        public List<string> variables => StoryGraphEditorWindow.storyGraph.variables.Select(v => v.variableName).ToList();
        public void ObjectsPopup(ref int data, StoryNode scenarioNode)
        {
            data = EditorGUILayout.Popup(data, objects.ToArray());
        }
    }

}
