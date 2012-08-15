//-----------------------------------------------------------------------------------------
//   <copyright  file="RemoteServiceHandler.cs">
//      所属项目：NDWR.Handler
//      创 建 人：王跃
//      创建日期：2012-7-25 14:26:36
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Handler {
    using System;
    using System.Web;
    using NDWR.Web;

    /// <summary>
    /// RemoteServiceHandler 概要
    /// </summary>
    public class RemoteServiceHandler : IHttpHandler {
        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            ContextSupport cs = new ContextSupport(context);
            //IResponse response = null;
            try {
                cs.ProcessRequest();
                if (cs.Response != null) {
                    cs.Response.WriteResult();
                }
            } catch (NDWRException ex) {
                throw ex;
            } catch (Exception ex) {
                throw ex;
            }
        }

    }
}
