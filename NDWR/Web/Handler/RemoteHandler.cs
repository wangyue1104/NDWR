//-----------------------------------------------------------------------------------------
//   <copyright  file="RemoteHandler.cs">
//      所属项目：NDWR.Handler
//      创 建 人：王跃
//      创建日期：2012-7-24 14:35:43
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Web.Handler {
    using System.Web;
    using System.Web.SessionState;
    using NDWR.Web;

    /// <summary>
    /// RemoteHandler 概要
    /// </summary>
    public class RemoteHandler : IHttpHandler, IRequiresSessionState {
        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            ContextSupport cs = new ContextSupport(context);
            try {
                cs.ProcessRequest();
                cs.Response.WriteResult();
                cs.CompleteRequest();
            } catch {
                throw new HttpException(500, "服务端异常");
            }
        }

    }
}
