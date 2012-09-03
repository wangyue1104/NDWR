//-----------------------------------------------------------------------------------------
//   <copyright file="JsonpResponse.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-8-18 12:01:30
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Web {
    using System.Text;

    /// <summary>
    /// 用于跨域提交，也是上传的提交方式
    /// </summary>
    public class JsonpResponse : JsonRespose {
        

        // Methods
        private void ResponseResult() {
            base.Context.Response.Clear();
            base.Context.Response.ContentType = "text/plain";
            base.Context.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
            base.Context.Response.Write(base.jsonResult);
            base.Context.ApplicationInstance.CompleteRequest();
        }

        public override void WriteResult() {
            base.buildResult();
            StringBuilder sbJsonp = new StringBuilder("<script type=\"text/javascript\">\r\n");
            sbJsonp.AppendFormat("window.parent.ndwr.RemoteCallback({0})\r\n", base.jsonResult);
            sbJsonp.Append("</script>");
            base.jsonResult = sbJsonp.ToString();
            this.ResponseResult();
        }
    }

}
