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
    public class DownloadInterceptor : Interceptor {
        public void Init() {
        }

        public void Intercept(Invocation methodInvoke) {
            // 执行目标方法
            methodInvoke.Invoke();
            // 如果输出流方式或包含输出流方式
            if (methodInvoke.MethodMetaData.OutputType == TypeCategory.BinaryType) {
                TransferFile tfile = ((TransferFile)methodInvoke.RetValue);
                NDWR.Util.Kit.SetAbsCache(tfile.ID, tfile);
                methodInvoke.RetValue = tfile.ID;
            }

        }

        public void Destroy() {
        }
    }
}
