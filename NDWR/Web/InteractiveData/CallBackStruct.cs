using System;
using System.Collections.Generic;

namespace NDWR {

    public class CallBackMethod {

        public String Method { get; set; }

        public IList<CallBackArg> Args { get; set; }
    }

    public class CallBackArg {
        public string Name { get; set; }
    }
}
