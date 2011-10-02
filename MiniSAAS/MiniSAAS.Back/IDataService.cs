using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MiniSAAS.Back.Classes;

namespace MiniSAAS.Back
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDataService" in both code and config file together.
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        bool RegisterTenant(string emailid, string password);
        [OperationContract]
        int LoginTenant(string emailid, string password);
        [OperationContract]
        bool CreateObject(ObjectDescription od);
        [OperationContract]
        bool DeleteObject(ObjectDescription od);
        [OperationContract]
        List<ObjectDescription> GetObjectCollection(int orgid);
        [OperationContract]
        int InsertData(DataDescription dd);
        [OperationContract]
        DataDescription ViewData(ObjectDescription od);
        //[OperationContract]
        //int DeleteData(DataDescription dd);
    }
}
