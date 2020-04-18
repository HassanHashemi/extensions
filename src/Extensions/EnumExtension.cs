using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static IEnumerable<EnumItem> GetItems<TEnumType, TDescriptionAttrType>()
            where TEnumType : Enum
            where TDescriptionAttrType : DescriptionAttribute
        {
            var enumType = typeof(TEnumType);
            var values = (TEnumType[])Enum.GetValues(enumType);

            return values.Select(item => new EnumItem()
            {
                Description = GetEnumDescription<TDescriptionAttrType>(item),
                Value = (int)Enum.Parse(enumType, item.ToString())
            });
        }

        public static IEnumerable<EnumItem> GetItems<TEnumType>() where TEnumType : Enum
        {
            return GetItems<TEnumType, DescriptionAttribute>();
        }

        public static TAttributeType GetEnumAttribute<TAttributeType>(this Type enumType, string memberName)
        {
            var memInfo = enumType.GetMember(memberName);
            var attributes = memInfo[0].GetCustomAttributes(typeof(TAttributeType), false);

            return (TAttributeType)attributes[0];
        }

        public static TAttr GetAttributeValue<TAttr>(this Enum enumValue) where TAttr : Attribute
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (TAttr[])fi.GetCustomAttributes(
                    typeof(TAttr),
                    false);

            return attributes.FirstOrDefault();
        }

        public static string GetEnumDescription(this Enum enumValue)
        {
            return GetEnumDescription<DescriptionAttribute>(enumValue);
        }

        public static string GetEnumDescription<TDescriptionAttrType>(this Enum enumValue)
            where TDescriptionAttrType : DescriptionAttribute
        {
            var fi = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (TDescriptionAttrType[])fi.GetCustomAttributes(
                    typeof(TDescriptionAttrType),
                    false);

            if (attributes?.Length > 0)
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