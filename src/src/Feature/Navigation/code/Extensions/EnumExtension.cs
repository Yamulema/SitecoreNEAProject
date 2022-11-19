using System;
using Neambc.Seiumb.Feature.Navigation.Attributes;
using System.Reflection;

namespace Neambc.Seiumb.Feature.Navigation.Extensions
{
    public static class EnumExtension
    {
        public static string GetEnumStringValue(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            StringValueAttribute[] attribs =
                fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
}