using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SilentMike.EnumService
{
    public class EnumService
    {

        /// <summary>
        /// Returns description attribute value
        /// </summary>
        /// <param name="value">Enum value</param>
        public static string GetDescription(Enum value)
        {
            var description = value.ToString();
            FieldInfo fieldInfo = value.GetType().GetField(description);
            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);

            if (attributes.Length <= 0) return description;
            if (attributes[0] is DescriptionAttribute descriptionAttribute)
                description = descriptionAttribute.Description;
            return description;
        }

        /// <summary>
        /// Returns list of key value pairs where key is enum value and value is description attribute
        /// </summary>
        /// <typeparam name="T">Enum type. Can be nullable</typeparam>
        public static List<KeyValuePair<T, string>> GetListOfValuesAndDescriptions<T>()
        {
            var type = typeof(T);
            if (Nullable.GetUnderlyingType(type) != null)
            {
                type = Nullable.GetUnderlyingType(type);
            }
            var values = Enum.GetValues(type);

            return (from T value in values select new KeyValuePair<T, string>(value, GetDescription(value as Enum))).ToList();
        }
    }
}
