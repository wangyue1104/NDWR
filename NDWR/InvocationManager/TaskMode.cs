using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDWR {

    /// <summary>
    /// 方法所执行的任务类型
    /// 每种类型将会对应不同的输出模版
    /// </summary>
    public enum TaskMode {
        /// <summary>
        /// 文本流
        /// </summary>
        TextStream = 1,
        /// <summary>
        /// 输入二进制流
        /// </summary>
        InputBinaryStream = 2,
        /// <summary>
        /// 输出二进制流
        /// </summary>
        OutputBinaryStream = 3
    }
}
