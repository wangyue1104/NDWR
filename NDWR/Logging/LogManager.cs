using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDWR.Logging {
    public static class LogManager {
        private static ILogFactory factory;

        static LogManager() {
            LogManager.factory = new NullLoggerFactory();
        }

        /// <summary>
        /// 指定一个新的日志工厂
        /// </summary>
        /// <param name="factory"></param>
        public static void AssignFactory(ILogFactory factory) {
            if (factory == null) {
                throw new ArgumentNullException("factory");
            }
            LogManager.factory = factory;
        }

        /// <summary>
        /// 按照类型返回
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILog GetLogger(Type type) {
            return factory.GetLogger(type);
        }

        /// <summary>
        /// 按照名称返回
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ILog GetLogger(string name) {
            return factory.GetLogger(name);
        }
    }
}
