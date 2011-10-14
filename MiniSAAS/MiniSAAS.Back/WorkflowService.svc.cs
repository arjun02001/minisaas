using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MiniSAAS.Back.Classes;

namespace MiniSAAS.Back
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkflowService" in code, svc and config file together.
    public class WorkflowService : IWorkflowService
    {
        public void DoWork()
        {
            WebServiceInvoker invoker = new WebServiceInvoker(new Uri("http://rjun.info:16000/asmx/Service1.asmx"));
            string servicename = invoker.AvailableServices[0];
            string method = invoker.EnumerateServiceMethods(servicename)[0];
            object[] args = new object[] { 1, 2 };
            int result = invoker.InvokeMethod<int>(servicename, method, args);
        }
    }
}
