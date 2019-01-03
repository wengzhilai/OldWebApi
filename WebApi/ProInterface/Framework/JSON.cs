using System;
using System.Collections.Generic;
using System.Web;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;

namespace ProInterface
{
    /// <summary>
    /// json操作
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// 转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T EncodeToEntity<T>(string jsonStr)
        {
            if (jsonStr == null) return default(T);
            T ent = JsonConvert.DeserializeObject<T>(jsonStr);
            return ent;
        }
        /// <summary>
        /// 对象转换成字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string DecodeToStr<T>(T entity)
        {
            if (entity == null) return null;

            if (entity == null) return null;
            if ((entity.GetType() == typeof(String) || entity.GetType() == typeof(string)))
            {
                return entity.ToString();
            }
            string DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            IsoDateTimeConverter dt = new IsoDateTimeConverter();
            dt.DateTimeFormat = DateTimeFormat;
            var jSetting = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            jSetting.Converters.Add(dt);
            return JsonConvert.SerializeObject(entity, jSetting);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string DecodeToStrNoRec<T>(T entity)
        {
            if (entity == null) return null;

            if (entity == null) return null;
            if ((entity.GetType() == typeof(String) || entity.GetType() == typeof(string)))
            {
                return entity.ToString();
            }
            string DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            IsoDateTimeConverter dt = new IsoDateTimeConverter();
            dt.DateTimeFormat = DateTimeFormat;
            var jSetting = new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling= DefaultValueHandling.Ignore,
                ObjectCreationHandling= ObjectCreationHandling.Reuse,
                TypeNameHandling= TypeNameHandling.None,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            jSetting.Converters.Add(dt);
            return JsonConvert.SerializeObject(entity, jSetting);
        }

        public static string DecodeToStr(int allNum, object o)
        {
            string json = DecodeToStr(o);
            if (json == null || json == "") json = "[]";
            return "{\"total\":" + allNum + ",\"rows\":" + json + "}";
        }

    }
}