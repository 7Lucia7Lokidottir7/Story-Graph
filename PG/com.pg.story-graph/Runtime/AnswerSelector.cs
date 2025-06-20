using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AnswerSelector : MonoBehaviour
{
    [SerializeField] private AssetReferenceGameObject _button;
    [SerializeField] private Transform _buttonsContainer;
    public void AddAnswer(string text, System.Action answerAction)
    {
        answerAction += ClearContainer;
        _button.InstantiateAsync(_buttonsContainer).Completed += (operation) =>
        {
            operation.Result.GetComponent<Button>().onClick.AddListener(answerAction.Invoke);
            operation.Result.GetComponentInChildren<TMP_Text>().text = text;
        };
    }
    public void ClearContainer()
    {
        if (_buttonsContainer.childCount > 0)
        {
            for (int i = _buttonsContainer.childCount-1; i >= 0; i--)
            {
                _button.ReleaseInstance(_buttonsContainer.GetChild(i).gameObject);
            }
        }
    }
}
