//-----------------------------------------------------------------------------------------
//   <copyright  file="InvocationBatch.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-7-25 15:01:46
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR {
    using System.Collections.Generic;
    using System.Linq;
    using NDWR.Web;
    using System.Web;
    using NDWR.InvocationManager;
    using NDWR.ServiceStruct;

    /// <summary>
    /// 批量执行是全局信息
    /// 该类实例伴随整个回话执行流程
    /// 作为回话处理状态的快照
    /// </summary>
    public class InvocationBatch {

        public InvocationBatch(string batchID, Invocation[] invokes, HttpFileCollection files) {
            this.BatchId = batchID;
            this.Invokes = invokes;
            this.Files = files;

            foreach (Invocation invoke in this.Invokes) {
                invoke.BatchOwner = this;
            }
        }

        /// <summary>
        /// 根据key获取文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TransferFile Get(string key) {
            if (string.IsNullOrWhiteSpace(key)) {
                return null;
            }
            HttpPostedFile PostFile = this.Files["ndwr_file_" + key];
            if (PostFile == null) {
                return null;
            }
            return new TransferFile(PostFile);
        }


        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchId { get; private set; }
        /// <summary>
        /// 批次执行集合
        /// </summary>
        public Invocation[] Invokes { get; private set; }
        /// <summary>
        /// 上传文件
        /// </summary>
        public HttpFileCollection Files { get; private set; }
        /// <summary>
        /// 输出模版
        /// </summary>
        public IResponse Response { get; set; }

    }

    /// <summary>
    /// 用户执行公开方法时的执行信息
    /// </summary>
    public class Invocation {

        public Invocation(int methodIndex, ServiceMethod methodMetaData, ParamItem[] paramValues) {

            MethodIndex = methodIndex;
            MethodMetaData = methodMetaData;
            ParamValues = paramValues;
            //this.TarParamValues = (from item in ParamValues
            //                       select item.TarValue).ToArray();
            // 初始化集合
            SystemErrors = new List<RspError>();
        }

        /// <summary>
        /// 
        /// </summary>
        public InvocationBatch BatchOwner { get; set; }

        /// <summary>
        /// 在批量调用中顺序的索引
        /// </summary>
        public int MethodIndex { get; private set; }
        /// <summary>
        /// 远程服务
        /// </summary>
        public ServiceMethod MethodMetaData { get; set; }
        /// <summary>
        /// 记录客户提交参数的信息，对应的服务器参数值
        /// </summary>
        public ParamItem[] ParamValues {get;private set;}
        /// <summary>
        /// 客户提交参数转换后的参数值
        /// </summary>
        public object[] TarParamValues {
            get {
                var t = from item in ParamValues
                        select item.TarValue;
                return t.ToArray();
            }
        }
        /// <summary>
        /// 系统错误集合
        /// </summary>
        public IList<RspError> SystemErrors { get; private set; }
        /// <summary>
        /// 方法返回值
        /// </summary>
        public object RetValue { get; set; }
        /// <summary>
        /// 输出模版
        /// </summary>
        public IResponse Response { get; set; }

    }

}
