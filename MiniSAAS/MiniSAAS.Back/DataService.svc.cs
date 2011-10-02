using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using MiniSAAS.Back.Classes;
using System.Transactions;

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

        public bool CreateObject(ObjectDescription od)
        {
            try
            {
                //to check if the the Object exists
                string sql = "Select ObjID from dbo.Objects where OrgID = '" + od.OrgID + "' and ObjName = '" + od.ObjName.ToLower() + "'";
                DataTable dt = DataManager.GetData(sql);
                if (dt.Rows.Count != 0)
                {
                    return false;
                }
                //to insert
                using (TransactionScope scope = new TransactionScope())
                {
                    sql = string.Format("Insert into dbo.Objects (OrgID, ObjName) values ('{0}','{1}');", od.OrgID, od.ObjName.ToLower());
                    DataManager.SetData(sql);

                    sql = string.Format("select objid from dbo.objects where orgid = '{0}' and objname = '{1}'", od.OrgID, od.ObjName.ToLower());
                    dt = DataManager.GetData(sql);
                    int objectid = Convert.ToInt32(dt.Rows[0][0]);

                    int fieldnumbercounter = 0;
                    foreach (KeyValuePair<string, string> pair in od.Fields)
                    {
                        sql = string.Format("Insert into dbo.Fields(orgid, objid, fieldname, datatype, fieldnumber, isprimary) values" +
                            " ( '{0}' , '{1}' , '{2}' , '{3}' , '{4}' , '{5}')"
                            , od.OrgID, objectid, pair.Key.ToLower(), pair.Value, fieldnumbercounter++, od.PrimaryKey.ToLower().Equals(pair.Key.ToLower()) ? 1 : 0);
                        DataManager.SetData(sql);
                    }
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool DeleteObject(ObjectDescription od)
        {
            try
            {
                string sql = string.Format("select objid from dbo.objects where orgid = '{0}' and objname = '{1}'", od.OrgID, od.ObjName.ToLower());
                DataTable dt = DataManager.GetData(sql);
                if (dt.Rows.Count == 0)
                {
                    return false;
                }
                int objid = Convert.ToInt32(dt.Rows[0][0]);
                using (TransactionScope scope = new TransactionScope())
                {
                    sql = string.Format("delete from dbo.heap where orgid = '{0}' and objid = '{1}'", od.OrgID, objid);
                    DataManager.SetData(sql);
                    sql = string.Format("delete from dbo.fields where orgid = '{0}' and objid = '{1}'", od.OrgID, objid);
                    DataManager.SetData(sql);
                    sql = string.Format("delete from dbo.objects where orgid = '{0}' and objid = '{1}'", od.OrgID, objid);
                    DataManager.SetData(sql);
                    scope.Complete();
                }
            }
            catch (Exception)
            {
                return false;                
            }
            return true;
        }

        public List<ObjectDescription> GetObjectCollection(int orgid)
        {
            List<ObjectDescription> objectcollection = new List<ObjectDescription>();
            int objID = 0;
            
            try
            {
                String sql = string.Format("select ObjID, objname from dbo.Objects where OrgID='{0}';", orgid);
                DataTable dt = DataManager.GetData(sql);
                
                foreach(DataRow row in dt.Rows) //creating each ObjectDescrition
                {
                    ObjectDescription od = new ObjectDescription();
                    objID = Convert.ToInt32(row[0]);
                    od.ObjName = row[1].ToString();
                    od.OrgID = orgid;
                    sql = string.Format("select fieldname, datatype, isprimary from dbo.fields where orgid = '{0}' and objid = '{1}'", orgid, objID);
                    DataTable dtobjectdescription = DataManager.GetData(sql);
                    Dictionary<String, String> fieldCollection = new Dictionary<String, String>();
                    foreach (DataRow fields in dtobjectdescription.Rows)//creating Fields dictionary for each ObjectDescription
                    {
                        fieldCollection.Add(fields[0].ToString(), fields[1].ToString());
                        if (Convert.ToInt32(fields[2]) == 1)
                        {
                            od.PrimaryKey = fields[0].ToString();
                        }
                    }
                    od.Fields = fieldCollection;
                    objectcollection.Add(od);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return objectcollection;
        }
    }
}
