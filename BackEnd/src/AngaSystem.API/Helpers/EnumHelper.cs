using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace AngaSystem.API.Helpers
{
    public static class EnumHelper
    {
        public static string Description<T>(this T pValue)
        {
            var field = pValue.GetType().GetField(pValue.ToString());

            var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault();

            if (attribute != null)
                return ((DescriptionAttribute)attribute).Description;

            return pValue.ToString();
        }

        public static EnumModel<TValue> GetEnumModel<TEnum, TValue>(TEnum pEnumValue)
        {
            if (pEnumValue == null)
                return null;

            var model = new EnumModel<TValue>
            {
                Nome = pEnumValue.ToString(),
                Valor = (TValue)Convert.ChangeType((TEnum)pEnumValue, typeof(TValue)),
                Descricao = pEnumValue.Description<TEnum>()
            };

            return model;
        }

        public static IEnumerable<EnumModel<TValue>> ToList<TEnum, TValue>()
        {
            foreach (var item in Enum.GetValues(typeof(TEnum)))
            {
                yield return GetEnumModel<TEnum, TValue>((TEnum)item);
            }
        }

        public static T ToEnum<T>(this string pValue)
        {
            var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (!String.IsNullOrEmpty(pValue) && Enum.IsDefined(type, pValue))
            {
                return (T)Enum.Parse(type, pValue);
            }

            return default(T);
        }

        public static T ToEnum<T>(this EnumModel<int> pValue)
        {
            if (pValue == null)
                return default(T);

            return pValue.Nome.ToEnum<T>();
        }

        public static T ToEnum<T>(this EnumModel<char> pValue)
        {
            if (pValue == null)
                return default(T);

            return pValue.Nome.ToEnum<T>();
        }

        public static string GetXmlEnumAttributeValueFromEnum<T>(this T pValue)
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) return null;

            var member = enumType.GetMember(pValue.ToString()).FirstOrDefault();
            if (member == null) return null;

            var attribute = member.GetCustomAttributes(false).OfType<XmlEnumAttribute>().FirstOrDefault();
            if (attribute == null) return null;
            return attribute.Name;
        }
    }
    public class EnumModel<TValue>
    {
        public string Descricao { get; set; }
        public string Nome { get; set; }
        public TValue Valor { get; set; }
    }
}
