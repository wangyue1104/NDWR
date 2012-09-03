using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using NDWR.ServiceStruct;

namespace NDWR.ByteCode {

    public class ServiceProxyByteCode {

        private static int n = 0; // 防止类型名重复
        private static AssemblyBuilder builder = null;
        private static ModuleBuilder moduleBuilder = null;

        private const string classNameSuffix = "ServiceProxy";

        private Service serviceMetaData;
        private string proxyClassName;

        public ServiceProxyByteCode(Service serviceMetaData) {
            this.serviceMetaData = serviceMetaData;

            this.proxyClassName = string.Format("{0}{1}{2}",  // DemoServiceProxy1
                serviceMetaData.ServiceType.Name, 
                classNameSuffix, 
                ServiceProxyByteCode.n++);

            if (moduleBuilder == null) {
                ServiceProxyByteCode.buildModule();
            }
        }
        /// <summary>
        /// 创建模块
        /// </summary>
        private static void buildModule() {
            // 获取当前域
            AppDomain ad = System.Threading.Thread.GetDomain();
            // 命名程序集
            AssemblyName name = new AssemblyName();
            name.Name = "NDWRServiceProxyAssembly";
            // 创建程序集 放到内存中
            ServiceProxyByteCode.builder = ad.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run); 
            // 定义一个模块（module）
            ServiceProxyByteCode.moduleBuilder = builder.DefineDynamicModule("NDWRProxyModel");
        }

        public static void SaveAssembly() {
            //builder.Save(@"NDWRProxyModel.dll");
        }

        /// <summary>
        /// 创建代理类
        /// </summary>
        /// <returns></returns>
        public IServiceProxy BuildProxy() {
            TypeBuilder theClass = buildType();
            ConstructorBuilder ctor = BuildMethod_ctor(theClass);
            BuildMethodget_Instance(theClass, ctor);
            buildMethodFuncSwitch(theClass);
            
            Type MathOpsClass = theClass.CreateType();// MathOpsClass.GetConstructor(System.Type.EmptyTypes);
            IServiceProxy proxy = Activator.CreateInstance(MathOpsClass) as IServiceProxy;
            return proxy;
        }

        private Type BuildProxyType() {
            TypeBuilder theClass = buildType();
            ConstructorBuilder ctor = BuildMethod_ctor(theClass);
            BuildMethodget_Instance(theClass, ctor);
            buildMethodFuncSwitch(theClass);

            return theClass.CreateType();// MathOpsClass.GetConstructor(System.Type.EmptyTypes);
        }

        public Func<IServiceProxy> ProxyCreateFunc() {
            Type type = BuildProxyType();
            return Expression.Lambda<Func<IServiceProxy>>(Expression.New(type)).Compile();
		}

        /// <summary>
        /// 创建类型
        /// </summary>
        /// <returns></returns>
        private TypeBuilder buildType() {
            // 创建一个类型   
            TypeBuilder theClass = moduleBuilder.DefineType(proxyClassName, 
                TypeAttributes.Public & 
                TypeAttributes.Class & 
                TypeAttributes.Sealed);
            theClass.SetParent(serviceMetaData.ServiceType); // 基类
            theClass.AddInterfaceImplementation(typeof(IServiceProxy)); // 继承接口 
            return theClass;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private ConstructorBuilder BuildMethod_ctor(TypeBuilder type) {
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.HideBySig;
            return type.DefineDefaultConstructor(methodAttributes);
        }

        /// <summary>
        /// 属性
        /// IServiceProxy Instance{get;} 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private MethodBuilder BuildMethodget_Instance(TypeBuilder type, ConstructorBuilder ctor) {
            // 定义属性
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.Virtual
                | System.Reflection.MethodAttributes.Final
                | System.Reflection.MethodAttributes.HideBySig
                | System.Reflection.MethodAttributes.NewSlot;
            MethodBuilder method = type.DefineMethod("get_Instance", methodAttributes);

            // 设置返回值
            method.SetReturnType(typeof(IServiceProxy));
            
            ILGenerator gen = method.GetILGenerator();
            // 创建当前实例
            gen.Emit(OpCodes.Newobj, ctor);
            gen.Emit(OpCodes.Ret);
            
            return method;
        }

        /// <summary>
        /// 创建代理分发方法FuncSwitch
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private MethodBuilder buildMethodFuncSwitch(TypeBuilder type) {
            // 定义方法
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.Virtual
                | System.Reflection.MethodAttributes.Final
                | System.Reflection.MethodAttributes.HideBySig
                | System.Reflection.MethodAttributes.NewSlot;
            MethodBuilder method = type.DefineMethod("FuncSwitch", methodAttributes);
            // 设置返回值
            method.SetReturnType(typeof(Object));
            // 设置参数集合
            method.SetParameters(typeof(Int32), typeof(Object[]));
            // 生成方法il指令
            ilGenBuilder(method);

            return method;
        }

        /// <summary>
        /// 生成代理分发方法体
        /// </summary>
        /// <param name="method"></param>
        private void ilGenBuilder(MethodBuilder method) {
            int i, j;
            ILGenerator gen = method.GetILGenerator();

            gen.DeclareLocal(typeof(object));

            // 方法元数据
            ServiceMethod[] methodMd = serviceMetaData.PublicMethod;
            int methodCount = methodMd.Length; // 方法数
            Label[] labels = new Label[methodCount];
            for (i = 0; i < labels.Length; i++) {
                labels[i] = gen.DefineLabel();
            }

            for (i = 0; i < methodCount; i++) {
                if (i > 0) {
                    gen.MarkLabel(labels[i - 1]);
                }
                // 比较参数中 int值 与 对应方法标识 是否相等
                gen.Emit(OpCodes.Ldarg_1);
                gen.Emit(OpCodes.Ldc_I4, methodMd[i].Id);
                gen.Emit(OpCodes.Bne_Un_S, labels[i]);
                // 将当前所属实体压入计算栈
                gen.Emit(OpCodes.Ldarg_0);

                // 压入计算堆栈参数
                ServiceMethodParam[] paramMd = methodMd[i].Params;
                if (paramMd != null && paramMd.Length > 0) {

                    for (j = 0; j < paramMd.Length; j++) {
                        // 加载参数2数组索引j处值
                        gen.Emit(OpCodes.Ldarg_2);
                        gen.Emit(OpCodes.Ldc_I4, j);
                        gen.Emit(OpCodes.Ldelem_Ref);
                        // 类型转换
                        if (paramMd[j].ParamType.IsValueType) { // 值类型
                            gen.Emit(OpCodes.Unbox_Any, paramMd[j].ParamType);
                        } else { // 引用类型
                            gen.Emit(OpCodes.Castclass, paramMd[j].ParamType);
                        }
                    }
                }
                // 调用目标方法
                gen.Emit(OpCodes.Call, methodMd[i].MethodInfo);
                

                // 如果方法返回类型为void ，则把null压入栈返回 属于特殊处理，不能直接返回void
                if (methodMd[i].ReturnType == typeof(void)) {
                    gen.Emit(OpCodes.Ldnull);
                }else if (methodMd[i].ReturnType.IsValueType) { // 如果是值类型 进行装箱操作
                    gen.Emit(OpCodes.Box, methodMd[i].ReturnType);
                    gen.Emit(OpCodes.Stloc_0);
                    gen.Emit(OpCodes.Ldloc_0);
                }
                gen.Emit(OpCodes.Ret);
            }
            // 默认返回值null
            gen.MarkLabel(labels[methodCount-1]);
            gen.Emit(OpCodes.Ldnull);
            gen.Emit(OpCodes.Ret);
        }

    }
}
