//-----------------------------------------------------------------------------------------
//   <copyright  file="JsonSerializerFactory.cs">
//      所属项目：NDWR.JsonSerializer
//      创 建 人：王跃
//      创建日期：2012-7-25 14:02:17
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.JsonSerializer {

    /// <summary>
    /// JsonSerializerFactory 概要
    /// </summary>
    public static class JsonSerializerFactory {

        public static IJsonSerializer Serializer {
            get {
                return new DataContractJsonSerializerImpl();
            }
        }

    }
}
