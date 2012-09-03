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
    using System.IO;
    using System.Text;
    using System.Web;

    /// <summary>
    /// 文件传输类
    /// 用于用户对二进制流输入输出时标识
    /// </summary>
    public class TransferFile {

        public const string EXCEL = "application/vnd.ms-excel";
        public const string ZIP = "application/x-zip-compressed";
        public readonly string ID;
        /// <summary>
        /// 默认构造
        /// </summary>
        public TransferFile() {
            FileName = string.Empty;
            ContentType = string.Empty;
            ContentEncoding = Encoding.UTF8;

            ID = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// 文件下载指定流构造
        /// </summary>
        /// <param name="dataStream"></param>
        public TransferFile(Stream dataStream) {
            this.FileName = string.Empty;
            this.ContentType = string.Empty;
            this.ContentEncoding = Encoding.UTF8;
            this.DataStream = dataStream;
        }
        /// <summary>
        /// 文件上传构造
        /// </summary>
        /// <param name="postFile"></param>
        public TransferFile(HttpPostedFile postFile) {
            this.FileName = postFile.FileName;
            this.ContentType = postFile.ContentType;
            this.DataStream = postFile.InputStream;
            this.ContentLength = postFile.ContentLength;
            this.PostFile = postFile;
        }


        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string ContentType { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding ContentEncoding { get; set; }
        /// <summary>
        /// 文件长度
        /// </summary>
        public long ContentLength { get; set; }
        /// <summary>
        /// 文件操作流
        /// </summary>
        public Stream DataStream { get; private set; }
        /// <summary>
        /// 文件数据
        /// </summary>
        public Byte[] DataBuffer { get; set; }
        /// <summary>
        /// 上传时的文件传输访问
        /// </summary>
        public HttpPostedFile PostFile { get; private set; }

        /// <summary>
        /// 获取服务器路径
        /// </summary>
        /// <param name="virPath">虚拟目录 [eg:"~/Upload"]</param>
        /// <returns></returns>
        public string GetPath(string virPath) {
            return HttpContext.Current.Server.MapPath(virPath);
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="virPath"></param>
        /// <param name="fileName"></param>
        public void SaveAs(string virPath,string fileName) {
            PostFile.SaveAs(GetPath(virPath) + "\\" + fileName);
            //string path = GetPath(virPath) + "\\" + fileName;
            ////创建文件流
            //    FileStream myFs = new FileStream(path, FileMode.Create);

            //StreamWriter sw = new StreamWriter(PostFile.InputStream);
            //sw.Write(
        }
    }

}
