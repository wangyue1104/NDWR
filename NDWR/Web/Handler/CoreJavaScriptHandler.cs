using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NDWR.JavaScript;

namespace NDWR.Web.Handler {

    internal class CoreJavaScriptHandler : JavascriptHandler{

        public override string Javascript {
            get { return CoreJavaScript.Content; }
        }
    }
}
