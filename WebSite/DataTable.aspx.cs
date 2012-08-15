using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class DataTable : System.Web.UI.Page {
    protected void Page_Load(object sender, EventArgs e) {
        var aaData = new List<object> {
            new string[] {"1","公司信息","地址信息","城市信息"},
            new string[] {"2","公司信息","地址信息","城市信息"},
            new string[] {"3","公司信息","地址信息","城市信息"},
            new string[] {"4","公司信息","地址信息","城市信息"},
            new string[] {"5","公司信息","地址信息","城市信息"},
            new string[] {"6","公司信息","地址信息","城市信息"},
            new string[] {"7","公司信息","地址信息","城市信息"},
            new string[] {"8","公司信息","地址信息","城市信息"},
            new string[] {"9","公司信息","地址信息","城市信息"},
            new string[] {"0","公司信息","地址信息","城市信息"}
        };
    }



    public class DataTableParameter {
        /// <summary>
        /// DataTable请求服务器端次数
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// 过滤文本
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// 每页显示的数量
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// 分页时每页跨度数量
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// 排序列的数量
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// 逗号分割所有的列
        /// </summary>
        public string sColumns { get; set; }
    }

}