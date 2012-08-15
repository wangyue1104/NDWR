//-----------------------------------------------------------------------------------------
//   <copyright  file="RemoteInvocation.cs">
//      所属项目：NDWR
//      创 建 人：王跃
//      创建日期：2012-7-25 17:10:38
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR {
    using System.Collections.Generic;
    using NDWR.Config;
    using NDWR.MethodInterceptor;
    using NDWR.ServiceStruct;
    using NDWR.Web;

    /// <summary>
    /// RemoteInvocation 概要
    /// </summary>
    public class MethodInvocationManager {

        private IList<Service> serviceList;

        
        private IList<Interceptor> interceptorList;

        private static MethodInvocationManager instance = null;
        private static object objLock = new object();

        private MethodInvocationManager() {
            serviceList = GlobalConfig.Instance.ServiceScanner.Services;
            interceptorList = GlobalConfig.Instance.Interceptors;
        }

        public static MethodInvocationManager Instance {
            get {
                if (instance == null) {
                    lock (objLock) {
                        if (instance == null) {
                            instance = new MethodInvocationManager();
                        }
                    }
                }
                return instance;
            }
        }
        /*
        public IList<IService> ServiceList {
            get { return serviceList; }
            set { serviceList = value; }
        }*/

        /// <summary>
        /// 匹配到对应的发布方法
        /// </summary>
        protected ServiceMethod MatchingServiceMethod(string serviceName, string methodName) {
            foreach (Service service in serviceList) {
                if (service.Name == serviceName) {
                    foreach (ServiceMethod method in service.PublicMethod) {
                        if (method.Name == methodName) {
                            return method;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 批量方法执行
        /// </summary>
        /// <param name="infos"></param>
        public IResponse BatchInvoke(InvocationBatch invokeBatch) {
            IResponse response = null;
            foreach (Invocation info in invokeBatch.Invokes) {
                Invoke(
                    info,
                    MatchingServiceMethod(info.Service, info.Method)
                );
                if (info.Response != null) {
                    response = info.Response;
                }
            }

            return response;
        }

        /// <summary>
        /// 单一方法执行
        /// </summary>
        /// <param name="invokeInfo"></param>
        /// <param name="method"></param>
        public void Invoke(Invocation invokeInfo, ServiceMethod method) {
            // 如果公开方法不存在
            if (method == null) {
                invokeInfo.SystemErrors.Add(
                    new RspError() {
                        Name = SystemError.NoMatchService.ToString(),
                        Message = "没有匹配到服务"
                    });
                invokeInfo.RetValue = null;
                return;
            }

            MehtodInvocation mi = new MehtodInvocation(invokeInfo, method);
            invokeInfo.RetValue = mi.Invoke();
        }

    }

}
