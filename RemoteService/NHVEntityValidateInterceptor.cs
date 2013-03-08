//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="NHVEntityValidateInterceptor.cs">
//      所属项目：RemoteService
//      创 建 人：王跃
//      创建日期：2012-8-9 8:17:36
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace RemoteService {
    using System.Collections;
    using System.Collections.Generic;
    using NDWR;
    using NDWR.InvocationManager;
    using NDWR.MethodInterceptor;
    using NDWR.ServiceStruct;
    using NHibernate.Validator.Engine;

    /// <summary>
    /// NHV实体验证拦截器
    /// </summary>
    public class NHVEntityValidateInterceptor : Interceptor {
        public void Init() {
        }

        public void Intercept(Invocation methodInvoke) {
            IList<ServiceMethodParam> paramList = methodInvoke.MethodMetaData.Params;
            int index = 0;
            foreach (ServiceMethodParam param in paramList) {
                if (param.TypeCategory == TypeCategory.EntityType) {
                    object retValue = methodInvoke.ParamValues[index].TarValue;
                    InvalidValue[] msgs = NHVHelper.Instance.Validate(retValue);
                    //if (!NHVHelper.Instance.IsValid(retValue)) {
                    if (msgs.Length > 0) {
                        foreach (InvalidValue iv in msgs) {
                            methodInvoke.SystemErrors.Add(
                                new RspError(iv.PropertyName, iv.Message));
                        }
                        return;
                    }
                }
                index++;
            }
            methodInvoke.Invoke();
        }

        public void Destroy() {
        }
    }
}
