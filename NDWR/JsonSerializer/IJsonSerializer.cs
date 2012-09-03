//-----------------------------------------------------------------------------------------
//   <copyright  file="IJsonSerializer.cs">
//      所属项目：NDWR.JsonSerializer
//      创 建 人：高勇
//      创建日期：2012-7-25 13:41:27
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.JsonSerializer {
    using System;

    /// <summary>
    /// IJsonSerializer 概要
    /// </summary>
    public interface IJsonSerializer {
        /// <summary>
        /// 实体序列化
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        string Serializer(object t);
        /// <summary>
        /// 实体反序列化
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        object Deserialize(string jsonString, Type type);
    }

}
