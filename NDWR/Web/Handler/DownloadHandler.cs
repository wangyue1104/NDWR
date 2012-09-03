using System.Web;

namespace NDWR.Web.Handler {
    internal class DownloadHandler : IHttpHandler{

        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context) {
            string downloadID = context.Request.QueryString["id"];
            if (string.IsNullOrEmpty(downloadID)) {
                return;
            }

            TransferFile FileInfo = NDWR.Util.Kit.GetCache<TransferFile>(downloadID);
            if (FileInfo == null) {
                throw new HttpException(410, "资源过期，不存在");
            }
            NDWR.Util.Kit.RemoveCache(downloadID);

            context.Response.ContentType = FileInfo.ContentType;
            context.Response.ContentEncoding = System.Text.Encoding.UTF8;
            context.Response.Charset = "";
            context.Response.AppendHeader(
                "Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(FileInfo.FileName, System.Text.Encoding.UTF8));
            context.Response.BinaryWrite(FileInfo.DataBuffer);
            context.ApplicationInstance.CompleteRequest();
        }
    }
}
