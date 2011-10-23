using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class Method
    {
        [DataMember]
        public int MethodID { get; set;  }
        [DataMember]
        public string MethodName { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public string URL { get; set; }
        [DataMember]
        public string ReturnType { get; set; }
        [DataMember]
        public string Parameters { get; set; }
        [DataMember]
        public int WorkflowID { get; set; }
    }
}