using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Validator.Constraints;

namespace RemoteEntity {

    public class Entity {
        [Range(Min = 1, Max = 10, Message = "范围1~10")]
        public int? Id { get; set; }
        public String Name { get; set; }
        public String Pswd { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
