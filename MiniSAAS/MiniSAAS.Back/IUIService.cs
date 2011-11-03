using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MiniSAAS.Back.Classes;

namespace MiniSAAS.Back
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IUIService" in both code and config file together.
    [ServiceContract]
    public interface IUIService
    {
        [OperationContract]
        List<Control> GetControls(int orgid, ControlLocation controllocation);
        [OperationContract]
        bool UpdateHeader(int orgid, Control control);
    }
}
