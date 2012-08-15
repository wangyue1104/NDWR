//-----------------------------------------------------------------------------------------
//   <copyright  file="XMLServiceScanner.cs">
//      所属项目：NDWR.ServiceScanner
//      创 建 人：王跃
//      创建日期：2012-7-25 14:31:10
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.ServiceScanner {
    using System;
    using NDWR.ServiceStruct;

    /// <summary>
    /// XMLServiceScanner 概要
    /// </summary>
    public class XMLServiceScanner : IServiceScanner {



        public string[] Azzembly {
            get { throw new NotImplementedException(); }
        }

        Service[] IServiceScanner.Services {
            get { throw new NotImplementedException(); }
        }
    }
}
