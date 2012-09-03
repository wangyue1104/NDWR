using System;
using NDWR.InvocationManager;

namespace NDWR.MethodInterceptor {

    public class ExceptionInterceptor : Interceptor {
        public void Init() {
        }

        public object Intercept(MethodInvocation methodInvoke) {
            try {
                return methodInvoke.Invoke();
            } catch (Exception ex) {
                methodInvoke.InvokeInfo.SystemErrors.Add(
                    new RspError(SystemError.ServiceException, ex.Message)
                );
                //throw ex;
                return null;
            }
        }

        public void Destroy() {
        }
    }

}
