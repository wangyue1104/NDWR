//-----------------------------------------------------------------------------------------
//   <copyright  file="ServiceJavaScriptCache.cs">
//      所属项目：NDWR.ServiceScanner
//      创 建 人：王跃
//      创建日期：2012-8-3 19:45:53
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.JavaScript {
    using System;
    using System.IO;
    using System.Text;
    using NDWR.ServiceStruct;
    using NDWR.Web;
    using NDWR.Config;
    using System.Collections.Generic;

    /// <summary>
    /// ServiceJavaScriptCache
    /// 单例类
    /// </summary>
    public class RemoteServiceScript {

        public const string extName = ".ashx"; // 拦截的请求扩展名
        public const string coreJSName = "ndwrcore" + extName; // 核心脚本映射文件名
        public const string remoteService = "ndwremote" + extName; // 远程提交请求映射文件名
        /// <summary>
        /// 核心JS脚本
        /// </summary>
        public string NDWRCoreJS { get; private set; }

        private static RemoteServiceScript cache = null;
        private static object objLock = new object();

        private RemoteServiceScript() {}

        public static RemoteServiceScript Instance {
            get {
                if (cache == null) {
                    lock (objLock) {
                        if (cache == null) {
                            cache = new RemoteServiceScript();
                        }
                    }
                }
                return cache;
            }
        }

        /// <summary>
        /// 生成服务的脚本
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public string BuildServiceJS(Service service) {
            StringBuilder sbScript = new StringBuilder("/*NDWR自动生成脚本*/ \r\n");
            sbScript.AppendFormat("ndwr.transport.url = '{0}ndwr/{1}';\r\n\r\n", Config.GlobalConfig.Instance.BasePath, remoteService);
            sbScript.AppendFormat("{0} = {1};\r\n\r\n", service.Name, "{}");
            // 生成服务公开方法脚本
            foreach (ServiceMethod method in service.PublicMethod) {
                // 函数定义 Service.Method = function(param1,param2,...,paramx,callbackfunc){
                sbScript.AppendFormat("{0}.{1} = {2} {{ \r\n", service.Name, method.Name, functionString(method));
                // 调用核心js库
                sbScript.AppendFormat("    ndwr.RemoteMethod('{0}','{1}',arguments,{2}); \r\n", service.Name, method.Name, method.Params.Length);
                // 函数结束
                sbScript.Append("}\r\n\r\n");
            }
            sbScript.Append("\r\n");

            return sbScript.ToString();
        }

        /// <summary>
        /// 生成函数头 function(param1,param2,param3,...)
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private string functionString(ServiceMethod method) {
            StringBuilder sbScript = new StringBuilder("function(");

            foreach (ServiceMethodParam param in method.Params) {
                sbScript.AppendFormat("{0},", param.Name);
            }

            sbScript.Append("callBackFunc");

            sbScript.Append(")");
            return sbScript.ToString();
        }
        
        
    }
}
