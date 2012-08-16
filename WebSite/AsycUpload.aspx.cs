using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AsycUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Params["action"] == "upload") {
            Response.Write(
                (Request.Files == null || Request.Files.Count == 0) ? "" : Request.Files[0].FileName
            );
            //Request.Files[0].InputStream;
            Response.End();
        }
    }
}