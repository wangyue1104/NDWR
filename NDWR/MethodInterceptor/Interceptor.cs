//-----------------------------------------------------------------------------------------
//   <copyright  file="Interceptor.cs">
//      所属项目：NDWR.Interceptor
//      创 建 人：王跃
//      创建日期：2012-7-25 18:59:19
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.MethodInterceptor {

    /// <summary>
    /// Interceptor 概要
    /// </summary>
    public interface Interceptor {

        void Init();
        object Intercept(MethodInvocation methodInvoke);
        void Destroy();
    }
}
