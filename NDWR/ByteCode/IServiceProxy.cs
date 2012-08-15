//-----------------------------------------------------------------------------------------
//   <copyright  file="InvocationMethod.cs">
//      所属项目：NDWR.ByteCode
//      创 建 人：王跃
//      创建日期：2012-7-24 15:05:26
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.ByteCode {

    /// <summary>
    /// 目标服务的代理类接口
    /// </summary>
    public interface IServiceProxy {
        IServiceProxy Instance { get; }
        object FuncSwitch(string methodName, object[] paramlst);
    }
}
