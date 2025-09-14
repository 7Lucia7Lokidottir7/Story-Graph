using System.Reflection;

public static class AttributeHelper
{
    public static FieldInfo GetFieldWithNodeTitleAttribute(object target)
    {
        var type = target.GetType();
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<NodeDataTitleAttribute>() != null)
                return field;
        }
        return null;
    }
}
