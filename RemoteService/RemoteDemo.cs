using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDWR.Attributes;
using NDWR.Web;
using NHibernate.Validator.Constraints;

namespace RemoteService {

    [RemoteService]
    public class RemoteDemo {


        [RemoteMethod]
        public string HelloWold() {
            return "Hello Wold!";
        }

        [RemoteMethod]
        public string PubMethod(int index) {
            throw new Exception("index 超出范围");
            //return index.ToString();
        }

        [RemoteMethod]
        public string PubMethodEntity(Entity entity) {
            return entity.Id.ToString() + "," + entity.Name;
        }

        private static IList<Entity> list = new List<Entity>(){
            new Entity() { Id = 10, Name = "张三", Pswd = "23se" },
            new Entity() { Id = 22, Name = "李四", Pswd = "fsf#" },
            new Entity() { Id = 43, Name = "王五", Pswd = "@#DWdf" },
            new Entity() { Id = 34, Name = "赵六", Pswd = "^$%$ffd" },
            new Entity() { Id = 51, Name = "田七", Pswd = "^$%$ffd" },
            new Entity() { Id = 36, Name = "张伟", Pswd = "^$%$ffd" },
            new Entity() { Id = 78, Name = "胡一菲", Pswd = "^$%$ffd" },
            new Entity() { Id = 38, Name = "曾小贤", Pswd = "^$%$ffd" },
            new Entity() { Id = 49, Name = "吕子乔", Pswd = "^$%$ffd" },
            new Entity() { Id = 10, Name = "陈美佳", Pswd = "^$%$ffd" },
            new Entity() { Id = 14, Name = "陆展博", Pswd = "^$%$ffd" },
            new Entity() { Id = 29, Name = "关谷神奇", Pswd = "^$%$ffd" },
            new Entity() { Id = 32, Name = "唐悠悠", Pswd = "^$%$ffd" },
            new Entity() { Id = 94, Name = "林宛瑜", Pswd = "^$%$ffd" },
            new Entity() { Id = 58, Name = "三不完", Pswd = "^$%$ffd" },
            new Entity() { Id = 61, Name = "四待续", Pswd = "^$%$ffd" },
            new Entity() { Id = 78, Name = "陈赫", Pswd = "^$%$ffd" },
            new Entity() { Id = 52, Name = "娄艺潇", Pswd = "^$%$ffd" },
            new Entity() { Id = 45, Name = "赵霁", Pswd = "^$%$ffd" },
            new Entity() { Id = 57, Name = "孙艺洲", Pswd = "^$%$ffd" },
            new Entity() { Id = 92, Name = "王传君", Pswd = "^$%$ffd" },
            new Entity() { Id = 37, Name = "邓家佳", Pswd = "^$%$ffd" },
            new Entity() { Id = 36, Name = "金世佳", Pswd = "^$%$ffd" },
            new Entity() { Id = 69, Name = "李金铭", Pswd = "^$%$ffd" },
            new Entity() { Id = 58, Name = "李佳航", Pswd = "^$%$ffd" },
            new Entity() { Id = 12, Name = "榕榕", Pswd = "^$%$ffd" },
            new Entity() { Id = 31, Name = "赵文琪", Pswd = "^$%$ffd" },
            new Entity() { Id = 78, Name = "徐佳琦", Pswd = "^$%$ffd" },
            new Entity() { Id = 18, Name = "高凌风", Pswd = "^$%$ffd" },
            new Entity() { Id = 62, Name = "康康 ", Pswd = "^$%$ffd" },
            new Entity() { Id = 45, Name = "韦正", Pswd = "^$%$ffd" },
            new Entity() { Id = 55, Name = "汪远", Pswd = "^$%$ffd" },
            new Entity() { Id = 89, Name = "萨钢云", Pswd = "^$%$ffd" },
            new Entity() { Id = 73, Name = "段倩茹", Pswd = "^$%$ffd" },
            new Entity() { Id = 81, Name = "张超", Pswd = "^$%$ffd" },
            new Entity() { Id = 23, Name = "杜俊", Pswd = "^$%$ffd" }
        };

        [RemoteMethod]
        public IList<Entity> DataList(int managerId) {
            return list;
        }


        [RemoteMethod]
        public DTOEntity DataTable(DataTableParameter dtParam) {

            var d = list.Skip(dtParam.iDisplayStart).Take(dtParam.iDisplayLength).ToList();

            return new DTOEntity() {
                sEcho = dtParam.sEcho,
                iTotalRecords = list.Count,
                iTotalDisplayRecords = list.Count,
                aaData = d
            };
        }

        [RemoteMethod]
        public TransferFile DownLoadFile(Entity entity) {
            return new TransferFile() {
                FileName = "dd..xls",
                ContentType = TransferFile.EXCEL,
                 DataBuffer = new byte[] {11,22}
            };
        }
    }


    public class Entity {
        [Range(Min = 1, Max = 10, Message = "范围1~10")]
        public int Id { get; set; }
        public String Name { get; set; }
        public String Pswd { get; set; }
    }

    public class DTOEntity{
        public string sEcho {get;set;}
        public int iTotalRecords{get;set;}
        public int iTotalDisplayRecords{get;set;}
        public IList<Entity> aaData{get;set;}
    }

    public class DataTableParameter {
        /// <summary>
        /// DataTable请求服务器端次数
        /// </summary>       
        public string sEcho { get; set; }
        /// <summary>
        /// 每页显示的数量
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// 分页时每页跨度数量
        /// </summary>
        public int iDisplayStart { get; set; }
        /// <summary>
        /// 排序列
        /// </summary>
        public string orderbyColName { get; set; }
        /// <summary>
        /// 排序方式
        /// </summary>
        public string sortType { get; set; }
        /// <summary>
        /// 过滤文本
        /// </summary>
        public string sSearch { get; set; }
    }
}
