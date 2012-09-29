//-----------------------------------------------------------------------------------------
//   <copyright  file="InvocationBatch.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-7-25 15:01:46
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR {
    using System.Collections.Generic;
    using System.Linq;
    using NDWR.Web;
    using System.Web;
    using NDWR.InvocationManager;
    using NDWR.ServiceStruct;
    using NDWR.Util;
    using NDWR.MethodInterceptor;
    using NDWR.Config;
    using NDWR.ByteCode;

    /// <summary>
    /// 用户执行公开方法时的执行信息
    /// </summary>
    public class Invocation {
        private static readonly NDWR.Logging.ILog log = NDWR.Logging.LogManager.GetLogger(typeof(Invocation));

        private int i = 0;// 拦截器递归调用计数器
        private IList<Interceptor> interceptorList = GlobalConfig.Instance.Interceptors; // 获取拦截器集合

        public Invocation(int methodIndex, ServiceMethod methodMetaData, ParamItem[] ParamItems,IRequest request) {
            MethodIndex = methodIndex;
            MethodMetaData = methodMetaData;
            // 初始化集合
            ParamValues = ParamItems;
            this.Request = request;
            SystemErrors = new List<RspError>();
        }

        /// <summary>
        /// 本次请求
        /// </summary>
        public IRequest Request { get; private set; }
        /// <summary>
        /// 在批量调用中顺序的索引
        /// </summary>
        public int MethodIndex { get; private set; }
        /// <summary>
        /// 远程服务
        /// </summary>
        public ServiceMethod MethodMetaData { get; set; }
        /// <summary>
        /// 记录客户提交参数的信息，对应的服务器参数值
        /// </summary>
        public ParamItem[] ParamValues {get;private set;}
        /// <summary>
        /// 客户提交参数转换后的参数值
        /// </summary>
        public object[] TarParamValues {
            get {
                var t = from item in ParamValues
                        select item.TarValue;
                return t.ToArray();
            }
        }
        /// <summary>
        /// 系统错误集合
        /// </summary>
        public IList<RspError> SystemErrors { get; private set; }
        /// <summary>
        /// 方法返回值
        /// </summary>
        public object RetValue { get; set; }



        /// <summary>
        /// 执行目标方法
        /// 递归执行拦截器，最会执行目标的代理方法
        /// </summary>
        /// <returns></returns>
        public void Invoke() {
            if (i >= interceptorList.Count) { // 如果拦截器已经执行完，执行目标方法
                this.RetValue = InvokeProxy();
                return;
            }
            // 递归调用拦截器
            interceptorList[i++].Intercept(this);
        }


        /// <summary>
        /// 单一方法执行
        /// </summary>
        /// <param name="invokeInfo"></param>
        /// <param name="method"></param>
        private object InvokeProxy() {

            log.DebugFormat("执行目标方法{0}.{1}", MethodMetaData.OwnerService.Name, MethodMetaData.Name);

            // 获取目标服务的桥接器
            IServiceProxy proxy = MethodMetaData.OwnerService.ServiceProxy;
            return proxy.FuncSwitch(
                        MethodMetaData.Id,
                        TarParamValues
                    );
        }
    }

}
