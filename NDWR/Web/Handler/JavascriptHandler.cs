using System.Web;

namespace NDWR.Web.Handler {

    internal abstract class JavascriptHandler : IHttpHandler {
        
        public bool IsReusable {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context) {

            context.Response.Clear();
            context.Response.ContentType = "application/x-javascript";
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");
            context.Response.Write(Javascript);
            context.ApplicationInstance.CompleteRequest();
        }

        public abstract string Javascript { get; }

    }
}
