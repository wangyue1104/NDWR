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
using Newtonsoft.Json;
using RemoteEntity;

namespace SmartAjaxTest {

    public class Product {
        public string Name { get; set; }
        public DateTime Expiry { get; set; }
        public decimal Price { get; set; }
        public string[] Sizes { get; set; }
        public object Include { get; set; }
    }

    [TestFixture]
    public class JsonDemo {

        [Test]
        public void SerializeTest() {
            IServiceProxyX proxy = new RemoteDemo_3_ServiceProxy();
            object rt = proxy.FuncSwitch(4, new object[] { 1, new Entity() { Id = 5 } });
            Product product = new Product();

            product.Name = "Apple";
            product.Expiry = new DateTime(2008, 12, 28);
            product.Price = 3.99M;
            product.Sizes = new string[] { "Small", "Medium", "Large" };
            product.Include = new ReturnDemo() { ID= 1, Name = "123" };
            string json = JavaScriptConvert.SerializeObject(product);
            JavaScriptConvert.SerializeObject(1);
            //System.Console.WriteLine(json);
            System.Console.WriteLine(DateTime.Now.ToString());
        }
        [Test]
        public void DeserializeTest() {
            //string json = "{\"Name\":\"Apple\",\"Expiry\":new Date(1230422400000),\"Price\":3.99,\"Sizes\":[\"Small\",\"Medium\",\"Large\"]}";
            string json = "{\"Name\":\"Apple\",\"Expiry\":\"2012-08-25T12:42:11\",\"Price\":3.99,\"Sizes\":[\"Small\",\"Medium\",\"Large\"]}";
            Product product = JavaScriptConvert.DeserializeObject(json, typeof(Product)) as Product;
            System.Console.WriteLine(product.Expiry.ToString());
            object o = JavaScriptConvert.DeserializeObject("\"2012-08-26T08:25:30\"", typeof(DateTime));
            System.Console.WriteLine(o.ToString() + "--");

        }
        //[Test]
        //public void CallBackMethodJsonStr() {
        //    CallBackMethod method0 = new CallBackMethod();
        //    method0.Method = "method0";
        //    method0.Args = new List<CallBackArg>() { 
        //        new CallBackArg() { Name = "0"},
        //        new CallBackArg() { Name = "1"}, 
        //        new CallBackArg() { Name = "2"}
        //    };

        //    CallBackMethod method1 = new CallBackMethod();
        //    method1.Method = "method1";
        //    method1.Args = new List<CallBackArg>() { 
        //        new CallBackArg() { Name = "0"},
        //        new CallBackArg() { Name = "1"}, 
        //        new CallBackArg() { Name = "2"}
        //    };

        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(List<CallBackMethod>));
        //    MemoryStream ms = new MemoryStream();
        //    ser.WriteObject(ms, new List<CallBackMethod>() { method0, method1 });
        //    string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        //    ms.Close();

        //    Console.WriteLine(jsonString);
        //}

        //[Test]
        //public void RspJsonStr() {
        //    IList<RspDataPackage> list = new List<RspDataPackage>(){
        //        new RspDataPackage(){
        //             Method = "", JsonData = "[{\"CustomeErrors\":null,\"Data\":null,\"Method\":\"\",\"SystemErrors\":null},{\"CustomeErrors\":null,\"Data\":null,\"Method\":\"\",\"SystemErrors\":null}]"
        //        },
        //        new RspDataPackage(){
        //             Method = "", JsonData = "324"
        //        }
        //    };

        //    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(IList<RspDataPackage>));
        //    MemoryStream ms = new MemoryStream();
        //    ser.WriteObject(ms, list);
        //    string jsonString = Encoding.UTF8.GetString(ms.ToArray());
        //    ms.Close();

        //    Console.WriteLine(jsonString);
        //}


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
