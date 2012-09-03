//-----------------------------------------------------------------------------------------
//   <copyright  file="StdRequest.cs">
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
    using NDWR.ServiceStruct;
    using NDWR.Config;

    /// <summary>
    /// 标准用户请求信息收集类
    /// </summary>
    public class StdRequest {
        private HttpRequest request;
        private InvocationBatch invokeBatch;
        private IList<Service> serviceList;

        public StdRequest(HttpRequest request) {
            this.request = request;
            serviceList = GlobalConfig.Instance.ServiceScanner.Services;
            // 收集参数
            collection();
        }

        public InvocationBatch InvokeBatch {
            get { return invokeBatch; }
        }

        public string TransferMode { get; private set; }

        private void collection() {
            string batchID = request.Form["BatchID"];
            TransferMode = request.Form["transferMode"];
            if (string.IsNullOrWhiteSpace(batchID)) {
                throw new NDWRException("非协定的请求：没有批次号");
            }
            
            Invocation[] invokes = collectionMethod();
            if (invokes == null) {
                throw new NDWRException("未发现调用用例");
            }
            invokeBatch = new InvocationBatch(batchID, invokes,request.Files);
        }

        /// <summary>
        /// 收集调用的方法
        /// 规则:Method|[paramIndex] = [ServiceName].[MethodName]
        /// </summary>
        private Invocation[] collectionMethod() {
            int methodIndex = 0;
            string[] keys;
            ServiceMethod methodMD;
            IList<Invocation> invokeList = new List<Invocation>();

            for (int i = 0; i < request.Form.Count; i++) { // 只会对post提交参数收集
                keys = request.Form.Keys[i].Split('|');
                if (keys.Length == 2 && keys[0] == "Method" 
                    && int.TryParse(keys[1], out methodIndex)) { // 满足Method|[paramIndex]条件

                    string[] value = request.Form[i].Split('.');
                    if (value.Length == 2) { // 满足[ServiceName].[MethodName]条件
                        methodMD = getMethodMetaData(value[0], value[1]);
                        if (methodMD == null) {
                            throw new NDWRException(string.Format("未匹配到服务[{0}].[{1}]", value[0], value[1]));
                        }
                        invokeList.Add(
                            new Invocation(methodIndex, methodMD, collectionParam(methodIndex))
                        );
                    }
                }
            }
            if (invokeList.Count < 2) { // 如果只有一个执行请求
                return invokeList.ToArray();
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
        private ParamItem[] collectionParam(int methodIndex) {
            int paramIndex = 0;

            IList<ParamItem> paramitems = new List<ParamItem>();
            for (int i = 0; i < request.Form.Count; i++) {
                string[] key = request.Form.Keys[i].Split('|');
                if (key.Length == 3 && key[0] == "Param"  // 参数格式满足 Param|[paramIndex]|[methodIndex] 
                    && methodIndex.ToString().Equals(key[2])  // 且为当前方法索引下
                    && int.TryParse(key[1], out paramIndex)) { // 成功转换参数索引

                    paramitems.Add(new ParamItem(paramIndex, request.Form[i]));
                }
            }
            if (paramitems.Count < 2) {
                return paramitems.ToArray();
            }
            // 对参数顺序按照传递索引进行排序
            var rt = from item in paramitems
                     orderby item.Id ascending
                     select item;
            return rt.ToArray();
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
    }
}
