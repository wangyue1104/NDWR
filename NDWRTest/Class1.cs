using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using NDWR.ByteCode;
using RemoteService;
using NUnit.Framework;
using System.Runtime.ConstrainedExecution;
using RemoteEntity;
using NDWR.Util;

namespace NDWRTest {

    [TestFixture]
    public class EmitCodeTest {

        [Test]
        public void BuildTest() {
            new EmitCodeDemo().ddd();
        }

        [Test]
        public void TTest() {
            object[] d = new object[1];
            d[0] = null;
            dsd(d);
        }

        public void dsd(object[] d) {
            if (d[0] != null) {
                System.Console.WriteLine(d[0]);
            }
        }

        [Test]
        public void KitTest() {

            string[] ss = {"s","d"};
            Kit.Each<string>(ss, (item, index) => {
                System.Console.WriteLine(item);
            });


            IList<string> list = new List<string>() { "a", "x" };
            Kit.Each<string>(list, (item, index) => {
                System.Console.WriteLine(item);
            });
        }
    }


    public class EmitCodeDemo {

        private Type srcType = typeof(RemoteDemo);
        private ConstructorBuilder ctor;

        /// <summary>
        /// 生成srcType代理类
        /// </summary>
        /// <param name="srcType"></param>
        public void ddd() {
            string proxyTypeName = srcType.Name + "Proxy";
            // 获取当前域
            AppDomain ad = System.Threading.Thread.GetDomain();
            // 命名程序集
            AssemblyName name = new AssemblyName();
            name.Name = "NDWRProxy";
            // 创建程序集 放到内存中
            AssemblyBuilder builder = ad.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave, @"F:\ArchiveDocument");
            // 定义一个模块（module）
            ModuleBuilder mb = builder.DefineDynamicModule("NDWRProxyModel");

            // 创建一个类型   
            TypeBuilder theClass = mb.DefineType(proxyTypeName, TypeAttributes.Public & TypeAttributes.Class & TypeAttributes.Sealed);
            theClass.SetParent(srcType); // 基类
            theClass.AddInterfaceImplementation(typeof(IServiceProxy)); // 继承接口 

            // 调用DefineMethod定义方法
            BuildMethod_ctor1(theClass);
            BuildMethodget_Instance(theClass);
            BuildMethodFuncSwitch(theClass);

            Type MathOpsClass = theClass.CreateType();

            IServiceProxy MathOpsInst = Activator.CreateInstance(MathOpsClass) as IServiceProxy;

            //object rtObj = MathOpsInst.FuncSwitch("HelloWold", new object[] { });

            //Console.WriteLine("Sum: {0}", rtObj.ToString());
        }

