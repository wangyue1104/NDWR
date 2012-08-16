using System;
using System.Collections.Generic;
using System.Reflection;
using NDWR.Web;

namespace NDWR.ServiceStruct {

    /// <summary>
    /// 对外发布的方法
    /// </summary>
    public class ServiceMethod {

        public ServiceMethod() { }

        public ServiceMethod(MethodInfo methodInfo, string name, ServiceMethodParam[] paramList) {
            this.MethodInfo = methodInfo;
            this.Name = name;
            this.Params = paramList;

            this.ReturnType = methodInfo.ReturnType;
            this.MethodType =
                methodInfo.ReturnType == typeof(TransferFile) ?
                MethodType.OutputBinaryStream :
                MethodType.TextStream;
        }
        /// <summary>
        /// 方法元数据信息
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }
        /// <summary>
        /// 对外公开方法名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 返回值类型
        /// </summary>
        public Type ReturnType { get; private set; }
        /// <summary>
        /// 方法类型
        /// </summary>
        public MethodType MethodType { get; private set; }
        /// <summary>
        /// 参数集合
        /// </summary>
        public ServiceMethodParam[] Params { get; private set; }
        /// <summary>
        /// 所属服务
        /// </summary>
        public Service OwnerService { get; set; }
    }


    public enum MethodType {
        /// <summary>
        /// 文本流
        /// </summary>
        TextStream,
        /// <summary>
        /// 输出二进制流
        /// </summary>
        OutputBinaryStream,
        /// <summary>
        /// 输入二进制流
        /// </summary>
        InputBinaryStream
    }
}
