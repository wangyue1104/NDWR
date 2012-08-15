//-----------------------------------------------------------------------------------------
//   <copyright  file="RequestParams.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-7-25 14:56:38
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

/*
 * ajax调用，采用post传值，批量调用规则如下：
 *      [ServiceName] 服务名
 *      [MethodName] 方法名
 *      [value] 参数值
 *      [index] 参数顺序索引
 *      [batchID] 批次索引
 *      
 SABatchID = [BatchID]
 Method|[methodIndex] = [ServiceName].[MethodName]
 Param|[paramIndex]|[methodIndex] = [value]
 Param|[paramIndex]|[methodIndex] = [value]
 Param|[paramIndex]|[methodIndex] = [value]
 Param|[paramIndex]|[methodIndex] = [value]
 */

namespace NDWR.Web {
    using System;
    using System.Web;

    /// <summary>
    /// RequestParams 概要
    /// </summary>
    public class ContextSupport {

        private HttpContext _httpContext;
        private AjaxRequest _request;
        private IResponse _response;

        public IResponse Response {
            get { return _response; }
            private set { _response = value; }
        }

        public AjaxRequest Request {
            get { return _request; }
            set { _request = value; }
        }

        public ContextSupport(HttpContext httpContext) {
            this._httpContext = httpContext;

            // 收集参数
            _request = new AjaxRequest(httpContext.Request);
        }

        /// <summary>
        /// 批量执行请求，并返回输出实体
        /// </summary>
        /// <returns></returns>
        public void ProcessRequest() {
            Response = MethodInvocationManager.Instance.BatchInvoke(_request.InvokeBatch);
            if (Response != null) {
                Response.Context = _httpContext;
                Response.InvokeBatch = _request.InvokeBatch;
            }
        }


        public void NullResponse(Exception ex) {
            Response = new NullResponse();
                Response.Context = _httpContext;
                Response.InvokeBatch = _request.InvokeBatch;
        }
    }

}
