using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
public class HighlightObjectController : MonoBehaviour
{
    public bool isEnable { 
        get => _isEnable; 
        set { 
            _isEnable = value;
            if (!_isEnable)
            {
                BackBaseHighlightColor();
                isSelected = false;
                selected?.Invoke(false);
            }
        } 
    }
    [SerializeField] private bool _isEnable = true;

    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private string _highlightParameter = "_Highlight";
    [SerializeField] private float _highlightValue = 1f;
    [SerializeField] private float _defaultHighlightValue = 0f;
    [SerializeField] private float _targetDuration = 0.5f;

    [SerializeField] private string _colorParameter = "_Highlight_Color";
    public Color baseHighlightColor { get; private set; }

    [SerializeField] private bool _isSelectEnable;
    [HideInInspector] public bool isSelected;
    public event System.Action<bool> selected;
    private void Awake()
    {
        TryGetComponent(out XRBaseInteractable interactable);
        interactable.hoverEntered.AddListener(args => SetHighlightActive());
        interactable.hoverExited.AddListener(args => SetHighlightInactive());
        interactable.selectEntered.AddListener(args => OnChangeSelect());
        baseHighlightColor = _renderers[0].materials[0].GetColor(_colorParameter);
    }
    public void ChangeHighlightColor(Color color)
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            for (int a = 0; a < _renderers[i].materials.Length; a++)
            {
                _renderers[i].materials[a].SetColor(_colorParameter, color);
            }
        }
    }
    public void BackBaseHighlightColor()
    {
        for (int i = 0; i < _renderers.Length; i++)
        {
            for (int a = 0; a < _renderers[i].materials.Length; a++)
            {
                _renderers[i].materials[a].SetColor(_colorParameter, baseHighlightColor);
            }
        }
    }
    public void SetHighlightActive()
    {
        if (!isEnable)
            return;

        SetHighlight(_highlightValue);
    }
    public void OnChangeSelect()
    {
        if (!isEnable || !_isSelectEnable)
        {
            return;
        }
        isSelected = !isSelected;
        selected?.Invoke(isSelected);
        if (!isSelected)
        {
            SetHighlight(_defaultHighlightValue);
        }
    }
    public void SetHighlightInactive()
    {
        if (!isEnable || (isSelected && _isSelectEnable))
            return;

        SetHighlight(_defaultHighlightValue);
    }
    public async void SetHighlight(float value)
    {
        for (float s = 0; s < _targetDuration; s+= Time.deltaTime)
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                for (int a = 0; a < _renderers[i].materials.Length; a++)
                {
                    _renderers[i].materials[a].SetFloat(_highlightParameter, Mathf.Lerp(_renderers[i].materials[a].GetFloat(_highlightParameter), value, s / _targetDuration));
                }
            }
            await Task.Yield();
        }
    }
}
