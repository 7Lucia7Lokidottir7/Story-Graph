using UnityEngine;

public class StoryGraphDropdownAttribute : PropertyAttribute
{
    public string sourceListField;

    /// <param name="sourceListField">
    /// »м€ пол€ или свойства в твоей ноде/редакторе, возвращающего List<string> дл€ выбора.
    /// Ќапример: nameof(objects) или "objects" или "variables".
    /// </param>
    public StoryGraphDropdownAttribute(string sourceListField)
    {
        this.sourceListField = sourceListField;
    }
}
