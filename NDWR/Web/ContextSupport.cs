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


        public HttpContext HttpContext { get; private set; }

        public IResponse Response { get; private set; }

        public IRequest Request { get; private set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContext"></param>
        public ContextSupport(HttpContext httpContext) {
            this.HttpContext = httpContext;

            // 初始化请求，收集参数
            Request = new StdRequest(HttpContext.Request);
            // 生成输出模板
            Response = ResponseFactory.Get(Request.TransferMode, this);
        }

        /// <summary>
        /// 批量执行请求，并返回输出实体
        /// </summary>
        /// <returns></returns>
        public void ProcessRequest() {
            var batchInvoke = Request.BatchInvoke;
            for (int i = 0; i < batchInvoke.Length; i++) {
                batchInvoke[i].Invoke();
            }
        }

        /// <summary>
        /// 完成输出
        /// </summary>
        public void CompleteRequest() {
            HttpContext.ApplicationInstance.CompleteRequest();
        }

    }

}
