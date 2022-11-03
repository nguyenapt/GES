using System;
using System.ComponentModel;
using System.Reflection;

namespace GES.Common.Extensions
{
    public static class ObjectExtension
    {
        public static void SetValueByPropertyName(this object obj, object value, string propertyName)
        {
            var propertyInfo = obj?.GetType().GetProperty(propertyName);
            propertyInfo?.SetValue(obj, value);
        }

        public static object GetValueByPropertyName(this object obj, string propertyName)
        {
            var propertyInfo = obj?.GetType().GetProperty(propertyName);
            return propertyInfo?.GetValue(obj);
        }

        public static string GetEnumDescription(this Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

    }
}
