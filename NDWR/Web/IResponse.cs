using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NDWR.Web {
    public interface IResponse {

        HttpResponse Response { get; }
        InvocationBatch InvokeBatch { set; }
        void WriteResult();
    }
}
