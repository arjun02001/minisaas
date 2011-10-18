using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MiniSAAS.Back.Classes
{
    public class ServiceMethod
    {
        public string MethodName { get; set; }
        public List<string> Parameters { get; set; }
        public string ReturnType { get; set; }
    }
}