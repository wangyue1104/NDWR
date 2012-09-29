//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="ParamItem.cs">
//      所属项目：NDWR.Web
//      创 建 人：王跃
//      创建日期：2012-8-18 14:18:29
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR {

    /// <summary>
    /// ParamItem 参数记录
    /// </summary>
    public class ParamItem {
        public ParamItem(int id, int methodId,string value) {
            this.Id = id;
            this.MethodId = methodId;
            this.SrcValue = value;
        }

        /// <summary>
        /// Id 用于排序
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// 对应的方法ID
        /// </summary>
        public int MethodId { get; private set; }
        /// <summary>
        /// 接收用户传输的值
        /// </summary>
        public string SrcValue { get; private set; }
        /// <summary>
        /// 目标方法对应的值
        /// </summary>
        public object TarValue { get; set; }

    }
}
