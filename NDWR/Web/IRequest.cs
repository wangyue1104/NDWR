using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDWR.Web {
    public interface IRequest {

        string TransferMode { get; }

        string BatchId { get; }

        Invocation[] BatchInvoke { get; }

        TransferFile GetFlie(string key);
    }
}
