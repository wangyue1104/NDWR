using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using RemoteService;
using RemoteEntity;
namespace NDWR.ByteCode {



    //public class RemoteDemo_0_ServiceProxy : IServiceProxy {

    //    private RemoteService.RemoteDemo service = new RemoteService.RemoteDemo();

    //    public IServiceProxy Instance {
    //        get {
    //            return new RemoteDemo_0_ServiceProxy();
    //        }
    //    }
    //    public object FuncSwitch(string methodName, object[] paramlst) {
    //        object ret = null;
    //        switch (methodName) {
    //            case "HelloWold": {
    //                    ret = service.HelloWold();
    //                }
    //                break;
    //            case "PubMethod": {
    //                    ret = service.PubMethod((System.Int32)paramlst[0]);
    //                }
    //                break;
    //            case "PubMethod1": {
    //                ret = service.PubMethod2((System.Int32)paramlst[0], (Entity)paramlst[1]);
    //                }
    //                break;
    //            case "PubMethodEntity": {
    //                ret = service.PubMethodEntity((Entity)paramlst[0]);
    //                }
    //                break;
    //        }
    //        return ret;
    //    }
    //}


    //public class RemoteDemo_1_ServiceProxy : RemoteDemo, IServiceProxy {

    //    public IServiceProxy Instance {
    //        get {
    //            return new RemoteDemo_1_ServiceProxy();
    //        }
    //    }
    //    public object FuncSwitch(string methodName, object[] paramlst) {
    //        object ret = null;
    //        switch (methodName) {
    //            case "HelloWold": {
    //                    ret = base.HelloWold();
    //                }
    //                break;
    //            case "PubMethod": {
    //                    ret = base.PubMethod((System.Int32)paramlst[0]);
    //                }
    //                break;
    //            case "PubMethod2": {
    //                    ret = base.PubMethod2((System.Int32)paramlst[0], (Entity)paramlst[1]);
    //                }
    //                break;
    //            case "PubMethodEntity": {
    //                ret = base.PubMethodEntity((Entity)paramlst[0]);
    //                }
    //                break;
    //            case "DataList": {
    //                ret = base.DataList((int)paramlst[0]);
    //                }
    //                break;
    //            case "DataTable": {
    //                ret = base.DataTable((DataTableParameter)paramlst[0]);
    //                }
    //                break;
    //        }
    //        return ret;
    //    }
    //}


    //public class RemoteDemo_2_ServiceProxy : RemoteDemo, IServiceProxy {
    //    public IServiceProxy Instance {
    //        get {
    //            return new RemoteDemo_1_ServiceProxy();
    //        }
    //    }
    //    public object FuncSwitch(string methodName, object[] paramlst) {
    //        if (methodName == "No1") {
    //            base.No1();
    //            return null;
    //        } else if (methodName == "HelloWold") {
    //            return base.HelloWold();
    //        } else if (methodName == "PubMethod") {
    //            return base.PubMethod((System.Int32)paramlst[0]);
    //        } else if (methodName == "PubMethod2") {
    //            return base.PubMethod2((System.Int32)paramlst[0], (Entity)paramlst[1]);
    //        }
    //        return null;
    //    }
    //}

    delegate object CreatorFunc(int i,int j);


    public interface IServiceProxyX {
        IServiceProxyX Instance { get; }
        object FuncSwitch(int methodId, object[] paramlst);
    }

    public class RemoteDemo_3_ServiceProxy : RemoteDemo, IServiceProxyX {

        public object Creator(int i ,int j) {
            CreatorFunc t = (int ii,int jj) => {
                return new object();
            };
            return t(i,j);
        }

        public IServiceProxyX Instance {
            get {
                return new RemoteDemo_3_ServiceProxy();
            }
        }
        public object FuncSwitch(int methodId, object[] paramlst) {

            if (methodId == 1) {
                base.No1();
                return null;
            } else if (methodId == 2) {
                return base.HelloWold();
            } else if (methodId == 3) {
                return base.PubMethod((System.Int32)paramlst[0]);
            } else if (methodId == 4) {
                return (object)base.PubMethod2((System.Int32)paramlst[0], (Entity)paramlst[1]);
            }
            return null;
        }
    }
}