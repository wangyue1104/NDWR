using NDWR.Config;
using NDWR.ServiceStruct;

namespace NDWR.Web.Handler {

    internal class ServiceJavaScriptHandler : JavascriptHandler {

        private string fileName;

        public ServiceJavaScriptHandler(string fileName) {
            this.fileName = fileName;
        }

        public override string Javascript {
            get {
                Service[] services = GlobalConfig.Instance.ServiceScanner.Services;
                foreach (Service service in services) {
                    if (service.Name == fileName) {
                        return service.JavaScript;
                    }
                }

                return string.Empty;
            }
        }
    }

}
