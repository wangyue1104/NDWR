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

        private string _assembly;

        public AttributeServiceScanner(string assembly) {
            this._assembly = assembly;
            Azzembly = new string[] { assembly };
            Services = new ServiceScanner(this).ScanService();
        }


        public Service[] Services { get; private set; }

        public string[] Azzembly { get; private set; }


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

                foreach (Type type in types) {// 遍历所有类型
                    object[] attrs = type.GetCustomAttributes(false); // 获取注解
                    Service service = owner.matchingAttribute<Service>(attrs, typeof(RemoteServiceAttribute), attribute => {
                        RemoteServiceAttribute ajaxServiceAttr = (RemoteServiceAttribute)attribute;

                        IList<ServiceMethod> methods = methodScanner.ScanMethod(type);
                        if (methods == null || methods.Count == 0) {
                            return null;
                        }

                        return new Service(
                            type,
                            string.IsNullOrEmpty(ajaxServiceAttr.Name) ? type.Name : ajaxServiceAttr.Name,
                            ajaxServiceAttr.Singleton, 
                            methods.ToArray()
                        );
                    });
                    if (service != null) {
                        serviceList.Add(service);
                    }
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
                ParamScanner paramScanner = new ParamScanner();
                foreach (MethodInfo method in methods) {
                    if (!isLegalMethod(method)) { // 不是有效方法
                        continue;
                    }

                    object[] attrs = method.GetCustomAttributes(false);// 获取注解
                    ServiceMethod serviceMethod = owner.matchingAttribute<ServiceMethod>(attrs, typeof(RemoteMethodAttribute), attribute => {
                        RemoteMethodAttribute ajaxMethodAttr = (RemoteMethodAttribute)attribute;
                        ServiceMethod sm = new ServiceMethod(
                            method,
                            string.IsNullOrEmpty(ajaxMethodAttr.Name) ? method.Name : ajaxMethodAttr.Name,
                            paramScanner.ScanParam(method));

                        return sm;
                    }); // 匹配注解
                    if (serviceMethod != null) {
                        actions.Add(serviceMethod);
                    }
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
                foreach (ParameterInfo parameterInfo in parameterInfos) {
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
