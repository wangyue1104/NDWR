//-----------------------------------------------------------------------------------------
//   <copyright  file="MehtodInvocation.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-7-27 14:21:51
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------
using System.Collections.Generic;
using NDWR.ByteCode;
using NDWR.Config;
using NDWR.MethodInterceptor;
using NDWR.ServiceStruct;

namespace NDWR {

    /// <summary>
    /// 单个目标方法执行
    /// 该类只能用与一次调用
    /// </summary>
    public class MethodInvocation {
        private static readonly NDWR.Logging.ILog log = NDWR.Logging.LogManager.GetLogger(typeof(MethodInvocation));

        private int i = 0; // 拦截器递归调用计数器
        private IList<Interceptor> interceptorList = GlobalConfig.Instance.Interceptors; // 获取拦截器集合

        private Invocation invokeInfo;

        /// <summary>
        /// 当前执行信息
        /// </summary>
        public Invocation InvokeInfo {
            get { return invokeInfo; }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="invokeInfo"></param>
        /// <param name="method"></param>
        public MethodInvocation(Invocation invokeInfo) {
            this.invokeInfo = invokeInfo;
        }

        /// <summary>
        /// 执行目标方法
        /// 递归执行拦截器，最会执行目标的代理方法
        /// </summary>
        /// <returns></returns>
        public object Invoke() {
            if (i >= interceptorList.Count) { // 如果拦截器已经执行完，执行目标方法
                return InvokeProxy();
            }
            // 递归调用拦截器
            return interceptorList[i++].Intercept(this);
        }


        /// <summary>
        /// 单一方法执行
        /// </summary>
        /// <param name="invokeInfo"></param>
        /// <param name="method"></param>
        private object InvokeProxy() {
            ServiceMethod methodMetaData = invokeInfo.MethodMetaData;

            log.DebugFormat("执行目标方法{0}.{1}", methodMetaData.OwnerService.Name, methodMetaData.Name);

            // 获取目标服务的桥接器
            IServiceProxy proxy = methodMetaData.OwnerService.ServiceProxy;
            return proxy.FuncSwitch(
                        methodMetaData.Id,
                        invokeInfo.TarParamValues
                    );
        }


    }
}