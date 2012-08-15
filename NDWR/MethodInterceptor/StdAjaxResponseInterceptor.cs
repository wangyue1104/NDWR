//-----------------------------------------------------------------------------------------
//   <copyright  file="StdAjaxResponseInterceptor.cs">
//      所属项目：NDWR.MethodInterceptor
//      创 建 人：王跃
//      创建日期：2012-8-3 13:49:46
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.MethodInterceptor {
    using NDWR.Web;
    using NDWR.ServiceStruct;

    /// <summary>
    /// StdAjaxResponseInterceptor 概要
    /// </summary>
    public class StdAjaxResponseInterceptor : Interceptor {
        public void Init() {
        }

        public object Intercept(MehtodInvocation methodInvoke) {
            if (methodInvoke.Method.MethodType == MethodType.BinaryStream) { // 如果方法是二进制流方式
                methodInvoke.InvokeInfo.Response = new FileDownloadResponse();
            } else {
                methodInvoke.InvokeInfo.Response = new AjaxRespose();
            }

            // 执行目标方法
            return methodInvoke.Invoke();
        }

        public void Destroy() {
        }
    }
}
