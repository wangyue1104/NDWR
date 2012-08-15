//-----------------------------------------------------------------------------------------
//   <copyright  file="AjaxService.cs">
//      所属项目：NDWR.Attributes
//      创 建 人：王跃
//      创建日期：2012-7-24 14:50:55
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Attributes {
    using System;

    /// <summary>
    /// RemoteServiceAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RemoteServiceAttribute : Attribute {

        private string name;
        private bool singleton = false;

        /// <summary>
        /// 服务名称
        /// 处于安全考虑，不支持命名空间
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 是否单例
        /// </summary>
        public bool Singleton {
            get { return singleton; }
            set { singleton = value; }
        }
    }
}
