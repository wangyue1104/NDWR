using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NDWR.Web {
    public interface IResponse {

        HttpContext Context { set; }
        InvocationBatch InvokeBatch { set; }
        void WriteResult();
    }
}
