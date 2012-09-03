//-----------------------------------------------------------------------------------------
//   <copyright  file="FileDownloadResponse.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-8-7 13:22:30
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Web {
    using System.Text;
    using System.Web;

    /// <summary>
    /// BinaryResponse 概要
    /// 二进制输出
    /// </summary>
    public class BinaryResponse : IResponse{

        /// <summary>
        /// Web设备上下文
        /// </summary>
        public HttpResponse Response { get; set; }
        /// <summary>
        /// 执行信息列表
        /// </summary>
        public InvocationBatch InvokeBatch { get; set; }


        public void WriteResult() {
            if (InvokeBatch == null || InvokeBatch.Invokes == null || InvokeBatch.Invokes.Length != 1) { // 如果是下载文件操作，不支持在批量中操作
                throw new NDWRException("文件下载异常,是否非法尝试在批量中提交文件下载调用");
            }

            TransferFile FileInfo = InvokeBatch.Invokes[0].RetValue as TransferFile;
            
            if (FileInfo == null) {
                FileInfo = new TransferFile();
                throw new NDWRException("文件下载异常");
            }

            Response.ContentType = FileInfo.ContentType;
            Response.ContentEncoding = Encoding.UTF8;
            Response.Charset = "";
            Response.AppendHeader(
                "Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(FileInfo.FileName, Encoding.UTF8));
            Response.BinaryWrite(FileInfo.DataBuffer);
            
        }

    }
}
