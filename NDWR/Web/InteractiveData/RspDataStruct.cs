//-----------------------------------------------------------------------------------------
//   <copyright  file="RspDataStruct.cs">
//      所属项目：NDWR
//      创 建 人：王跃
//      创建日期：2012-7-24 9:59:11
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR {
    using System.Collections.Generic;
    using System.ComponentModel;


    public class RspBatchPackage {

        /// <summary>
        /// 本次调用的批次号
        /// </summary>
        public string BatchId { get; set; }

        public IList<RspDataPackage> DataList { get; set; }

    }

    /// <summary>
    /// RspDataStruct 概要
    /// </summary>
    public class RspDataPackage {
        /// <summary>
        /// 本次调用的批次号
        /// </summary>
        public int MethodIndex { get; set; }
        public string Service { get; set; }
        public string Method { get; set; }
        //public IList<RspError> CustomeErrors { get; set; }
        public IList<RspError> SystemErrors { get; set; }
        public object JsonData { get; set; }
    }

    public class RspError {

        public RspError() { }

        public RspError(SystemError error,string message) {
            this.Name = error.ToString();
            this.Message = message;
        }

        public RspError(SystemError error) {
            switch (error) {
                case SystemError.NoPermission: {
                        this.Name = error.ToString();
                        this.Message = "没有权限";
                    }
                    break;
                case SystemError.RunTimeError: {
                        this.Name = error.ToString();
                        this.Message = "运行环境出错";
                    }
                    break;
                case SystemError.SessionTimeout: {
                        this.Name = error.ToString();
                        this.Message = "会话过期";
                    }
                    break;
                case SystemError.ServiceException: {
                        this.Name = error.ToString();
                        this.Message = "服务端方法异常";
                    }
                    break;
                case SystemError.NoMatchService: {
                        this.Name = error.ToString();
                        this.Message = "没有匹配到相应的服务";
                    }
                    break;
                case SystemError.NoMatchParam: {
                        this.Name = error.ToString();
                        this.Message = "没有匹配到参数";
                    }
                    break;
                case SystemError.ParamIndex: {
                        this.Name = error.ToString();
                        this.Message = "参数索引顺序错误";
                    }
                    break;
                default: {
                        this.Name = SystemError.UnKnown.ToString();
                        this.Message = "未知错误";
                    }
                    break;
            }
        }

        public RspError(string name, string message) {
            this.Name = name;
            this.Message = message;
        }

        public string Name { get; set; }
        public string Message { get; set; }
    }


    public enum SystemError {

        /// <summary>
        /// 未知错误
        /// </summary>
        [Description("未知错误")]
        UnKnown,
        /// <summary>
        /// 运行环境出错，级框架自身错误
        /// </summary>
        RunTimeError,
        /// <summary>
        /// 没有权限
        /// </summary>
        [Description("没有权限")]
        NoPermission,
        /// <summary>
        /// 会话过期
        /// </summary>
        [Description("会话过期")]
        SessionTimeout,
        /// <summary>
        /// 服务端方法异常
        /// </summary>
        [Description("服务端方法异常")]
        ServiceException,
        /// <summary>
        /// 没有匹配到相应的服务
        /// </summary>
        [Description("没有匹配到相应的服务")]
        NoMatchService ,
        /// <summary>
        /// 没有匹配到参数
        /// </summary>
        [Description("没有匹配到参数")]
        NoMatchParam,
        /// <summary>
        /// 参数索引顺序错误
        /// </summary>
        [Description("参数索引顺序错误")]
        ParamIndex,

        /// <summary>
        /// 参数转换错误
        /// </summary>
        [Description("参数转换错误")]
        ParamConvertError
    }
}
