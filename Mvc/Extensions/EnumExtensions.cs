using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Mvc.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var type = enumValue.GetType();
            var field = type.GetField(enumValue.ToString());
            var attr = field == null
                ? null
                : field.GetCustomAttribute<DisplayAttribute>();
            return attr != null
                ? attr.Name
                : enumValue.ToString();
        }
    }
}
