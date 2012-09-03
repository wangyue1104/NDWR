//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="NewtonsoftJsonSerializerImpl.cs">
//      所属项目：NDWR.JsonSerializer
//      创 建 人：王跃
//      创建日期：2012-8-24 10:37:50
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.JsonSerializer {
    using System;
    using System.Data;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;
    using NDWR.Validator;

    /// <summary>
    /// NewtonsoftJsonSerializerImpl 概要
    /// </summary>
    public class NewtonsoftJsonSerializerImpl : IJsonSerializer {

        public string Serializer(object t) {
            if (TypeHelper.IsBaseType(t.GetType())) {
                return JavaScriptConvert.ToString(t);
            }
            return JavaScriptConvert.SerializeObject(t);
        }

        public object Deserialize(string jsonString, Type type) {
            return JavaScriptConvert.DeserializeObject(jsonString, type);
        }
    }
}
