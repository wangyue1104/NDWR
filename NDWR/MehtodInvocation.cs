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

namespace NDWR {
    using System.Collections.Generic;
    using NDWR.ByteCode;
    using NDWR.Config;
    using NDWR.MethodInterceptor;
    using NDWR.ServiceStruct;

    /// <summary>
    /// 单个目标方法执行
    /// 该类只能用与一次调用
    /// </summary>
    public class MehtodInvocation {

        private int i = 0; // 拦截器递归调用计数器
        private IList<Interceptor> interceptorList = GlobalConfig.Instance.Interceptors; // 获取拦截器集合

        private Invocation invokeInfo;
        private ServiceMethod method;


        /// <summary>
        /// 当前执行信息
        /// </summary>
        public Invocation InvokeInfo {
            get { return invokeInfo; }
        }

        /// <summary>
        /// 映射的方法元数据
        /// </summary>
        public ServiceMethod Method {
            get { return method; }
        }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="invokeInfo"></param>
        /// <param name="method"></param>
        public MehtodInvocation(Invocation invokeInfo, ServiceMethod method) {
            this.invokeInfo = invokeInfo;
            this.method = method;
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
            // 获取目标服务的桥接器
            IServiceProxy adapter = method.OwnerService.ServiceProxy;

            //try {
                return adapter.FuncSwitch(
                        method.Name,
                        invokeInfo.TargetParams
                    );
            //} catch (Exception ex) {
            //    invokeInfo.SystemErrors.Add(
            //        new RspError() {
            //            Name = SystemError.ServiceException.ToString(),
            //            Message = "服务端方法异常"
            //        });
            //    throw new NDWRException("方法执行失败", ex);
            //}
        }


    }
}