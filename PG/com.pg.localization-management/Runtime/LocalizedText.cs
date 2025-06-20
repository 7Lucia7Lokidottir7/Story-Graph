using TMPro;
using UnityEngine;

namespace PG.LocalizationManagement
{
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textObject;
        private string _standardText;
        private void Awake()
        {
            _standardText = _textObject.text;
        }
        [ContextMenu("Get Cache")]
        private void GetCache() => TryGetComponent(out _textObject);
        void SetStandardText(string text)
        {
            _standardText = text;
            _textObject.text = CSVLocalizationManager.Instance.GetLocalizedValue(_standardText);
        }
        private void Start()
        {
            _textObject.text = CSVLocalizationManager.Instance.GetLocalizedValue(_standardText);
        }
    }
}
