using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PG.LocalizationManagement;
namespace PG.StorySystem.Nodes
{
    public class AnswerSelectorNode : ActionNode
    {
        private AnswerSelector _answerSelector;
        [SerializeField] private bool _useLocalization;
        [TextArea(3, 10)] public string textAnswer1;
        public string textKeyAnswer1;
        [NonReorderable]public List<Answer> answers = new List<Answer>(1);
        private int _answerIndex;

        [System.Serializable]
        public class Answer
        {
            [TextArea(3,10)] public string textKey;
            [TextArea(3,10)] public string text;
            [HideInInspector] 
            public List<int> childrenID = new List<int>();
        }
        protected override void Init(StoryGraph storyGraph)
        {
            base.Init(storyGraph);
            storyGraph.GetObject(objectNameID, out _answerSelector);
        }
        protected override void OnEnd(StoryGraph storyGraph)
        {
            _answerSelector.ClearContainer();
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            // Добавляем дефолтный ответ (без ветвления)
            System.Action defaultAction = () => {
                _answerIndex = 0;
                OnTransitionToNextNode(storyGraph);
            };

            _answerSelector.AddAnswer(
                _useLocalization ?
                CSVLocalizationManager.Instance.GetLocalizedValue(textKeyAnswer1, textAnswer1) : textAnswer1,
                defaultAction);

            // Добавляем дополнительные ответы из списка answers
            if (_useLocalization)
            {
                for (int i = 0; i < answers.Count; i++)
                {
                    int index = i; // корректное захватывание переменной для замыкания
                    System.Action branchAction = () => {
                        _answerIndex = index + 1;
                        OnTransitionToNextNode(storyGraph);
                    };
                    _answerSelector.AddAnswer(
                        CSVLocalizationManager.Instance.GetLocalizedValue(answers[i].textKey, answers[i].text),
                        branchAction);
                }
            }
            else
            {
                for (int i = 0; i < answers.Count; i++)
                {
                    int index = i; // корректное захватывание переменной для замыкания
                    System.Action branchAction = () => {
                        _answerIndex = index + 1;
                        OnTransitionToNextNode(storyGraph);
                    };
                    _answerSelector.AddAnswer(answers[i].text, branchAction);
                }
            }
        }

        public override void OnDublicateChildren(Dictionary<int, int> idMapping, StoryNode originalNode)
        {
            base.OnDublicateChildren(idMapping, originalNode);

            AnswerSelectorNode answerSelectorNode = originalNode as AnswerSelectorNode;
            if (answers.Count > 0)
            {
                for (int i = 0; i < answers.Count; i++)
                {
                    answers[i].childrenID.Clear();
                    foreach (int oldChildId in answerSelectorNode.answers[i].childrenID)
                    {
                        if (idMapping.TryGetValue(oldChildId, out int newChildId))
                        {
                            answers[i].childrenID.Add(newChildId); // Добавляем новую связь к скопированным нодам
                        }
                    }
                }
            }
        }
        protected override void OnUpdate(StoryGraph storyGraph)
        {
        }
        public override void OnNextNode(StoryGraph storyGraph)
        {
            if (_answerIndex == 0)
            {
                base.OnNextNode(storyGraph);
            }
            else
            {
                End(storyGraph);
                if (_groupNode != null)
                {

                    foreach (var child in answers[_answerIndex-1].childrenID)
                    {
                        StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);


                        if (!_groupNode.currentNodes.Contains(childInstance))
                        {
                            _groupNode.currentNodes.Add(childInstance);
                        }

                        childInstance.Initialize(storyGraph, _groupNode);  // Передаем null для groupNode
                    }
                }
                else
                {
                    foreach (var child in answers[_answerIndex-1].childrenID)
                    {
                        StoryNode childInstance = storyGraph.GetNodeByID(child, _groupNode);

                        if (!storyGraph.currentNodes.Contains(childInstance))
                        {
                            storyGraph.currentNodes.Add(childInstance);
                        }

                        childInstance.Initialize(storyGraph);  // Передаем null для groupNode
                    }
                }
            }
        }



    }
}
