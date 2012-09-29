using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace NDWR.Web {

    public interface IResponse {
        ContextSupport Context { get; }
        void WriteResult();
    }
}
