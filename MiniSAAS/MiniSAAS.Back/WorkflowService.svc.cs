﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MiniSAAS.Back.Classes;
using System.Data;
using System.Text.RegularExpressions;

namespace MiniSAAS.Back
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WorkflowService" in code, svc and config file together.
    public class WorkflowService : IWorkflowService
    {
        public void DoWork()
        {
            //WebServiceInvoker invoker = new WebServiceInvoker(new Uri("http://rjun.info:16000/asmx/Service1.asmx"));
            WebServiceInvoker invoker = new WebServiceInvoker(new Uri("http://wsf.cdyne.com/ProfanityWS/Profanity.asmx"));
            List<string> services = invoker.AvailableServices;
            List<ServiceMethod> methods = invoker.EnumerateServiceMethods(services[0]);
            //object[] args = new object[] { 1, 2 };
            //int result = invoker.InvokeMethod<int>(services[0], methods[0].MethodName, args);
        }

        public int Login(string emailid, string password)
        {
            try
            {
                if (!Utility.ValidateEmail(emailid))
                {
                    return -1;
                }
                string sql = string.Format("select orgid from dbo.Tenant where emailid = '{0}' and password = '{1}'", emailid, password);
                DataTable dt = DataManager.GetData(sql);
                if (dt.Rows.Count == 0)
                {
                    return -1;
                }
                return Convert.ToInt32(dt.Rows[0][0]);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool ForgotPassword(string emailid)
        {
            try
            {
                if (!Utility.ValidateEmail(emailid))
                {
                    return false;
                }
                string sql = string.Format("select password from dbo.Tenant where emailid = '{0}'", emailid);
                DataTable dt = DataManager.GetData(sql);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                return Utility.SendEmail(emailid, "Password", string.Format("Password = {0}", dt.Rows[0][0].ToString()));
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Register(string emailid, string password)
        {
            try
            {
                if (!Utility.ValidateEmail(emailid))
                {
                    return false;
                }
                string sql = "select * from dbo.Tenant where emailid = '" + emailid + "'";
                DataTable dt = DataManager.GetData(sql);
                if (dt.Rows.Count != 0)
                {
                    return false;
                }
                sql = string.Format("insert into dbo.Tenant (emailid, password) values ('{0}', '{1}')", emailid, password);
                DataManager.SetData(sql);
                Utility.SendEmail(emailid, "New account registered", string.Format("Email = {0}\nPassword = {1}", emailid, password));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public double ApplyCoupon(double amount, string couponcode)
        {
            try
            {
                switch (couponcode.ToLower())
                {
                    case "aaa": return 0;
                    case "bbb": return amount / 2.0;
                    case "ccc": return amount / 3.0;
                    default: return amount;
                }
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public double CalculateTax(double amount)
        {
            try
            {
                return amount + (amount * 12 / 100);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public string VerifyCreditCard(string cardnumber, DateTime expdate)
        {
            try
            {
                if (cardnumber.Length < 13)
                {
                    return "Invalid Card";
                }
                else if (expdate < DateTime.Now)
                {
                    return "Invalid Card";
                }
                else
                {
                    int sum = 0;
                    int l = cardnumber.Length;
                    int offset = l % 2;
                    byte[] digits = new System.Text.ASCIIEncoding().GetBytes(cardnumber);

                    for (int i = 0; i < l; i++)
                    {
                        digits[i] -= 48;
                        if (((i + offset) % 2) == 0)
                        {
                            digits[i] *= 2;
                        }
                        sum += (digits[i] > 9) ? digits[i] - 9 : digits[i];
                    }
                    if (Regex.IsMatch(cardnumber, "(^4[0-9]{12}(?:[0-9]{3})?$)"))
                    {
                        return "Visa Card";
                    }
                    else if (Regex.IsMatch(cardnumber, "(^5[1-5][0-9]{14}$)"))
                    {
                        return "Master card";
                    }
                    else if (Regex.IsMatch(cardnumber, "(^3[47][0-9]{13}$)"))
                    {
                        return "American Express";
                    }
                    else if (Regex.IsMatch(cardnumber, "(^3(?:0[0-5]|[68][0-9])[0-9]{11}$)"))
                    {
                        return "Diners Club";
                    }
                    else
                    {
                        return "Unknown Card Number";
                    }
                }
            }
            catch (Exception)
            {
                return "Error";
            }
        }

        public List<string> GetShippingVendors()
        {
            try
            {
                return new List<string>() { "UPS", "USPS", "FEDEX", "DHL" };
            }
            catch (Exception)
            {
                return new List<string>() { string.Empty };
            }
        }

        public bool ValidateZIP(string zip)
        {
            string zip_regex = "(^[0-9]{5}$)|(^[0-9]{5}-[0-9]{4}$)";
            try
            {
                return (!String.IsNullOrEmpty(zip) && Regex.IsMatch(zip, zip_regex));
            }
            catch
            {
                return false;
            }
        }

        public bool AddToCart(int orgid, string userid, string productid)
        {
            try
            {
                string sql = string.Format("insert into dbo.Cart values ('{0}', '{1}', '{2}')", orgid, userid, productid);
                DataManager.SetData(sql);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<string> GetCart(int orgid, string userid)
        {
            List<string> productids = new List<string>();
            try
            {
                string sql = string.Format("select ProductID from dbo.Cart where OrgID = '{0}' and UserID = '{1}'", orgid, userid);
                DataTable dt = DataManager.GetData(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    productids.Add(dr[0].ToString());
                }
            }
            catch (Exception)
            {
            }
            return productids;
        }

        public bool DeleteCart(int orgid, string userid, List<string> productids)
        {
            int i = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format(" delete from dbo.Cart where OrgID = '{0}' and UserID = '{1}' and ProductID in ( ", orgid, userid ));
                for (i = 0; i < productids.Count - 1; i++)
                {
                    sb.Append(string.Format(" '{0}', ", productids[i]));
                }
                sb.Append(string.Format(" '{0}') ", productids[i]));
                DataManager.SetData(sb.ToString());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}