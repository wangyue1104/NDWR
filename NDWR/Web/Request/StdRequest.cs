using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using NDWR.Config;
using NDWR.ServiceScanner;
using NDWR.ServiceStruct;

namespace NDWR.Web {

    public class StdRequest : IRequest {

        private HttpRequest request;
        private IServiceScanner serviceScanner;

        public StdRequest(HttpRequest request) {
            this.request = request;
            serviceScanner = GlobalConfig.Instance.ServiceScanner;
            // 收集参数
            parseBatchId();
            parseTransportMode();
            parseInvocations();
        }

        public string TransferMode { get; private set; }

        public string BatchId { get; private set; }

        public Invocation[] BatchInvoke { get; private set; }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TransferFile GetFlie(string key) {
            if (string.IsNullOrWhiteSpace(key)) {
                return null;
            }
            HttpPostedFile PostFile = request.Files["ndwr_file_" + key];
            if (PostFile == null) {
                return null;
            }
            return new TransferFile(PostFile);
        }

        /// <summary>
        /// 规则:Method|[paramIndex] = [ServiceName].[MethodName]
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private Invocation parseMethod(string index, string value) {
            int outVal;
            if(string.IsNullOrEmpty(value)){
                throw new NDWRException("没有指定服务");
            }
            if (!int.TryParse(index, out outVal)) {
                throw new NDWRException("调用服务时索引类型错误");
            }
            string[] vals = value.Split('.');
            if (vals.Length != 2) {
                throw new NDWRException("调用的服务部满足规则[Service].[Method]");
            }
            ServiceMethod methodMD = this.serviceScanner.GetMethod(vals[0], vals[1]);
            if (methodMD == null) {
                throw new NDWRException(string.Format("未匹配到服务[{0}].[{1}]", value[0], value[1]));
            }

            return new Invocation(
                outVal, 
                methodMD,
                parseParamter(methodMD,outVal),
                this);

        }

        /// <summary>
        /// 如果匹配服务，则按照索引从 0 开始匹配参数
        /// 规则：Param|[paramIndex]|[methodIndex] = [value]
        /// </summary>
        /// <param name="methodMD"></param>
        /// <returns></returns>
        private ParamItem[] parseParamter(ServiceMethod methodMD,int methodIndex) {
            IList<ParamItem> paramlst = new List<ParamItem>();
            NameValueCollection form = request.Form;
            string value;
            string tmp = "Param|{0}|{1}";
            for (int i = 0; i < methodMD.Params.Length; i++) {
                value = form[string.Format(tmp, i, methodIndex)];
                paramlst.Add(
                    new ParamItem(methodIndex, i, value)
                );
            }
            return paramlst.ToArray();
        }


        /// <summary>
        /// 获取批次号
        /// </summary>
        private void parseBatchId() {
            this.BatchId = request.Form["BatchID"];
            if (string.IsNullOrWhiteSpace(this.BatchId)) {
                throw new NDWRException("非协定的请求：没有批次号");
            }
        }
        /// <summary>
        /// 获取提交模式 可空
        /// </summary>
        private void parseTransportMode() {
            this.TransferMode = request.Form["TransportMode"];
        }

        private void parseInvocations() {
            IList<Invocation> methodlst = new List<Invocation>();
            string tempItem;
            string[] args;
            NameObjectCollectionBase.KeysCollection keys = request.Form.Keys;
            for (int i = 0; i < keys.Count; i++) {
                tempItem = keys[i];
                if(string.IsNullOrEmpty(tempItem)){ // 如果该项为空 跳过该次
                    continue;
                }
                args = tempItem.Split('|');
                if (args.Length != 2 || args[0] != "Method") { // 不满足Method|[paramIndex] = [ServiceName].[MethodName] 跳过该次
                    continue;
                }
                methodlst.Add(
                    parseMethod(args[1], request.Form[tempItem])
                );
            }
            this.BatchInvoke = (from item in methodlst
                                orderby item.MethodIndex
                                select item).ToArray();
        }
        
    }
}
