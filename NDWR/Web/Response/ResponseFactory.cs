//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="ResponseFactory.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-9-1 11:47:56
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Web {
    using System;
    using System.Data;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Web;

    /// <summary>
    /// ResponseFactory 概要
    /// </summary>
    public class ResponseFactory {

        public static IResponse Get(string transferMode, HttpResponse response) {
            if (string.IsNullOrEmpty(transferMode) || transferMode == "scriptTag") {
                return new JsonRespose(response);
            } else if (transferMode == "iframe") {
                return new JsonRespose(response, true);
            } else {
                return new NullResponse();
            }
        }
    }
}
