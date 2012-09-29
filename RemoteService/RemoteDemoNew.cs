using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NDWR.Attributes;
using NDWR.Web;
using RemoteEntity;

namespace RemoteService {

    [RemoteService]
    public class RemoteDemoNew {

        [RemoteMethod]
        public void Method1() {

        }

        [RemoteMethod]
        public void Method2(int i) {

        }

        [RemoteMethod]
        public int Method3(int i) {
            return ++i;
        }

        [RemoteMethod]
        public int Method4(int i) {
            throw new Exception("抛出异常" + i);
        }

        [RemoteMethod]
        public int Method5(Entity entity) {
            if (entity == null) {
                throw new Exception("传入实体不能为空");
            }
            return entity.Id.Value;
        }

        [RemoteMethod]
        public Entity Method6(Entity entity) {
            if (entity == null) {
                throw new Exception("传入实体不能为空");
            }
            return entity;
        }


        [RemoteMethod]
        public TransferFile Method7(string name) {
            return new TransferFile() {
                FileName = name + ".xls",
                ContentType = TransferFile.EXCEL,
                DataBuffer = new byte[] { 11, 22 }
            };
        }



        [RemoteMethod]
        public string Method8(TransferFile file) {
            if (file == null) {
                return "未发现文件";
            }
            if (!string.IsNullOrEmpty(file.FileName)) {
                byte[] data = new byte[file.ContentLength];
                file.DataStream.Read(data, 0, (int)file.ContentLength);

                file.SaveAs("~/Upload", file.FileName);
            }
            return "上传成功";
        }
    }
}
