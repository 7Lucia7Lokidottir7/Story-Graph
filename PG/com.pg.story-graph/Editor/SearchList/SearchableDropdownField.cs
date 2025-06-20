using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.Experimental.GraphView; // Для SearchWindow и ISearchWindowProvider

/// <summary>
/// VisualElement, реализующий выпадающий список с поиском, стилизованный как стандартный DropdownField.
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
    /// Событие, вызываемое при выборе нового значения.
    /// </summary>
    public event Action<string> OnValueChanged;

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public SearchableDropdownField()
    {
        // Применяем стандартные USS классы, как у DropdownField
        AddToClassList("unity-base-field");
        AddToClassList("unity-base-field--no-label");
        AddToClassList("unity-base-popup-field");
        AddToClassList("unity-popup-field");

        VisualElement visualElement = new VisualElement();
        visualElement.AddToClassList("unity-base-popup-field__input");
        visualElement.AddToClassList("unity-base-field__input");
        visualElement.AddToClassList("unity-popup-field__input");
        Add(visualElement);

        // Элемент для отображения выбранного текста с классом для текста
        _textLabel = new Label
        {
            text = ""
        };
        _textLabel.AddToClassList("unity-base-popup-field__text");
        visualElement.Add(_textLabel);

        // Элемент-стрелка, имитирующий иконку выпадающего списка
        var arrowIcon = new VisualElement();
        arrowIcon.AddToClassList("unity-base-popup-field__arrow");
        arrowIcon.pickingMode = PickingMode.Ignore;
        visualElement.Add(arrowIcon);

        // Регистрируем обработку клика по всему элементу
        RegisterCallback<ClickEvent>(evt => OpenSearchWindow());
    }

    /// <summary>
    /// Конструктор с параметрами, позволяющий задать список опций, значение по умолчанию и название списка.
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
    /// Метод, открывающий окно поиска с заданным списком опций.
    /// </summary>
    private void OpenSearchWindow()
    {
        // Определяем позицию для окна поиска
        Vector2 screenPos = GUIUtility.GUIToScreenPoint(worldBound.position);
        SearchWindowContext context = new SearchWindowContext(screenPos);

        // Создаем и инициализируем провайдер поиска, передавая список опций и callback выбора
        var provider = ScriptableObject.CreateInstance<StringListSearchProvider>();
        provider.listName = listName;
        provider.Init(choices, OnItemSelected);

        // Открываем окно поиска
        SearchWindow.Open(context, provider);
    }

    /// <summary>
    /// Callback, вызываемый после выбора элемента в окне поиска.
    /// Обновляет отображаемый текст и вызывает событие.
    /// </summary>
    private void OnItemSelected(string selectedItem)
    {
        _selected = selectedItem;
        _textLabel.text = selectedItem;
        OnValueChanged?.Invoke(selectedItem);
    }

    /// <summary>
    /// Свойство доступа к выбранному значению.
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
