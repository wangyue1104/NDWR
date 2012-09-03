//-----------------------------------------------------------------------------------------
//   <copyright  file="MethodInvocationManager.cs">
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
    using NDWR.InvocationManager;
    using NDWR.MethodInterceptor;
    using NDWR.ServiceStruct;

    /// <summary>
    /// RemoteInvocation 概要
    /// </summary>
    public class MethodInvocationManager {
        private static readonly NDWR.Logging.ILog log = NDWR.Logging.LogManager.GetLogger(typeof(MethodInvocationManager));
        // 服务元数据信息
        private IList<Service> serviceList;
        // 拦截器
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

        /// <summary>
        /// 匹配到对应的发布方法
        /// </summary>
        protected ServiceMethod getMethodMetaData(string serviceName, string methodName) {
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
        public void BatchInvoke(InvocationBatch invokeBatch) {
            foreach (Invocation info in invokeBatch.Invokes) {
                Invoke(info);
            }
        }

        /// <summary>
        /// 单一方法执行
        /// </summary>
        /// <param name="invokeInfo"></param>
        /// <param name="method"></param>
        public void Invoke(Invocation invokeInfo) {

            MethodInvocation mi = new MethodInvocation(invokeInfo);
            invokeInfo.RetValue = mi.Invoke();
        }

    }

}
