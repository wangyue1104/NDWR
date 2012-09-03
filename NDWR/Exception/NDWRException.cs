//-----------------------------------------------------------------------------------------
//   <copyright  file="NDWRException.cs">
//      所属项目：NDWR
//      创 建 人：王跃
//      创建日期：2012-7-25 9:43:27
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR {

    using System;

    /// <summary>
    /// NDWRException 概要
    /// </summary>
    [Serializable]
    public class NDWRException : ApplicationException{

        public NDWRException(string message)
            : base(message) {
        }

        public NDWRException(string message, Exception innerException)
            : base(message, innerException) {

        }
    }
}
