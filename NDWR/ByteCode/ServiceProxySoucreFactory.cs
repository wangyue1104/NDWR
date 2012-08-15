//-----------------------------------------------------------------------------------------
//   <copyright  file="ServiceAdapterSoucreFactory.cs">
//      所属项目：NDWR.ByteCode
//      创 建 人：王跃
//      创建日期：2012-7-24 15:32:36
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.ByteCode {
    using System;
    using System.CodeDom.Compiler;
    using System.Text;
    using NDWR.ServiceStruct;

    /// <summary>
    /// ServiceAdapterSoucreFactory 概要
    /// </summary>
    public class ServiceProxySoucreFactory {
        private const string defaultNamespace = "NDWR.ByteCode";
        private const string classNameSuffix = "ServiceProxy";
        private static int n = 0; // 防止类型名重复

        private Service serviceMetaData;

        private string className;

        public ServiceProxySoucreFactory(Service serviceMetaData) {
            this.serviceMetaData = serviceMetaData;

            this.className = string.Format("{0}_{1}_{2}", serviceMetaData.ServiceType.Name, n++, classNameSuffix);
        }
        /// <summary>
        /// 构建实体属性设置器
        /// </summary>
        /// <returns></returns>
        public IServiceProxy Build() {

            // 创建 C# 编译器
            CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
            // 创建编译参数
            CompilerParameters options = new CompilerParameters();

            // 添加对 System.dll 的引用
            options.ReferencedAssemblies.Add("System.dll");
            // 添加对 System.Data.dll 的引用
            options.ReferencedAssemblies.Add("System.Data.dll");
            // 添加对该项目的引用
            options.ReferencedAssemblies.Add(this.GetType().Assembly.Location);
            // 添加对实体项目的引用
            options.ReferencedAssemblies.Add(serviceMetaData.ServiceType.Assembly.Location);
            // 只在内存中编译
            options.GenerateInMemory = true;

            string sourceCode = SourceBuild();
            // 编译并获取编译结果
            CompilerResults compileResult = provider.CompileAssemblyFromSource(options, new string[] { sourceCode });

            //输出编译错误
            StringBuilder sb = new StringBuilder("Output:\n");
            foreach (string output in compileResult.Output) {
                sb.Append(output + "\n");
            }
            sb.Append("\nErrors:\n");
            foreach (CompilerError error in compileResult.Errors) {
                sb.Append(error.ErrorText + "\n");
            }
            // 编译失败则抛出异常
            if (compileResult.NativeCompilerReturnValue != 0) {
                throw new Exception("Compile Failed\r\n" + sb.ToString());
            }

            // 创建设置器
            object entityHandler = compileResult.CompiledAssembly.CreateInstance(defaultNamespace + "." + className);

            return (IServiceProxy)entityHandler;
        }

        private string SourceBuild() {
            Type type = serviceMetaData.ServiceType;
            
            StringBuilder sb = new StringBuilder("");
            sb.Append("using System;\r\n");
            sb.Append("using System.Collections.Generic;\r\n");
            sb.Append("using System.Text;\r\n");
            sb.Append("using System.Data;\r\n");
            sb.AppendFormat("namespace {0} {{\r\n", defaultNamespace);
            sb.AppendFormat("   public class {0} : IServiceProxy {{\r\n", className);
            sb.AppendFormat("       private {0} service = new {0}(); \r\n", type.FullName);
            sb.Append("     public IServiceProxy Instance {\r\n");
            sb.Append("         get{\r\n");
            sb.AppendFormat("       return new {0}();\r\n", className);
            sb.Append("         }\r\n");
            sb.Append("     }\r\n");
            sb.AppendFormat("   public object FuncSwitch(string methodName,object[] paramlst){{\r\n", type.FullName);
            sb.Append("            object ret = null; \r\n");
            sb.Append("            switch(methodName){\r\n");
            foreach (ServiceMethod method in serviceMetaData.PublicMethod) {
                sb.AppendFormat("        case \"{0}\" : {{\r\n", method.Name);
                if ((method.Params == null) || (method.Params.Length == 0)) {
                    sb.AppendFormat("        ret = service.{0}();\r\n", method.MethodInfo.Name);
                } else {
                    string paramStrs = string.Empty;
                    int index = 0;
                    foreach (ServiceMethodParam param in method.Params) {
                        paramStrs = paramStrs + string.Format("({0})paramlst[{1}],", param.ParamType.FullName, index++);
                    }
                    paramStrs = paramStrs.TrimEnd(new char[] { ',' });
                    sb.AppendFormat("        ret = service.{0}({1});\r\n", method.MethodInfo.Name, paramStrs);
                }
                sb.Append("   }\r\n");
                sb.Append("   break;\r\n");
            }
            sb.Append("            }\r\n");
            //sb.AppendFormat("      ErrorMessage = service.ErrorMessage;\r\n", type.FullName);
            sb.Append("            return ret;\r\n");
            sb.Append("         }\r\n");

            //sb.Append("     public IList<string> ErrorMessage { get; set; }");

            sb.Append("     }\r\n");
            sb.Append("}\r\n");
            return sb.ToString();
        }
    }
}

