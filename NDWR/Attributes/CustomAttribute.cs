//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="CustomAttribute.cs">
//      所属项目：NDWR.Attributes
//      创 建 人：王跃
//      创建日期：2012-10-18 16:11:04
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Attributes {
    using System;
    using System.Data;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// CustomAttribute 概要
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public abstract class CustomAttribute : Attribute{
    }
}
