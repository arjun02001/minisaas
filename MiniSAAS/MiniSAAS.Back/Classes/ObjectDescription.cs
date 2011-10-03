using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniSAAS.Back.Classes;
using System.Runtime.Serialization;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class ObjectDescription
    {
        [DataMember]
        public int OrgID { get; set; }
        [DataMember]
        public string ObjName { get; set; }
        [DataMember]
        public Dictionary<string, string> Fields { get; set; }
        [DataMember]
        public string PrimaryKey { get; set; }
    }
}