//-----------------------------------------------------------------------------------------
//   <copyright  file="SimpleParamConvertInterceptor.cs">
//      所属项目：NDWR.MethodInterceptor
//      创 建 人：王跃
//      创建日期：2012-7-26 15:11:27
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.MethodInterceptor {
    using NDWR.Config;
    using NDWR.Validator;

    /// <summary>
    /// SimpleParamConvertInterceptor 概要
    /// </summary>
    public class ParamConvertInterceptor : Interceptor {
        public void Init() {
        }

        public object Intercept(MehtodInvocation methodInvoke) {

            var srcParams = methodInvoke.InvokeInfo.SourceParams;
            var tarParams = methodInvoke.Method.Params;
            // 参数个数映射不符
            if (srcParams != null && tarParams != null && srcParams.Length != tarParams.Length) {
                methodInvoke.InvokeInfo.SystemErrors.Add(new RspError() {
                    Name = SystemError.NoMatchParam.ToString(),
                    Message = "参数不匹配"
                });
                return null;
            }

            object retValue;
            if (srcParams != null) {
                for (int i = 0; i < tarParams.Length; i++) {
                    if (tarParams[i].IsSimplyType) {
                        if (!ValueConvert.ConvertBaseType(srcParams[i], tarParams[i].ParamType, out retValue)) {
                            methodInvoke.InvokeInfo.SystemErrors.Add(
                                new RspError(SystemError.ParamConvertError, "参数转换错误：" + tarParams[i].Name)
                            );
                            return null;
                        }
                        // 转换成功 进行赋值
                        methodInvoke.InvokeInfo.TargetParams[i] = retValue;
                    } else {
                        try {
                            retValue = GlobalConfig.Instance.JsonSerializer.Deserialize(srcParams[i], tarParams[i].ParamType);
                        } catch {
                            methodInvoke.InvokeInfo.SystemErrors.Add(
                                new RspError(SystemError.ParamConvertError, "参数转换错误：" + tarParams[i].Name)
                            );
                            //throw ex;
                            return null;
                        }
                        // 转换成功 进行赋值
                        methodInvoke.InvokeInfo.TargetParams[i] = retValue;
                    }
                }
            }
            return methodInvoke.Invoke();
        }

        public void Destroy() {
        }

    }
}
