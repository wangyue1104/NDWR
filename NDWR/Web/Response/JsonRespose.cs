//-----------------------------------------------------------------------------------------
//   <copyright  file="IRespose.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-7-26 11:39:51
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Web {
    using System.Collections;
    using System.Collections.Generic;
    using System.Web;
    using NDWR.Config;
    using System.Text;
    using NDWR.Validator;

    /// <summary>
    /// JsonRespose
    /// 标准json格式文本流输出
    /// </summary>
    public class JsonRespose : IResponse {
        

        protected string jsonResult = string.Empty;
        // 标识外部是否采用Iframe调用
        private bool isIframe = false;

        private readonly string prefixScriptNode = string.Empty; // 脚本节点前缀
        private readonly string suffixScriptNode = string.Empty; // 脚本节点后缀
        private readonly string ndwrVariableHost = string.Empty; // ndwr变量宿主
        private readonly string contentType = "text/plain";

        public JsonRespose(HttpResponse response) {
            Response = response;
        }
        public JsonRespose(HttpResponse response,bool isIframe) {
            Response = response;
            this.isIframe = isIframe;
            if (isIframe) {
                prefixScriptNode = "<script type=\"text/javascript\">\r\n";
                suffixScriptNode = "\r\n</script>";
                ndwrVariableHost = "window.parent.";
                contentType = "text/html";
            }
        }

        /// <summary>
        /// Web设备上下文
        /// </summary>
        public HttpResponse Response { get; private set; }
        /// <summary>
        /// 执行信息列表
        /// </summary>
        public InvocationBatch InvokeBatch { get; set; }
        /// <summary>
        /// 构造结果集
        /// </summary>
        protected void buildResult() {
            if (Response == null || InvokeBatch == null) {
                throw new NDWRException("输出时参数不完整");
            }

            StringBuilder jsInvoke = new StringBuilder(prefixScriptNode);
            jsInvoke.Append("/*NDWR's reply*/\r\n");

            foreach (Invocation invokeInfo in InvokeBatch.Invokes) {
                jsInvoke.AppendFormat("\r\n/*{0}.{1}*/\r\n", invokeInfo.MethodMetaData.OwnerService.Name, invokeInfo.MethodMetaData.Name);
                jsInvoke.AppendFormat("{0}ndwr.handleCallback({1},{2},{3}{4});\r\n",
                    ndwrVariableHost,
                    InvokeBatch.BatchId,
                    invokeInfo.MethodIndex,
                    invokeInfo.RetValue == null ? 
                        "null" :
                        GlobalConfig.Instance.JsonSerializer.Serializer(invokeInfo.RetValue),
                    invokeInfo.SystemErrors.Count == 0 ?
                        string.Empty :
                        "," + GlobalConfig.Instance.JsonSerializer.Serializer(invokeInfo.SystemErrors));
            }

            jsInvoke.Append(suffixScriptNode);
            jsonResult = jsInvoke.ToString();
        }


        private void ResponseResult() {
            Response.Clear();
            Response.ContentType = contentType;
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.Write(jsonResult);
        }

        public virtual void WriteResult() {
            buildResult();
            ResponseResult();
        }

    }
}