        public MethodBuilder BuildMethod_ctor(TypeBuilder type) {
            // Declaring method builder                           
            // Method attributes
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.HideBySig;
            MethodBuilder method = type.DefineMethod(".ctor", methodAttributes);
            // 获取被代理类构造方法
            ConstructorInfo ctor1 = typeof(Object).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { },
                null
                );
            // Setting return type
            method.SetReturnType(typeof(void));
            // Adding parameters
            ILGenerator gen = method.GetILGenerator();
            // Writing body
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, ctor1);
            gen.Emit(OpCodes.Ret);
            // finished
            return method;
        }


        public void BuildMethod_ctor1(TypeBuilder type) {
            // Declaring method builder
            // Method attributes
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.HideBySig;
            ctor = type.DefineDefaultConstructor(methodAttributes);

        }

        /// <summary>
        /// IServiceProxy Instance{get;} 属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public MethodBuilder BuildMethodget_Instance(TypeBuilder type) {
            // Declaring method builder
            // Method attributes
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.Virtual
                | System.Reflection.MethodAttributes.Final
                | System.Reflection.MethodAttributes.HideBySig
                | System.Reflection.MethodAttributes.NewSlot;
            MethodBuilder method = type.DefineMethod("get_Instance", methodAttributes);
            // Preparing Reflection instances

            // 设置返回值
            method.SetReturnType(typeof(IServiceProxy));
            // Adding parameters
            ILGenerator gen = method.GetILGenerator();
            // Writing body
            gen.Emit(OpCodes.Newobj, ctor);
            gen.Emit(OpCodes.Ret);
            // finished
            return method;
        }


        public MethodBuilder BuildMethodFuncSwitch(TypeBuilder type) {
            // Declaring method builder
            // Method attributes
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.Virtual
                | System.Reflection.MethodAttributes.Final
                | System.Reflection.MethodAttributes.HideBySig
                | System.Reflection.MethodAttributes.NewSlot;
            MethodBuilder method = type.DefineMethod("FuncSwitch", methodAttributes);
            // Preparing Reflection instances
            MethodInfo method_StrEqual = typeof(String).GetMethod(
                "op_Equality",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(String), typeof(String) },
                null
                );
            MethodInfo method_Base1 = srcType.GetMethod(
                "HelloWold",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { },
                null
                );
            MethodInfo method_Base2 = srcType.GetMethod(
                "PubMethod",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[] { typeof(Int32) },
                null
                );
            // Setting return type
            method.SetReturnType(typeof(Object));
            // Adding parameters
            method.SetParameters(
                typeof(String),
                typeof(Object[])
                );

            ILGenerator gen = method.GetILGenerator();
            // 定义局部变量
            LocalBuilder ret = gen.DeclareLocal(typeof(Object));
            LocalBuilder local_temp = gen.DeclareLocal(typeof(String));
            // 定义跳转标签
            Label label59 = gen.DefineLabel();
            Label label35 = gen.DefineLabel();
            Label label44 = gen.DefineLabel();
            // Writing body
            // 初始化变量 ret = null
            gen.Emit(OpCodes.Ldnull);
            gen.Emit(OpCodes.Stloc_0);
            // 讲参数1拷贝到变量
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Dup);
            gen.Emit(OpCodes.Stloc_1);
            // 判断参数1的值是否为空
            gen.Emit(OpCodes.Brfalse_S, label59);
            // 判断变量1中字符串是否等于 "HelloWold"
            gen.Emit(OpCodes.Ldloc_1);
            gen.Emit(OpCodes.Ldstr, "HelloWold");
            gen.Emit(OpCodes.Call, method_StrEqual);
            gen.Emit(OpCodes.Brtrue_S, label35);
            // 判断变量1中字符串是否等于 "PubMethod"
            gen.Emit(OpCodes.Ldloc_1);
            gen.Emit(OpCodes.Ldstr, "PubMethod");
            gen.Emit(OpCodes.Call, method_StrEqual);
            gen.Emit(OpCodes.Brtrue_S, label44);
            gen.Emit(OpCodes.Br_S, label59);

            gen.MarkLabel(label35);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, method_Base1);
            gen.Emit(OpCodes.Stloc_0);
            gen.Emit(OpCodes.Br_S, label59);

            gen.MarkLabel(label44);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Unbox_Any, typeof(Int32));
            gen.Emit(OpCodes.Call, method_Base2);
            gen.Emit(OpCodes.Stloc_0);

            gen.MarkLabel(label59);
            gen.Emit(OpCodes.Ldloc_0);
            gen.Emit(OpCodes.Ret);

            // finished
            return method;
        }


        public MethodBuilder BuildMethodFuncSwitch2(TypeBuilder type) {
            // Declaring method builder
            // Method attributes
            System.Reflection.MethodAttributes methodAttributes =
                  System.Reflection.MethodAttributes.Public
                | System.Reflection.MethodAttributes.Virtual
                | System.Reflection.MethodAttributes.Final
                | System.Reflection.MethodAttributes.HideBySig
                | System.Reflection.MethodAttributes.NewSlot;
            MethodBuilder method = type.DefineMethod("FuncSwitch", methodAttributes);
            // Preparing Reflection instances
            MethodInfo method1 = typeof(RemoteDemo).GetMethod(
                "No1",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{ },
                null
                );
            MethodInfo method2 = typeof(RemoteDemo).GetMethod(
                "HelloWold",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{ },
                null
                );
            MethodInfo method3 = typeof(RemoteDemo).GetMethod(
                "PubMethod",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{ typeof(Int32) },
                null
                );
            MethodInfo method4 = typeof(RemoteDemo).GetMethod(
                "PubMethod2",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                null,
                new Type[]{ typeof(Int32), typeof(Entity) },
                null
                );
            // Setting return type
            method.SetReturnType(typeof(Object));
            // Adding parameters
            method.SetParameters(typeof(Int32), typeof(Object[]));
            // Parameter methodId
            ParameterBuilder methodId = method.DefineParameter(1, ParameterAttributes.None, "methodId");
            // Parameter paramlst
            ParameterBuilder paramlst = method.DefineParameter(2, ParameterAttributes.None, "paramlst");
            ILGenerator gen = method.GetILGenerator();
            // Preparing labels
            Label label12 = gen.DefineLabel();
            Label label23 = gen.DefineLabel();
            Label label42 = gen.DefineLabel();
            Label label69 = gen.DefineLabel();
            // Writing body
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_1);
            gen.Emit(OpCodes.Bne_Un_S, label12);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, method1);
            gen.Emit(OpCodes.Ldnull);
            gen.Emit(OpCodes.Ret);

            gen.MarkLabel(label12);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_2);
            gen.Emit(OpCodes.Bne_Un_S, label23);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Call, method2);
            gen.Emit(OpCodes.Ret);

            gen.MarkLabel(label23);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_3);
            gen.Emit(OpCodes.Bne_Un_S, label42);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Unbox_Any, typeof(Int32));
            gen.Emit(OpCodes.Call, method3);
            gen.Emit(OpCodes.Ret);

            gen.MarkLabel(label42);
            gen.Emit(OpCodes.Ldarg_1);
            gen.Emit(OpCodes.Ldc_I4_4);
            gen.Emit(OpCodes.Bne_Un_S, label69);
            gen.Emit(OpCodes.Ldarg_0);
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Ldc_I4_0);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Unbox_Any, typeof(Int32));
            gen.Emit(OpCodes.Ldarg_2);
            gen.Emit(OpCodes.Ldc_I4_1);
            gen.Emit(OpCodes.Ldelem_Ref);
            gen.Emit(OpCodes.Castclass, typeof(Entity));
            gen.Emit(OpCodes.Call, method4);
            gen.Emit(OpCodes.Ret);

            gen.MarkLabel(label69);
            gen.Emit(OpCodes.Ldnull);
            gen.Emit(OpCodes.Ret);
            // finished
            return method;
        }

 

 



    }

}

