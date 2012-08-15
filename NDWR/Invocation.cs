//-----------------------------------------------------------------------------------------
//   <copyright  file="Param.cs">
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
    using NDWR.Web;

    /// <summary>
    /// 批量执行是全局信息
    /// 该类实例伴随整个回话执行流程
    /// 作为回话处理状态的快照
    /// </summary>
    public class InvocationBatch {

        public InvocationBatch(string batchID, Invocation[] invokes) {
            this.BatchId = batchID;
            this.Invokes = invokes;
        }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchId { get; private set; }

        public Invocation[] Invokes { get; private set; }

    }

    /// <summary>
    /// 用户执行公开方法时的执行信息
    /// </summary>
    public class Invocation {

        private string[] sourceParams;

        public Invocation(int methodIndex, string service, string method, string[] sourceParams) {

            MethodIndex = methodIndex;
            Service = service;
            Method = method;
            SourceParams = sourceParams;
            // 初始化集合
            SystemErrors = new List<RspError>();
        }

        /// <summary>
        /// 在批量调用中顺序的索引
        /// </summary>
        public int MethodIndex { get; private set; }
        /// <summary>
        /// 远程服务
        /// </summary>
        public string Service { get; private  set; }
        /// <summary>
        /// 远程方法
        /// </summary>
        public string Method { get; private set; }
        /// <summary>
        /// 远程方法参数
        /// </summary>
        public string[] SourceParams {
            get { return this.sourceParams; }
            private set { 
                this.sourceParams = value;
                if (sourceParams != null && sourceParams.Length > 0) { // 映射出相同个数的目标参数
                    TargetParams = new object[sourceParams.Length];
                }
            }
        }
        /// <summary>
        /// 目标参数
        /// </summary>
        public object[] TargetParams { get; private set; }
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
