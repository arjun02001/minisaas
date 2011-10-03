using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class DataDescription
    {
        [DataMember]
        public int OrgID { get; set; }
        [DataMember]
        public string ObjName { get; set; }
        [DataMember]
        public List<String> Fields { get; set; }
        [DataMember]
        public List<List<String>> Data { get; set; }
    }
}