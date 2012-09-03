//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="Session.cs">
//      所属项目：NDWR.Util
//      创 建 人：王跃
//      创建日期：2012-8-23 18:40:20
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Util {
    using System;
    using System.Web;

    /// <summary>
    /// NDWR工具类
    /// 转移用户对Http系列类使用
    /// </summary>
    public class Kit {

        /// <summary>
        /// 获取session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetSession<T>(string key) {
            object obj = HttpContext.Current.Session[key];
            return obj == null ?
                default(T) :
                (T)obj;
        }
        /// <summary>
        /// 保存session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetSession<T>(string key, T value) {
            HttpContext.Current.Session[key] = value;
        }
        /// <summary>
        /// 获取cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetCache<T>(string key) {
            object obj = HttpContext.Current.Cache[key];
            return obj == null ?
                default(T) :
                (T)obj;
        }

        public static void RemoveCache(string key) {
            HttpContext.Current.Cache.Remove(key);
        }
        /// <summary>
        /// 保存cache
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetCache<T>(string key, T value) {
            HttpContext.Current.Cache[key] = value;
        }
        public static void SetAbsCache<T>(string key, T value) {
            HttpContext.Current.Cache.Insert(key, value, null, DateTime.Now.AddMinutes(10), TimeSpan.Zero);
        }

    }

}
