//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="AuthorityAttribute.cs">
//      所属项目：RemoteService
//      创 建 人：王跃
//      创建日期：2012-10-19 9:59:57
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace RemoteService {
    using System;
    using System.Data;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 自定义权限过滤特性
    /// </summary>
    public class AuthorityAttribute : NDWR.Attributes.CustomAttribute{

        public string Name { get; set; }
    }
}
