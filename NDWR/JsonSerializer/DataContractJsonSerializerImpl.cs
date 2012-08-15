//-----------------------------------------------------------------------------------------
//   <copyright  file="DataContractJsonSerializerImpl.cs">
//      所属项目：NDWR.JsonSerializer
//      创 建 人：王跃
//      创建日期：2012-7-25 14:03:58
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.JsonSerializer {
    using System;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;

    /// <summary>
    /// DataContractJsonSerializerImpl 概要
    /// </summary>
    public class DataContractJsonSerializerImpl : IJsonSerializer {

        public string Serializer(object t) {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(t.GetType());
            string jsonString = string.Empty;
            MemoryStream ms = new MemoryStream();
            try {
                ser.WriteObject(ms, t);
                jsonString = Encoding.UTF8.GetString(ms.ToArray());
                return jsonString;
            } catch (Exception ex) {
                throw new NDWRException("序列化实体失败", ex);
            } finally {
                ms.Close();
            }
        }


        public object Deserialize(string jsonString, Type type) {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(type);
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            try {
                return ser.ReadObject(ms);
            } catch (Exception ex) {
                throw new NDWRException("反序列化实体失败", ex);
            } finally {
                ms.Close();
            }
        }

    }
}
