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

    /// <summary>
    /// IRespose 概要
    /// </summary>
    public class AjaxRespose : IResponse {

        private IList<RspDataPackage> rspDataList = new List<RspDataPackage>();
        private RspBatchPackage rspBatch = new RspBatchPackage();
        private string jsonResult = string.Empty;

        public AjaxRespose() {
        }


        /// <summary>
        /// Web设备上下文
        /// </summary>
        public HttpContext Context { get; set; }
        /// <summary>
        /// 执行信息列表
        /// </summary>
        public InvocationBatch InvokeBatch { get; set; }
        /// <summary>
        /// 构造结果集
        /// </summary>
        private void buildResult() {
            if (Context == null || InvokeBatch == null) {
                throw new NDWRException("输出时参数不完整");
            }
            rspBatch.BatchId = InvokeBatch.BatchId;

            IList<RspDataPackage> rspDataList = new List<RspDataPackage>();
            foreach (Invocation invokeInfo in InvokeBatch.Invokes) {
                RspDataPackage rdp = new RspDataPackage();
                rdp.MethodIndex = invokeInfo.MethodIndex;
                rdp.Service = invokeInfo.Service;
                rdp.Method = invokeInfo.Method;
                rdp.SystemErrors = invokeInfo.SystemErrors;
                //rdp.CustomeErrors = invokeInfo.CustomeErrors;

                rdp.JsonData = invokeInfo.RetValue == null ? null : GlobalConfig.Instance.JsonSerializer.Serializer(invokeInfo.RetValue);


                rspDataList.Add(rdp);
            }
            rspBatch.DataList = rspDataList;

            jsonResult = GlobalConfig.Instance.JsonSerializer.Serializer(rspBatch);
        }

        /// <summary>
        /// 输出的结果集
        /// </summary>
        public IList<RspDataPackage> RspDataList {
            get { return rspDataList; }
        }


        public void ResponseResult() {
            Context.Response.Clear();
            Context.Response.ContentType = "text/xml";
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.Write(jsonResult);
            Context.ApplicationInstance.CompleteRequest();
            //_httpContext.Response.End();
        }

        public void WriteResult() {
            buildResult();

            ResponseResult();
        }

    }
}
