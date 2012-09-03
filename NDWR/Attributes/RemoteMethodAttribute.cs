//-----------------------------------------------------------------------------------------
//   <copyright  file="RemoteMethodAttribute.cs">
//      所属项目：NDWR.Attributes
//      创 建 人：王跃
//      创建日期：2012-7-24 14:51:18
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Attributes {
    using System;

    /// <summary>
    /// RemoteMethodAttribute 概要
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RemoteMethodAttribute : Attribute {

        private string name;
        /// <summary>
        /// 方法名
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }
    }
}
