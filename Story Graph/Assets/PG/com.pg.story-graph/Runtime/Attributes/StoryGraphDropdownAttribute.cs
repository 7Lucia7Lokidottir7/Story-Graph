using UnityEngine;

public class StoryGraphDropdownAttribute : PropertyAttribute
{
    public string sourceListField;

    /// <param name="sourceListField">
    /// ��� ���� ��� �������� � ����� ����/���������, ������������� List<string> ��� ������.
    /// ��������: nameof(objects) ��� "objects" ��� "variables".
    /// </param>
    public StoryGraphDropdownAttribute(string sourceListField)
    {
        this.sourceListField = sourceListField;
    }
}
