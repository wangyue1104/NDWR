//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="AuthorityInterceptor.cs">
//      所属项目：RemoteService
//      创 建 人：王跃
//      创建日期：2012-10-19 9:58:09
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace RemoteService {
    using System;
    using System.Data;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using NDWR.MethodInterceptor;
    using NDWR;
    using NDWR.InvocationManager;

    /// <summary>
    /// 权限过滤拦截器
    /// </summary>
    public class AuthorityInterceptor : Interceptor {
        public void Init() {
        }

        public void Intercept(Invocation methodInvoke) {
            AuthorityAttribute d = methodInvoke.MethodMetaData.GetCustomAttr<AuthorityAttribute>();
            if (d == null) {
                methodInvoke.Invoke();
                return;
            }

            methodInvoke.SystemErrors.Add(new RspError(SystemError.NoPermission));
        }

        public void Destroy() {
        }
    }
}
