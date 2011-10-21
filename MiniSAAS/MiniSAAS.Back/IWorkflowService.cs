using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MiniSAAS.Back
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IWorkflowService" in both code and config file together.
    [ServiceContract]
    public interface IWorkflowService
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        int Login(string emailid, string password);
        [OperationContract]
        bool ForgotPassword(string emailid);
        [OperationContract]
        bool Register(string emailid, string password);
        [OperationContract]
        double ApplyCoupon(double amount, string couponcode);
        [OperationContract]
        double CalculateTax(double amount);
        [OperationContract]
        string VerifyCreditCard(string cardnumber, DateTime expdate);
        [OperationContract]
        List<string> GetShippingVendors();
        [OperationContract]
        bool ValidateZIP(string zip);
        [OperationContract]
        bool AddToCart(int orgid, string userid, string productid);
        [OperationContract]
        List<string> GetCart(int orgid, string userid);
        [OperationContract]
        bool DeleteCart(int orgid, string userid, List<string> productids);
    }
}
