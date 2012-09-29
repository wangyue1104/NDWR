using NDWR.Config;
using NDWR.ServiceStruct;
using NDWR.Util;

namespace NDWR.Web.Handler {

    internal class ServiceJavaScriptHandler : JavascriptHandler {

        private string fileName;

        public ServiceJavaScriptHandler(string fileName) {
            this.fileName = fileName;
        }

        public override string Javascript {
            get {
                Service[] services = GlobalConfig.Instance.ServiceScanner.Services;
                var service = Kit.Each<Service>(services, item => item.Name == fileName);
                return service == null ?
                    string.Empty :
                    service.JavaScript;

                //foreach (Service service in services) {
                //    if (service.Name == fileName) {
                //        return service.JavaScript;
                //    }
                //}

                //return string.Empty;
            }
        }
    }

}
