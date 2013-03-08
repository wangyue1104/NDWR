using System;
using NDWR.InvocationManager;

namespace NDWR.MethodInterceptor {

    public class ExceptionInterceptor : Interceptor {
        private static readonly NDWR.Logging.ILog log = NDWR.Logging.LogManager.GetLogger(typeof(ExceptionInterceptor).Name);
        public void Init() {
        }

        public void Intercept(Invocation methodInvoke) {
            try {
                methodInvoke.Invoke();
            } catch (Exception ex) {
                methodInvoke.SystemErrors.Add(
                    new RspError(SystemError.ServiceException, ex.Message)
                );
                log.Error("ExceptionInterceptor捕获到异常", ex);
                //throw ex;
            }
        }

        public void Destroy() {
        }
    }

}
