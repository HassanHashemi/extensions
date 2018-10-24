using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Extensions
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

        public static TAttributeType GetEnumAttribute<TAttributeType>(this Type enumType, string memberName)
        {
            var memInfo = enumType.GetMember(memberName);
            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttributeType), false);

            return (TAttributeType)attributes[0];
        }

        public static string GetEnumDescription(this Enum enumValue)
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());
            var attributes =(DescriptionAttribute[]) fi.GetCustomAttributes(
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