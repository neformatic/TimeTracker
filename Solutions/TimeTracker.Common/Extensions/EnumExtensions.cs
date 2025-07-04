using System.ComponentModel;

namespace TimeTracker.Common.Extensions;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T enumValue) where T : Enum, IConvertible
    {
        if (!typeof(T).IsEnum)
        {
            return null;
        }

        var description = enumValue.ToString();
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        if (fieldInfo != null)
        {
            var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (attrs.Any())
            {
                description = ((DescriptionAttribute)attrs.First()).Description;
            }
        }

        return description;
    }
}