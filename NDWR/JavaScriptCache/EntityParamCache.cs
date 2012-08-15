//-----------------------------------------------------------------------------------------
//   <copyright  file="EntityParamCache.cs">
//      所属项目：NDWR.ServiceScanner
//      创 建 人：王跃
//      创建日期：2012-8-3 17:29:56
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace NDWR.ServiceScanner {
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using NDWR.Validator;

    /// <summary>
    /// EntityParamCache 概要
    /// </summary>
    public class EntityParamCache {

        private IList<EntityParam> paramList = new List<EntityParam>();

        /// <summary>
        /// 注册实体
        /// </summary>
        /// <param name="type"></param>
        public void RegisterEntity(Type type) {
            foreach (EntityParam entity in paramList) {
                if (entity.EntityType == type) {
                    return;
                }
            }
            paramList.Add(new EntityParam(type));
        }
        /// <summary>
        /// 按照类型获取该实体的JS类代码
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string EntityJavaScript(Type type) {
            foreach (EntityParam entity in paramList) {
                if (entity.EntityType == type) {
                    return entity.JavaScript;
                }
            }
            return string.Empty;
        }

        private class EntityParam {

            public Type EntityType { get; set; }
            public string JavaScript { get; set; }

            public EntityParam(Type type) {
                this.EntityType = type;
            }

            public void buildEntityJS() {
                object entity = Activator.CreateInstance(EntityType);
                //object getValue;
                PropertyInfo[] pis = EntityType.GetProperties();
                if (pis == null || pis.Length == 0) {
                    JavaScript = string.Empty;
                    return;
                }
                StringBuilder sbEntityJS = new StringBuilder("\r\n/*NDWR自动生成参数实体*/\r\n");

                sbEntityJS.AppendFormat("function {0}() {{", EntityType.Name);

                foreach (PropertyInfo proInfo in pis) {
                    if (ValueConvert.IsBaseType(proInfo.PropertyType) &&
                        proInfo.CanRead &&
                        proInfo.CanWrite 
                        ) {
                            //getValue = proInfo.GetValue(entity, null);
                            //if (getValue == null) {
                            //    sbEntityJS.Append("    this.{0}; \r\n");
                            //} else {
                            //    sbEntityJS.AppendFormat("    this.{0} = {1}; \r\n", getValue.ToString());
                            //}
                            sbEntityJS.Append("    this.{0}; \r\n");
                    }
                }
                sbEntityJS.Append("}");

                this.JavaScript = sbEntityJS.ToString();
            }
        }
    }

}
