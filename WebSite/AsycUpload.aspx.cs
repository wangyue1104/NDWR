using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AsycUpload : System.Web.UI.Page {

    protected void Page_Load(object sender, EventArgs e) {
        if (Request.Params["action"] == "upload") {
            if (Request.Files == null || Request.Files.Count == 0) {
                Response.Write("");
            } else {
                Response.Write(Request.Files[0].FileName);
                Request.Files[0].SaveAs(
                    Server.MapPath("~/Upload") + "\\" + Request.Files[0].FileName
                );
            }

            Response.End();
        }
    }
}