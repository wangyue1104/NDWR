//-----------------------------------------------------------------------------------------
//   <copyright company="同程网" file="NHVHelper.cs">
//      所属项目：RemoteService
//      创 建 人：王跃
//      创建日期：2012-8-9 8:14:08
//      用    途：请一定在此描述用途
//
//      更新记录:
//
//   </copyright> 
//-----------------------------------------------------------------------------------------

namespace RemoteService {
    using System;
    using System.Data;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using NHibernate.Validator.Engine;
    using System.IO;
    using System.Web;

    /// <summary>
    /// NHVHelper 概要
    /// </summary>
    public class NHVHelper {

        private ValidatorEngine engine = null;

        private static NHVHelper helper = new NHVHelper();

        public static ValidatorEngine Instance {
            get {
                return helper.engine;
            }
        }

        private NHVHelper() {
            engine = new ValidatorEngine();
            //var nhvc = new XmlConfiguration();
            //nhvc.Properties[NHibernate.Validator.Cfg.Environment.ApplyToDDL] = "false";
            //nhvc.Properties[NHibernate.Validator.Cfg.Environment.AutoregisterListeners] = "false";
            //nhvc.Properties[NHibernate.Validator.Cfg.Environment.ValidatorMode] = "UseAttribute";
            //nhvc.Mappings.Add(new MappingConfiguration("WebApplication1", null));
            engine.Configure(
                HttpContext.Current.Server.MapPath(
                    Path.Combine("~/App_Data/", "nhvalidator.cfg.xml")
                )
            );
        }
    }
}