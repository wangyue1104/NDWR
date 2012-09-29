using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDWR.ServiceStruct {

    public enum TypeCategory {
        /// <summary>
        /// 简单类型
        /// </summary>
        SimplyType,
        /// <summary>
        /// 实体类型
        /// </summary>
        EntityType,
        /// <summary>
        /// 输入流类型[TransferFile类型]
        /// </summary>
        BinaryType
    }
}
