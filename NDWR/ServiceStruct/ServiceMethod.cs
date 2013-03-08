using System;
using System.Collections.Generic;
using System.Reflection;
using NDWR.Validator;
using NDWR.Web;
using NDWR.Attributes;

namespace NDWR.ServiceStruct {

    /// <summary>
    /// 对外发布的方法
    /// </summary>
    public class ServiceMethod {

        public ServiceMethod(int id,MethodInfo methodInfo, string name, ServiceMethodParam[] paramList,CustomAttribute[] customAttrs) {
            this.Id = id;
            // 方法反射元数据信息
            this.MethodInfo = methodInfo;
            this.Name = name;
            // 参数信息列表
            this.Params = paramList;
            // 返回值类型
            this.ReturnType = methodInfo.ReturnType;
            // 方法类型
            this.OutputType = TypeHelper.GetTypeCategory(this.ReturnType);

            this.customAttrs = customAttrs;
        }
        /// <summary>
        /// 方法元数据信息
        /// </summary>
        public MethodInfo MethodInfo { get; private set; }
        /// <summary>
        /// 方法标识ID,用于检索
        /// </summary>
        public int Id { get; set; }
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
        public TypeCategory OutputType { get; private set; }
        /// <summary>
        /// 参数集合
        /// </summary>
        public ServiceMethodParam[] Params { get; private set; }
        /// <summary>
        /// 所属服务
        /// </summary>
        public Service OwnerService { get; set; }

        /// <summary>
        /// 自定义特性
        /// </summary>
        private CustomAttribute[] customAttrs = null;
        public T GetCustomAttr<T>() where T: CustomAttribute {
            if (customAttrs == null) {
                return null;
            }
            Type type = typeof(T);
            for (int i = 0; i < customAttrs.Length; i++) {
                if (customAttrs[i].GetType() == type) {
                    return (T)customAttrs[i];
                }
            }
            return null;
        }
    
    }

}
