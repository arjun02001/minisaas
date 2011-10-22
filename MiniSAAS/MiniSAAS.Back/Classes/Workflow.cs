using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class Workflow
    {
        [DataMember]
        public int WorkflowID;
        [DataMember]
        public string WorkflowName;
        [DataMember]
        public List<Method> Methods;
    }
}