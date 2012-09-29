using System.IO;
using System.Web;

namespace NDWR.Web.Handler {


    public class HandlerFactory : IHttpHandlerFactory {


        public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated) {

            string filename = Path.GetFileNameWithoutExtension(context.Request.Path);

            if (string.IsNullOrEmpty(filename)) {
                throw error404();
            }

            switch (requestType) {
                case "GET":
                    return requestType_GET(filename);
                case "POST":
                    return requestType_POST(filename);
            }

            throw error404();
        }

        public void ReleaseHandler(IHttpHandler handler) {
        }


        private IHttpHandler requestType_GET(string fileName) {
            if (fileName == "ndwrcore") {
                return new CoreJavaScriptHandler();
            }else if(fileName == "ndwrdownload"){
                return new DownloadHandler();
            }
            return new ServiceJavaScriptHandler(fileName);
        }

        private IHttpHandler requestType_POST(string fileName) {
            if (fileName == "ndwremote") {
                return new RemoteHandler();
            }
            throw error404();
        }


        protected HttpException error404() {
            return new HttpException(404, "映射失败");
        }
    }
}
