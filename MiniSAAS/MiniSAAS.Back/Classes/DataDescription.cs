using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniSAAS.Back.Classes
{
    public class DataDescription
    {
        public int OrgID { get; set; }
        public string ObjName { get; set; }
        public List<String> Fields { get; set; }
        public List<List<String>> Data { get; set; }
    }
}