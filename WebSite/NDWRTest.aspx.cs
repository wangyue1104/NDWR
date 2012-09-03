using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default4 : System.Web.UI.Page
{

    public string BasePath;
    // ndwr.transport.xhr.send('http://localhost:1829/WebSite/NDWRTest.aspx?action=test','1=1')
    protected void Page_Load(object sender, EventArgs e)
    {
        BasePath = Request.Url.Scheme + "://" +
            Request.Url.Host + ":" + Request.Url.Port.ToString() + 
            Request.ApplicationPath + "/";
        if (Request.Params["action"] == "test") {
            Response.AddHeader("Content-Type", "text/plain");
            Response.Write("alert('OK');");
            Response.End();
        }
    }
}