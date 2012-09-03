using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NDWR.JavaScript {
    public static class CoreJavaScript {

        public static string Content ;

        static CoreJavaScript() {
            Stream jsSteam = null;
            StreamReader sr = null;
            try {
                jsSteam = typeof(CoreJavaScript).Assembly.GetManifestResourceStream("NDWR.ndwrcore.js");
                sr = new StreamReader(jsSteam);

                Content = sr.ReadToEnd();
            } catch (Exception ex) {
                throw new NDWRException("读取嵌入JS核心脚本异常", ex);
            } finally {
                if (sr != null) {
                    sr.Close();//关闭流
                }
                if (jsSteam != null) {
                    jsSteam.Close();
                }
            }
        
        }
    }
}
