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
    /// FileDownloadResponse 概要
    /// </summary>
    public class FileDownloadResponse : IResponse{
        /// <summary>
        /// Web设备上下文
        /// </summary>
        public HttpContext Context { get; set; }
        /// <summary>
        /// 执行信息列表
        /// </summary>
        public InvocationBatch InvokeBatch { get; set; }


        public void WriteResult() {
            if (InvokeBatch == null || InvokeBatch.Invokes == null || InvokeBatch.Invokes.Length != 1) {
                throw new NDWRException("文件下载异常");
            }

            TransferFile FileInfo = InvokeBatch.Invokes[0].RetValue as TransferFile;
            
            //if (FileInfo == null) {
            //    throw new NDWRException("文件下载异常");
            //}
            Context.Response.ContentType = FileInfo.ContentType;
            Context.Response.ContentEncoding = Encoding.UTF8;
            Context.Response.Charset = "";
            Context.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(FileInfo.FileName, Encoding.UTF8));
            Context.Response.BinaryWrite(FileInfo.DataBuffer);
            Context.Response.End();
        }
    }
}
