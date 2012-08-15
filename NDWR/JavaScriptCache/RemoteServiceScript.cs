//-----------------------------------------------------------------------------------------
//   <copyright  file="RemoteServiceScript.cs">
//      所属项目：NDWR.Web.Script
//      创 建 人：王跃
//      创建日期：2012-8-3 15:43:03
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Web.Script {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;
    using NDWR.Config;
    using NDWR.ServiceScanner;
    using NDWR.ServiceStruct;

    /// <summary>
    /// RemoteServiceScript 概要
    /// </summary>
    public class RemoteServiceScript {

        /// <summary>
        /// 生成服务的脚本
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public string BulidServiceScript(string scriptFile) {
            if (scriptFile == "ndwrcore.js") {
                return ServiceJavaScriptCache.Instance.NDWRCoreJS;
            } else {
                Service service = GetService(scriptFile);
                if (service == null) {
                    return string.Empty;
                }
                return service.JavaScript;
            }
            //return string.Empty;
        }

        /// <summary>
        /// 按照服务名获取服务信息
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Service GetService(string scriptFile) {

            IList<Service> serviceList = GlobalConfig.Instance.ServiceScanner.Services;
            foreach (Service service in serviceList) {
                if (service.Name + ".js" == scriptFile) {
                    return service;
                }
            }
            return null;
        }

        /// <summary>
        /// 生成函数头 function(param1,param2,param3,...,callBackFunc)
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public string ParamsString(ServiceMethod method) {
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
