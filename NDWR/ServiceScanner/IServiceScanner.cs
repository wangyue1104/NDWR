//-----------------------------------------------------------------------------------------
//   <copyright  file="IServiceScanner.cs">
//      所属项目：NDWR.ServiceScanner
//      创 建 人：王跃
//      创建日期：2012-7-25 14:29:47
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.ServiceScanner {
    using NDWR.ServiceStruct;

    /// <summary>
    /// IServiceScanner 概要
    /// </summary>
    public interface IServiceScanner {

        /// <summary>
        /// 获取服务元数据信息列表
        /// </summary>
        Service[] Services { get; }
        /// <summary>
        /// 获取服务所属程序集名称
        /// </summary>
        string[] Azzembly { get; }

        Service GetService(string serviceName);

        ServiceMethod GetMethod(string serviceName, string methodName);
    }
}
