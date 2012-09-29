using System;
using NDWR.InvocationManager;

namespace NDWR.MethodInterceptor {

    public class ExceptionInterceptor : Interceptor {
        public void Init() {
        }

        public void Intercept(Invocation methodInvoke) {
            try {
                methodInvoke.Invoke();
            } catch (Exception ex) {
                methodInvoke.SystemErrors.Add(
                    new RspError(SystemError.ServiceException, ex.Message)
                );
                //throw ex;
            }
        }

        public void Destroy() {
        }
    }

}
