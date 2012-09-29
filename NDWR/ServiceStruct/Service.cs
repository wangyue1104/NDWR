//-----------------------------------------------------------------------------------------
//   <copyright  file="Service.cs">
//      所属项目：NDWR.ServiceStruct
//      创 建 人：王跃
//      创建日期：2012-7-24 15:02:14
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.ServiceStruct {
    using System;
    using NDWR.ByteCode;
    using NDWR.JavaScript;



    /// <summary>
    /// 对外发布的服务
    /// </summary>
    public class Service  {

        private IServiceProxy proxy = null;
        private object obj_lock = new object();

        public Service(Type serviceType, string name, bool singleton, ServiceMethod[] publicMethods) {
            this.ServiceType = serviceType;
            this.Name = name;
            this.Singleton = singleton;
            this.PublicMethod = publicMethods;

            //this.ServiceProxy = new ServiceProxyByteCode(this).BuildProxy();
            this.ProxyFunc = new ServiceProxyByteCode(this).ProxyCreateFunc();
            //this.proxy = this.Singleton ? null : this.ProxyFunc();

            this.JavaScript = RemoteServiceScript.Instance.BuildServiceJS(this);

            foreach (ServiceMethod method in this.PublicMethod) {
                method.OwnerService = this;
            }
        }

        /// <summary>
        /// 方法标识ID,用于检索
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 服务类类型
        /// </summary>
        public Type ServiceType { get; private set; }
        /// <summary>
        /// 服务发布名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 该服务生命周期为单例
        /// </summary>
        public bool Singleton { get; private set; }
        /// <summary>
        /// 服务对应方法选择器
        /// </summary>
        public IServiceProxy ServiceProxy {
            get {
                if (!Singleton) {
                    return ProxyFunc();
                }
                // 单例类
                if (this.proxy == null) {
                    lock (this) {
                        if (this.proxy == null) {
                            this.proxy = this.ProxyFunc();
                        }
                    }
                }
                return this.proxy;
            }
        }
        /// <summary>
        /// 代理类创建表达式数
        /// </summary>
        public Func<IServiceProxy> ProxyFunc { get; private set; }
        /// <summary>
        /// 服务公开方法
        /// </summary>
        public ServiceMethod[] PublicMethod { get; private set; }


        /// <summary>
        /// 该服务生成的JS脚本
        /// </summary>
        public string JavaScript { get; private set; }
    }

}
