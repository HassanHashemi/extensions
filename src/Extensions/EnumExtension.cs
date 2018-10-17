using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MySolution.Base.Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<EnumItem> GetItems<TEnumType>() where TEnumType : Enum
        {
            var enumType = typeof(TEnumType);
            var values = (TEnumType[])Enum.GetValues(enumType);

            return values.Select(item => new EnumItem()
            {
                Description = GetEnumDescription(item),
                Value = (int)Enum.Parse(enumType, item.ToString())
            });
        }


        public static string GetEnumDescription(this Enum enumValue)
        {
            FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(
                    typeof (DescriptionAttribute),
                    false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return enumValue.ToString();
            }
        }
    }
}