using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using NUnit.Framework;
using NDWR;
using NDWR.ServiceStruct;
using NDWR.ByteCode;
using NDWR.ServiceScanner;
using NDWR.Attributes;

namespace SmartAjaxTest {
    
    [TestFixture]
    public class JsonDemo {

        [Test]
        public void CallBackMethodJsonStr() {
            CallBackMethod method0 = new CallBackMethod();
            method0.Method = "method0";
            method0.Args = new List<CallBackArg>() { 
                new CallBackArg() { Name = "0"},
                new CallBackArg() { Name = "1"}, 
                new CallBackArg() { Name = "2"}
            };

            CallBackMethod method1 = new CallBackMethod();
            method1.Method = "method1";
            method1.Args = new List<CallBackArg>() { 
                new CallBackArg() { Name = "0"},
                new CallBackArg() { Name = "1"}, 
                new CallBackArg() { Name = "2"}
            };

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<CallBackMethod>));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, new List<CallBackMethod>() { method0, method1 });
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();

            Console.WriteLine(jsonString);
        }

        [Test]
        public void RspJsonStr() {
            IList<RspDataPackage> list = new List<RspDataPackage>(){
                new RspDataPackage(){
                     Method = "", JsonData = "[{\"CustomeErrors\":null,\"Data\":null,\"Method\":\"\",\"SystemErrors\":null},{\"CustomeErrors\":null,\"Data\":null,\"Method\":\"\",\"SystemErrors\":null}]"
                },
                new RspDataPackage(){
                     Method = "", JsonData = "324"
                }
            };

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IList<RspDataPackage>));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, list);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();

            Console.WriteLine(jsonString);
        }


        [Test]
        public void AssemblyScannerTest() {
            AttributeServiceScanner ss = new AttributeServiceScanner("SmartAjaxTest");
            var rt = ss.Services;

            foreach (var item in rt) {

            }
        }

        [Test]
        public void EntityConvertTest() {
            string jsonString = "{\"ID\" : 0,\"Name\" : \"123\"}";
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ReturnDemo));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            ReturnDemo obj = (ReturnDemo)ser.ReadObject(ms);

            System.Console.WriteLine(obj.ID + "_" + obj.Name);
        }
    }

    [RemoteService]
    public class ServiceDemo {

        public IList<string> ErrorMessage {get;set;}

        [RemoteMethod]
        public string PubMethod(int index) {
            return index.ToString();
        }
    }

    public class ReturnDemo {

        public int ID { get; set; }
        public String Name { get; set; }
    }
}
