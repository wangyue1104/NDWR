using System;
using NDWR.Validator;

namespace NDWR.ServiceStruct {

    /// <summary>
    /// 方法参数
    /// </summary>
    public class ServiceMethodParam {
        public ServiceMethodParam() { }
        public ServiceMethodParam(string name, Type paramType) {
            this.Name = name;
            this.ParamType = paramType;
            this.IsSimplyType = ValueConvert.IsBaseType(paramType);
        }
        /// <summary>
        /// 参数名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public Type ParamType { get; private set; }
        /// <summary>
        /// 是否为简单类型
        /// </summary>
        public bool IsSimplyType { get; private set; }
    }

}
