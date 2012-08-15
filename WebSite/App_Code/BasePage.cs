using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///BasePage 的摘要说明
/// </summary>
public class BasePage : System.Web.UI.Page {
    public string BasePath;
    protected void Page_Load(object sender, EventArgs e) {
        BasePath = Request.Url.Scheme + "://" +
            Request.Url.Host + ":" + Request.Url.Port.ToString() +
            Request.ApplicationPath + "/";
    }
}