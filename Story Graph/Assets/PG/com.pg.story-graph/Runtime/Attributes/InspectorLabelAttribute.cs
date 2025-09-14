using UnityEngine;

public class InspectorLabelAttribute : PropertyAttribute
{
    public string label;
    public InspectorLabelAttribute(string label)
    {
        this.label = label;
    }
}