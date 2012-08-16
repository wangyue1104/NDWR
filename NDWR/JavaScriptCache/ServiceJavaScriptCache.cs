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

namespace NDWR.ServiceScanner {
    using System;
    using System.IO;
    using System.Text;
    using NDWR.ServiceStruct;

    /// <summary>
    /// ServiceJavaScriptCache 概要
    /// </summary>
    public class ServiceJavaScriptCache {

        public string NDWRCoreJS { get; private set; }

        private static ServiceJavaScriptCache cache = null;
        private static object objLock = new object();

        private ServiceJavaScriptCache() {
            loadNDWRCoreJS();
        }

        public static ServiceJavaScriptCache Instance {
            get {
                if (cache == null) {
                    lock (objLock) {
                        if (cache == null) {
                            cache = new ServiceJavaScriptCache();
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
            sbScript.AppendFormat("ndwr.remoteURL = '{0}ndwr/remote.ndwr';\r\n\r\n", Config.GlobalConfig.Instance.BasePath);
            sbScript.AppendFormat("{0} = {1};\r\n\r\n", service.Name, "{}");

            foreach (ServiceMethod method in service.PublicMethod) {
                sbScript.AppendFormat("{0}.{1} = {2} {{ \r\n", service.Name, method.Name, functionString(method));

                sbScript.Append(entityJsonConvert(method));



                sbScript.AppendFormat("    ndwr.RemoteMethod('{0}','{1}',arguments,{2}{3}); \r\n", service.Name, method.Name, method.Params.Length,
                    method.MethodType == MethodType.OutputBinaryStream ?
                        ",'download'" :
                        method.MethodType == MethodType.InputBinaryStream ?
                        ",'upload'" : string.Empty);


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


        private string entityJsonConvert(ServiceMethod method) {
            StringBuilder sbScript = new StringBuilder("");
            int i = 0;
            foreach (ServiceMethodParam param in method.Params) {
                if (!param.IsSimplyType) {
                    // arguments[i] = JSONUtil.encode(arguments[i]);
                    sbScript.AppendFormat("    arguments[{0}] = JSONUtil.encode(arguments[{0}]);\r\n", i);
                }
                i++;
            }
            return sbScript.ToString();
        }


        /// <summary>
        /// 加载嵌入的JS核心处理脚本
        /// </summary>
        private void loadNDWRCoreJS() {
            Stream jsSteam = null;
            StreamReader sr = null;
            try {
                jsSteam = this.GetType().Assembly.GetManifestResourceStream("NDWR.ndwrcore.js");
                sr = new StreamReader(jsSteam);

                NDWRCoreJS = sr.ReadToEnd();
            } catch (Exception ex) {
                throw new NDWRException("读取嵌入JS核心脚本异常", ex);
            } finally {
                if (sr != null) {
                    sr.Close();//关闭流
                }
                if (jsSteam != null) {
                    jsSteam.Close();
                }
            }
        }

    }
}
