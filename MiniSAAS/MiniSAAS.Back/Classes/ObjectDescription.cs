using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MiniSAAS.Back.Classes;

namespace MiniSAAS.Back.Classes
{
    public class ObjectDescription
    {
        public int OrgID { get; set; }
        public string ObjName { get; set; }
        public Dictionary<string, string> Fields { get; set; }
        public string PrimaryKey { get; set; }
    }
}