//-----------------------------------------------------------------------------------------
//   <copyright  file="NullResponse.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-8-8 10:42:52
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

using NDWR.InvocationManager;
namespace NDWR.Web {

    /// <summary>
    /// NullResponse 概要
    /// </summary>
    public class NullResponse : IResponse{
        public System.Web.HttpResponse Response { get; set; }

        public InvocationBatch InvokeBatch { get; set; }

        public void WriteResult() {
            Response.Clear();
            Response.ContentType = "text/xml";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.Write(
                string.Format("{\"Name\":\"{0}\",\"Message\" : \"系统发生未知错误\"}",SystemError.UnKnown.ToString())
            );
        }
    }
}
