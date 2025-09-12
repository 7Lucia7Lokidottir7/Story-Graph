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
        private string _printedText;
        [SerializeField] private float _printTime = 0.01f;


        protected override void OnEnd(StoryGraph storyGraph)
        {
            if (_textObject != null)
            {
                _textObject.text = text;
            }
        }
        protected override void Init(StoryGraph storyGraph)
        {
            storyGraph.GetObject(objectNameID, out _textObject);
            for (int i = 0; i < storyGraph.variables.Count; i++)
            {
                string variableName = storyGraph.variables[i].variableName;
                string pattern = @"\[" + variableName + @"([+\-*/]\d+)?\]";
                var matches = System.Text.RegularExpressions.Regex.Matches(text, pattern);

                foreach (System.Text.RegularExpressions.Match match in matches)
                {
                    string expression = match.Value; // Например, "[VariableName+1]"
                    string operation = match.Groups[1].Value; // Например, "+1"

                    // Получаем значение переменной
                    object variableValue = storyGraph.GetVariableValue(variableName);

                    double result = 0;

                    // Проверка типов переменной
                    if (variableValue is int intValue)
                    {
                        result = intValue;
                    }
                    else if (variableValue is float floatValue)
                    {
                        result = floatValue;
                    }
                    else
                    {
                        Debug.LogError($"Variable '{variableName}' is not a numeric type.");
                        continue;
                    }

                    // Выполняем операцию, если указана
                    if (!string.IsNullOrEmpty(operation))
                    {
                        char op = operation[0];
                        double number = double.Parse(operation.Substring(1));

                        switch (op)
                        {
                            case '+': result += number; break;
                            case '-': result -= number; break;
                            case '*': result *= number; break;
                            case '/': result /= number; break;
                        }
                    }

                    // Заменяем в тексте
                    text = text.Replace(expression, result.ToString());
                }
            }


        }

        protected override void OnStart(StoryGraph storyGraph)
        {
            if (_isPrintText)
            {
                storyGraph.runner.StartCoroutine(PrintText(storyGraph));
            }
            else
            {
                _textObject.text = text;
                TransitionToNextNodes(storyGraph);
            }
        }
        IEnumerator PrintText(StoryGraph storyGraph)
        {
            string newText = "";

            newText = text;
            _printedText = "";
            for (int i = 0; i < newText.Length; i++)
            {
                _printedText += newText[i];
                _textObject.text = _printedText;
                yield return new WaitForSeconds(_printTime);
            }
            _textObject.text = newText;
            TransitionToNextNodes(storyGraph);
        }



    }
}
