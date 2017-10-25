using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace BACKnetLutron.BusinessEntities.Common_Constant
{
    public enum LightSceneEnum
    {
        [Description("Off")]
        Off = 1,
        [Description("Full$20On")]
        Full20On = 2,
        [Description("High")]
        High = 3,
        [Description("Medium")]
        Medium = 4,
        [Description("Low")]
        Low = 5,
        [Description("High$201")]
        High201 = 6,
        [Description("VTC")]
        VTC = 7,
        [Description("Low$201")]
        Low201 = 8,
        [Description("Unknown")]
        Unknown = 9
    }
    public class EnumConstants
    {
        public static string GetEnumDescription<T>(T enumValue)
        {
            return ((DescriptionAttribute)Attribute.GetCustomAttribute(typeof(T).GetField(Enum.GetName(typeof(T), enumValue)), typeof(DescriptionAttribute))).Description;
        }

        public static List<string> GetEnumValues<T>()
        {
            List<string> enumList = new List<string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                enumList.Add(GetEnumDescription<T>((T)item));
            }
            return enumList;
        }

        public static T GetValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)

                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            //throw new ArgumentException("Not found.", "description");
            return default(T);
        }


        public static int GetEnumValueFromDescription<T>(string description)
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) throw new InvalidOperationException();
            foreach (var field in enumType.GetFields())
            {
                DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null)
                    continue;
                if (attribute.Description == description)
                {
                    return (int)field.GetValue(null);
                }
            }
            return 0;
           
        }


        //public static List<SceneEntity> GetEnumList<T>()
        //{
        //    var list = new List<SceneEntity>();
        //    foreach (var e in Enum.GetValues(typeof(T)))
        //    {
        //        list.Add(new SceneEntity { SceneId = (int)e, SceneName = e.ToString() });
        //    }
        //    return list;
        //}
    }
}