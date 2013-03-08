//-----------------------------------------------------------------------------------------
//   <copyright  file="ServiceScanner.cs">
//      所属项目：NDWR.ServiceScanner
//      创 建 人：王跃
//      创建日期：2012-7-25 9:11:56
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.ServiceScanner {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using NDWR.Attributes;
    using NDWR.ServiceStruct;

    /// <summary>
    /// ServiceScanner 概要
    /// </summary>
    public class AttributeServiceScanner : IServiceScanner {
        private static readonly NDWR.Logging.ILog log = NDWR.Logging.LogManager.GetLogger(typeof(AttributeServiceScanner));

        private string _assembly;

        public AttributeServiceScanner(string assembly) {
            this._assembly = assembly;
            Azzembly = new string[] { assembly };
            Services = new ServiceScanner(this).ScanService();
            // 保存代理类
            //NDWR.ByteCode.ServiceProxyByteCode.SaveAssembly();
        }


        public Service[] Services { get; private set; }

        public string[] Azzembly { get; private set; }


        public Service GetService(string serviceName) {
            int length = Services.Length;
            for (int i = 0; i < length; i++) {
                if (Services[i].Name == serviceName) {
                    return Services[i];
                }
            }
            return null;
        }

        public ServiceMethod GetMethod(string serviceName, string methodName) {
            Service service = GetService(serviceName);
            if (service == null) {
                return null;
            }
            ServiceMethod[] methods = service.PublicMethod;
            int length = methods.Length;
            for (int i = 0; i < length; i++) {
                if (methods[i].Name == methodName) {
                    return methods[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 匹配注解
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="attributes">被标注特性集合</param>
        /// <param name="targetType">目标特性</param>
        /// <param name="callBack">回调函数</param>
        /// <returns></returns>
        private T matchingAttribute<T>(object[] attributes, Type targetType, Func<object, T> callBack) {
            if (attributes == null) {
                return default(T);
            }

            foreach (object attribute in attributes) { // 遍历注解
                if (attribute.GetType() == targetType) { // 找到了。。。
                    return callBack(attribute);
                }
            }
            return default(T);
        }


        private class ServiceScanner {

            private AttributeServiceScanner owner;

            public ServiceScanner(AttributeServiceScanner owner) {
                this.owner = owner;
            }

            /// <summary>
            /// 扫描程序集中所有类型
            /// </summary>
            /// <returns></returns>
            private Type[] ScanAssemblyType() {
                try {
                    Assembly assembly = Assembly.Load(owner._assembly);
                    Type[] types = assembly.GetTypes();
                    return types;
                } catch (Exception ex) {
                    throw new NDWRException("扫描程序集异常", ex);
                }
            }

            /// <summary>
            /// 扫描所有类型，获取一个映射结构图
            /// </summary>
            /// <param name="types"></param>
            /// <returns></returns>
            public Service[] ScanService() {
                Type[] types = ScanAssemblyType();
                if (types == null) {
                    return null;
                }
                IList<Service> serviceList = new List<Service>(); // 定义结构图
                MethodScanner methodScanner = new MethodScanner(owner);
                Type type;
                RemoteServiceAttribute ajaxServiceAttr;
                IList<CustomAttribute> customAttr;
                //
                for(int i =0; i< types.Length; i++){
                    type = types[i];
                    object[] attrs = type.GetCustomAttributes(false); // 获取注解
                    if(attrs == null || attrs.Length  == 0){ // 没有特性标识 跳出
                        continue;
                    }

                    ajaxServiceAttr = null;
                    customAttr = new List<CustomAttribute>();
                    foreach (object attribute in attrs) {
                        if (attribute.GetType() == typeof(RemoteServiceAttribute)) {
                            ajaxServiceAttr = (RemoteServiceAttribute)attribute;
                        } else if(attribute.GetType().BaseType == typeof(CustomAttribute)) {
                            customAttr.Add((CustomAttribute)attribute);
                        }
                    }
                    // 如果没有找到服务特性
                    if (ajaxServiceAttr == null) {
                        continue;
                    }
                    // 遍历开放的方法
                    IList<ServiceMethod> methods = methodScanner.ScanMethod(type);
                    if (methods == null || methods.Count == 0) { // 没有找到，跳出
                        continue;
                    }

                    Service service = new Service(
                        i,
                        type,
                        string.IsNullOrEmpty(ajaxServiceAttr.Name) ? type.Name : ajaxServiceAttr.Name,
                        ajaxServiceAttr.Singleton,
                        methods.ToArray(),
                        customAttr.ToArray());

                    serviceList.Add(service);
                }
                
                return serviceList.ToArray();
            }
        }


        private class MethodScanner {

            private AttributeServiceScanner owner;
            public MethodScanner(AttributeServiceScanner owner) {
                this.owner = owner;
            }

            /// <summary>
            /// 扫描Method
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public IList<ServiceMethod> ScanMethod(Type type) {
                MethodInfo[] methods = type.GetMethods(); // 获取成员方法
                IList<ServiceMethod> actions = new List<ServiceMethod>(); // 实例化Action集合
                ParamScanner paramScanner = new ParamScanner(); // 参数扫描
                // 循环变量
                MethodInfo method; // 方法
                RemoteMethodAttribute ajaxMethodAttr; // 特性
                IList<CustomAttribute> customAttr; // 其他自定义特性

                for(int i=0; i<methods.Length; i++){
                    method = methods[i];
                    if (!isLegalMethod(method)) { // 不是有效方法
                        continue;
                    }

                    object[] attrs = method.GetCustomAttributes(false);// 获取注解
                    if (attrs == null || attrs.Length == 0) {
                        continue;
                    }
                    ajaxMethodAttr = null;
                    customAttr = new List<CustomAttribute>();
                    foreach (object attribute in attrs) {
                        if (attribute.GetType() == typeof(RemoteMethodAttribute)) {
                            ajaxMethodAttr = (RemoteMethodAttribute)attribute;
                        } else if (attribute.GetType().BaseType == typeof(CustomAttribute)) {
                            customAttr.Add((CustomAttribute)attribute);
                        }
                    }

                    if (ajaxMethodAttr == null) {
                        continue;
                    }

                    ServiceMethod serviceMethod = new ServiceMethod(
                        i,
                        method,
                        string.IsNullOrEmpty(ajaxMethodAttr.Name) ? method.Name : ajaxMethodAttr.Name,
                        paramScanner.ScanParam(method),
                        customAttr.ToArray()
                    );
                    actions.Add(serviceMethod);
                }
                return actions;
            }

            private bool isLegalMethod(MethodInfo method) {
                if (method.IsAbstract || method.IsStatic || method.IsVirtual) { // 不是有效方法
                    return false ;
                }
                return true;
            }
        }

        private class ParamScanner {


            /// <summary>
            /// 扫描参数
            /// </summary>
            /// <param name="method"></param>
            /// <returns></returns>
            public ServiceMethodParam[] ScanParam(MethodInfo method) {
                // 获取参数
                ParameterInfo[] parameterInfos = method.GetParameters();
                if (parameterInfos == null) {
                    return null;
                }
                //遍历参数
                IList<ServiceMethodParam> paramList = new List<ServiceMethodParam>();
                ParameterInfo parameterInfo;

                for(int i=0; i < parameterInfos.Length; i++){
                    parameterInfo = parameterInfos[i];
                    ServiceMethodParam paramInfo = new ServiceMethodParam(
                        parameterInfo.Name,
                        parameterInfo.ParameterType);
                    paramList.Add(paramInfo);
                }

                return paramList.ToArray();
            }
        }


    }
}
