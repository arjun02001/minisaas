using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class WorkflowDescription
    {
        [DataMember]
        public int OrgID;
        [DataMember]
        public List<Workflow> Workflows;
    }
}