/*
L_0000: nop 
L_0001: ldnull 
L_0002: stloc.0 
L_0003: ldarg.1 
L_0004: stloc.2 
L_0005: ldloc.2 
L_0006: brfalse.s L_004c
L_0008: ldloc.2 
L_0009: ldstr "HelloWold"
L_000e: call bool [mscorlib]System.String::op_Equality(string, string)
L_0013: brtrue.s L_0024
L_0015: ldloc.2 
L_0016: ldstr "PubMethod"
L_001b: call bool [mscorlib]System.String::op_Equality(string, string)
L_0020: brtrue.s L_0034
L_0022: br.s L_004c
L_0024: nop 
L_0025: ldarg.0 
L_0026: ldfld class [RemoteService]RemoteService.RemoteDemo NDWR.ByteCode.RemoteDemo_0_ServiceProxy::service
L_002b: callvirt instance string [RemoteService]RemoteService.RemoteDemo::HelloWold()
L_0030: stloc.0 
L_0031: nop 
L_0032: br.s L_004c
L_0034: nop 
L_0035: ldarg.0 
L_0036: ldfld class [RemoteService]RemoteService.RemoteDemo NDWR.ByteCode.RemoteDemo_0_ServiceProxy::service
L_003b: ldarg.2 
L_003c: ldc.i4.0 
L_003d: ldelem.ref 
L_003e: unbox.any int32
L_0043: callvirt instance string [RemoteService]RemoteService.RemoteDemo::PubMethod(int32)
L_0048: stloc.0 
L_0049: nop 
L_004a: br.s L_004c
L_004c: ldloc.0 
L_004d: stloc.1 
L_004e: br.s L_0050
L_0050: ldloc.1 
L_0051: ret 

*/