//-----------------------------------------------------------------------------------------
//   <copyright  file="AjaxScriptHandler.cs">
//      所属项目：NDWR.Handler
//      创 建 人：王跃
//      创建日期：2012-7-24 14:35:43
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Handler {
    using System.Web;
    using NDWR.Web.Script;

    /// <summary>
    /// AjaxScriptHandler 概要
    /// </summary>
    public class AjaxScriptHandler : IHttpHandler{
        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            context.Response.Clear();
            context.Response.ContentType = "application/x-javascript";
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("utf-8");


            string[] urlArr = context.Request.AppRelativeCurrentExecutionFilePath.Split('/');
            if (urlArr == null || urlArr[1] != "ndwr" || urlArr.Length != 3) {
                context.Response.Write("//未匹配到服务脚本");
                context.Response.End();
                return;
            }

            RemoteServiceScript scripBuild = new RemoteServiceScript();
            string scriptFile = urlArr[2];
            scripBuild.BulidServiceScript(scriptFile);
            context.Response.Write(
                scripBuild.BulidServiceScript(scriptFile)
            );
            context.Response.End();
        }
    }
}
