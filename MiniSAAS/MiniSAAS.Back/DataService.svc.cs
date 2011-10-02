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
        /// <summary>
        /// Return true if registration successfull. Else return false
        /// </summary>
        /// <param name="emailid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool RegisterTenant(string emailid, string password)
        {
            try
            {
                string sql = "select * from dbo.Tenant where emailid = '" + emailid + "'";
                DataTable dt = DataManager.GetData(sql);
                if (dt.Rows.Count != 0)
                {
                    return false;
                }
                sql = string.Format("insert into dbo.Tenant (emailid, password) values ('{0}', '{1}')", emailid, password);
                DataManager.SetData(sql);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Return OrgID if login successfull. Else return -1
        /// </summary>
        /// <param name="emailid"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int LoginTenant(string emailid, string password)
        {
            try
            {
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

        public bool CreateTable(ObjectDescription od)
        {
            try
            {
                
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
