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
    using System.Collections;
    using System.Collections.Generic;
    using NDWR.Config;
    using NDWR.ServiceStruct;
    using NDWR.Validator;
    using NDWR.Web;
    using NDWR.InvocationManager;
    using NDWR.Util;

    /// <summary>
    /// SimpleParamConvertInterceptor 概要
    /// </summary>
    public class ParamConvertInterceptor : Interceptor {
        public void Init() {
        }

        public void Intercept(Invocation methodInvoke) {

            var paramValues = methodInvoke.ParamValues; // 用户输入参数源信息
            var paramMetas = methodInvoke.MethodMetaData.Params; // 服务端方法参数信息
            // 参数个数映射不符
            if (paramValues.Length != paramMetas.Length) { // 两个参数理论不会为null
                methodInvoke.SystemErrors.Add(new RspError() {
                    Name = SystemError.NoMatchParam.ToString(),
                    Message = "参数不匹配"
                });
                return;
            }
            // 设置参数
            if (!setParamValue(methodInvoke)) {
                return ;
            }
            // 执行目标方法
            methodInvoke.Invoke();
        }

        public void Destroy() {
        }

        private bool setParamValue(Invocation methodInvoke) {
            // 执行快照中的参数信息
            ParamItem[] valParams = methodInvoke.ParamValues;
            // 参数元数据信息
            ServiceMethodParam[] mdParams = methodInvoke.MethodMetaData.Params;
            // 参数个数
            int paramLength = mdParams.Length;
            // 错误信息集合
            IList<RspError> systemErrors = methodInvoke.SystemErrors;
            // 循环体用到临时变量
            ParamItem valParam;
            ServiceMethodParam mdParam;
            // 转换后的值
            object retValue;

            for (int i = 0; i < paramLength; i++) {
                valParam = valParams[i];
                mdParam = mdParams[i];
                
                try {
                    retValue = GlobalConfig.Instance.JsonSerializer.Deserialize(
                        valParam.SrcValue,
                        mdParam.TypeCategory == TypeCategory.BinaryType ? typeof(string) : mdParam.ParamType);
                } catch {
                    systemErrors.Add(
                        new RspError(SystemError.ParamConvertError, "参数转换错误：" + mdParam.Name)
                    );
                    //throw ex;
                    return false;
                }

                if (mdParam.TypeCategory == TypeCategory.BinaryType) { // 输入流方式，则返回输入流类型值
                    retValue = methodInvoke.Request.GetFlie(retValue.ToString());
                }
                // 转换成功 进行赋值
                valParam.TarValue = retValue;
            }

            return true;
        }

    }
}
