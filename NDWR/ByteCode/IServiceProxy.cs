//-----------------------------------------------------------------------------------------
//   <copyright  file="IServiceProxy.cs">
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
        /// <summary>
        /// 返回实现类的实例
        /// </summary>
        IServiceProxy Instance { get; }
        /// <summary>
        /// 执行目标标识的方法
        /// </summary>
        /// <param name="methodId">标识ID</param>
        /// <param name="paramlst">方法参数</param>
        /// <returns></returns>
        object FuncSwitch(int methodId, object[] paramlst);
    }
}
