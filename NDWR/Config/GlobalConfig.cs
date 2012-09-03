//-----------------------------------------------------------------------------------------
//   <copyright  file="GlobalConfig.cs">
//      所属项目：NDWR.Config
//      创 建 人：王跃
//      创建日期：2012-7-25 14:37:19
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Config {
    using System.Collections.Generic;
using System.Web;
using NDWR.JsonSerializer;
using NDWR.Logging;
using NDWR.MethodInterceptor;
using NDWR.ServiceScanner;

    /// <summary>
    /// GlobalConfig 概要
    /// </summary>
    public class GlobalConfig {

        private static GlobalConfig config = new GlobalConfig();

        private GlobalConfig() {
            Interceptors = new List<Interceptor>();
            HttpRequest request = HttpContext.Current.Request;
            BasePath = request.Url.Scheme + "://" +
                request.Url.Host + ":" + request.Url.Port.ToString() +
                request.ApplicationPath + "/";
        }

        public static GlobalConfig Instance {
            get {
                return config;
            }
        }

        public void DefaultConfig(string assembly) {
            DefaultConfig(assembly, null);
        }
        public void DefaultConfig(string assembly,ILogFactory factory) {
            // 首先要设置日志工厂
            if (factory != null) {
                LogFactory = factory;
            }
            // Json序列化
            JsonSerializer = new NewtonsoftJsonSerializerImpl();
            // 服务扫描
            ServiceScanner = new AttributeServiceScanner(assembly);

            // 添加基础类型参数映射
            Interceptors.Add(new ExceptionInterceptor()); // 异常捕获
            Interceptors.Add(new ParamConvertInterceptor()); // 参数转换
            Interceptors.Add(new DownloadInterceptor()); // 输出模版

        }

        /// <summary>
        /// 公开方法搜索方式
        /// </summary>
        public IServiceScanner ServiceScanner { get; set; }
        /// <summary>
        /// json序列化
        /// </summary>
        public IJsonSerializer JsonSerializer { get; set; }
        /// <summary>
        /// 拦截器
        /// </summary>
        public IList<Interceptor> Interceptors { get; set; }
        /// <summary>
        /// 日志工厂
        /// </summary>
        public ILogFactory LogFactory {
            set {
                LogManager.AssignFactory(value);
            }
        }
        /// <summary>
        /// 服务器路径
        /// </summary>
        public string BasePath { get; private set; }
        /// <summary>
        /// 调试模式
        /// </summary>
        public bool DebugMode { get; set; }
    }
}
