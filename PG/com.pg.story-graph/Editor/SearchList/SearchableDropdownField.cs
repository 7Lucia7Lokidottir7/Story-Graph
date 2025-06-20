using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView; // ��� SearchWindow � ISearchWindowProvider

/// <summary>
/// VisualElement, ����������� ���������� ������ � �������, ������������� ��� ����������� DropdownField.
/// </summary>

//For Unity 6 and later
#if UNITY_6000_0_OR_NEWER
[UxmlElement]
#endif
public partial class SearchableDropdownField : VisualElement
{
    //For Unity 2022 LTS and older
#if !UNITY_6000_0_OR_NEWER
    public new class UxmlFactory : UxmlFactory<SearchableDropdownField, UxmlTraits> { }
#endif
    public string listName;

    private Label _textLabel;
    public List<string> choices = new List<string>();
    private string _selected;

    /// <summary>
    /// �������, ���������� ��� ������ ������ ��������.
    /// </summary>
    public event Action<string> OnValueChanged;

    /// <summary>
    /// ����������� �� ���������.
    /// </summary>
    public SearchableDropdownField()
    {
        // ��������� ����������� USS ������, ��� � DropdownField
        AddToClassList("unity-base-field");
        AddToClassList("unity-base-field--no-label");
        AddToClassList("unity-base-popup-field");
        AddToClassList("unity-popup-field");

        VisualElement visualElement = new VisualElement();
        visualElement.AddToClassList("unity-base-popup-field__input");
        visualElement.AddToClassList("unity-base-field__input");
        visualElement.AddToClassList("unity-popup-field__input");
        Add(visualElement);

        // ������� ��� ����������� ���������� ������ � ������� ��� ������
        _textLabel = new Label
        {
            text = ""
        };
        _textLabel.AddToClassList("unity-base-popup-field__text");
        visualElement.Add(_textLabel);

        // �������-�������, ����������� ������ ����������� ������
        var arrowIcon = new VisualElement();
        arrowIcon.AddToClassList("unity-base-popup-field__arrow");
        arrowIcon.pickingMode = PickingMode.Ignore;
        visualElement.Add(arrowIcon);

        // ������������ ��������� ����� �� ����� ��������
        RegisterCallback<ClickEvent>(evt => OpenSearchWindow());
    }

    /// <summary>
    /// ����������� � �����������, ����������� ������ ������ �����, �������� �� ��������� � �������� ������.
    /// </summary>
    public SearchableDropdownField(List<string> options, string defaultValue = "", string defaultListName = "Select value")
        : this()
    {
        choices = options;
        _selected = defaultValue;
        listName = defaultListName;
        _textLabel.text = string.IsNullOrEmpty(defaultValue) ? "" : defaultValue;
    }

    /// <summary>
    /// �����, ����������� ���� ������ � �������� ������� �����.
    /// </summary>
    private void OpenSearchWindow()
    {
        // ���������� ������� ��� ���� ������
        Vector2 screenPos = GUIUtility.GUIToScreenPoint(worldBound.position);
        SearchWindowContext context = new SearchWindowContext(screenPos);

        // ������� � �������������� ��������� ������, ��������� ������ ����� � callback ������
        var provider = ScriptableObject.CreateInstance<StringListSearchProvider>();
        provider.listName = listName;
        provider.Init(choices, OnItemSelected);

        // ��������� ���� ������
        SearchWindow.Open(context, provider);
    }

    /// <summary>
    /// Callback, ���������� ����� ������ �������� � ���� ������.
    /// ��������� ������������ ����� � �������� �������.
    /// </summary>
    private void OnItemSelected(string selectedItem)
    {
        _selected = selectedItem;
        _textLabel.text = selectedItem;
        OnValueChanged?.Invoke(selectedItem);
    }

    /// <summary>
    /// �������� ������� � ���������� ��������.
    /// </summary>
    public string value
    {
        get => _selected;
        set
        {
            _selected = value;
            _textLabel.text = value;
        }
    }
}
