//-----------------------------------------------------------------------------------------
//   <copyright  file="IRequest.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-7-26 11:39:41
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    /// <summary>
    /// IRequest 概要
    /// </summary>
    public class AjaxRequest {
        private HttpRequest request;
        private InvocationBatch invokeBatch;

        public InvocationBatch InvokeBatch {
            get { return invokeBatch; }
        }

        public AjaxRequest(HttpRequest request) {
            this.request = request;
            // 收集参数
            collection();
        }

        private void collection() {
            string batchID = request.Form["SABatchID"];
            if (string.IsNullOrWhiteSpace(batchID)) {
                throw new NDWRException("非协定的请求：没有批次号");
            }
            Invocation[] invokes = collectionMethod();
            if (invokes == null) {
                throw new NDWRException("未发现调用用例");
            }
            invokeBatch = new InvocationBatch(batchID,invokes);
        }

        /// <summary>
        /// 收集调用的方法
        /// 规则:Method|[paramIndex] = [ServiceName].[MethodName]
        /// </summary>
        private Invocation[] collectionMethod() {
            int methodIndex = 0;
            string[] key;
            IList<Invocation> invokeList = new List<Invocation>();

            for (int i = 0; i < request.Form.Count; i++) {
                key = request.Form.Keys[i].Split('|');
                if (key.Length == 2 && key[0] == "Method" &&
                    int.TryParse(key[1], out methodIndex)) { // 满足Method|[paramIndex]条件

                    string[] value = request.Form[i].Split('.');
                    if (value.Length == 2) { // 满足[ServiceName].[MethodName]条件
                        invokeList.Add(
                            new Invocation(methodIndex, value[0], value[1], collectionParam(methodIndex))
                        );
                    }
                }
            }
            var rt = from item in invokeList
                     orderby item.MethodIndex
                     select item;
            return rt.ToArray();
        }

        /// <summary>
        /// 参数收集
        /// Param|[paramIndex]|[methodIndex]  = [value]
        /// </summary>
        private string[] collectionParam(int methodIndex) {
            int paramIndex = 0;

            IList<ParamItem> paramitems = new List<ParamItem>();
            for (int i = 0; i < request.Form.Count; i++) {
                string[] key = request.Form.Keys[i].Split('|');
                if (key.Length == 3 && key[0] == "Param" && methodIndex.ToString().Equals(key[2]) &&
                    int.TryParse(key[1], out paramIndex)) {

                        paramitems.Add(new ParamItem(paramIndex, request.Form[i]));
                }
            }
            // 对参数顺序按照传递索引进行排序
            var rt = from item in paramitems
                    orderby item.Id ascending
                    select item.Value;
            return rt.ToArray();

        }


        /// <summary>
        /// 获取Request值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Invocation RequestValue(string service, string method) {
            foreach (Invocation invo in InvokeBatch.Invokes) {
                if (invo.Service == service && invo.Method == method) {
                    return invo;
                }
            }
            return null;
        }



        private class ParamItem {

            public ParamItem(int id, string value) {
                this.Id = id;
                this.Value = value;
            }

            public int Id { get; set; }
            public string Value { get; set; }
        }
    }
}
