//-----------------------------------------------------------------------------------------
//   <copyright  file="TransferFile.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-8-7 13:25:39
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.Web {
    using System;
    using System.Text;

    /// <summary>
    /// TransferFile 概要
    /// </summary>
    public class TransferFile {

        public const string EXCEL = "application/vnd.ms-excel";
        public const string ZIP = "application/x-zip-compressed";

        public TransferFile() {
            FileName = string.Empty;
            ContentType = string.Empty;
            ContentEncoding = Encoding.UTF8;
        }

        public string FileName { get; set; }

        public Byte[] DataBuffer { get; set; }

        public string ContentType { get; set; }

        public Encoding ContentEncoding { get; set; }
    }

}
