using PG.Localization;
using System.Collections;
using TMPro;
using UnityEngine;

namespace PG.StorySystem.Nodes
{
    public class SetTextOnObjectNode : ActionNode
    {
        private TMP_Text _textObject;
        [SerializeField] private bool _useLocalization;
        public string textKey;
        [TextArea(3,10)] public string text;
        [SerializeField] private bool _isPrintText;
        [SerializeField] private float _printTime = 0.01f;

        private LocalizeText _localizedText;
        protected override bool useUpdate => true;

        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (_textObject != null)
            {
                _textObject.text = _useLocalization ? LocalizationSystem.instance.GetLocalizedValue(textKey, text) : text;
            }
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _textObject);

            _textObject.TryGetComponent(out _localizedText);
        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (!_isPrintText)
            {
                if (LocalizationSystem.instance != null && _useLocalization)
                {

                    if (_localizedText != null)
                    {
                        _localizedText.key = textKey;
                        _localizedText.Localize();
                    }
                    else
                    {
                        _textObject.text = _useLocalization ? LocalizationSystem.instance.GetLocalizedValue(textKey, text) : text; ;
                    }
                }
                else
                {
                    _textObject.text = text;
                }
                TransitionToNextNodes(storyGraph);
            }
        }
        protected override IEnumerator OnUpdate(StoryGraph storyGraph)
        {
            if (!_isPrintText)
            {
                yield break;
            }
            string printedText;
            string newText = "";


            if (_useLocalization && LocalizationSystem.instance != null)
            {
                if (_localizedText != null)
                {
                    _localizedText.key = textKey;
                    _localizedText.Localize();
                }
                else
                {
                    newText = LocalizationSystem.instance.GetLocalizedValue(textKey, text);
                }
            }
            else
            {
                newText = text;
            }


                printedText = "";
            for (int i = 0; i < newText.Length; i++)
            {
                printedText += newText[i];
                _textObject.text = printedText;
                yield return new WaitForSeconds(_printTime);
            }
            _textObject.text = newText;
            TransitionToNextNodes(storyGraph);
        }



    }
}
