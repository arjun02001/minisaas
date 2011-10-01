using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using MiniSAAS.Back.Classes;

namespace MiniSAAS.Back
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataService" in code, svc and config file together.
    public class DataService : IDataService
    {
        public bool RegisterTenant(string emailid, string tenant)
        {
            return true;
        }

        public int LoginTenant(string emailid, string password)
        {
            return 1;
        }
    }
}
