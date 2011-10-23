using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data;
using System.Transactions;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class WorkflowDescription
    {
        [DataMember]
        public int OrgID;
        [DataMember]
        public List<Workflow> Workflows;

        public static bool ReplicateWorkflows(int orgid)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string sql = string.Format("insert into dbo.Workflow (OrgID, WorkflowName) select {0}, WorkflowName from dbo.Workflow where OrgID = '0'", orgid);
                    DataManager.SetData(sql);
                    sql = string.Format("select * from dbo.Workflow where OrgID = '0'");
                    DataTable dt = DataManager.GetData(sql);
                    foreach (DataRow dr in dt.Rows)
                    {
                        int workflowid = Convert.ToInt32(dr["WorkflowID"]);
                        string workflowname = dr["WorkflowName"].ToString();

                        sql = string.Format("select WorkflowID from dbo.Workflow where OrgID = '{0}' and WorkflowName = '{1}'", orgid, workflowname);
                        int newworkflowid = Convert.ToInt32(DataManager.GetData(sql).Rows[0]["WorkflowID"]);

                        sql = string.Format("insert into dbo.Method select {0}, MethodName, URL, ReturnType, Parameters, Sequence from dbo.Method where WorkflowID = '{1}'", newworkflowid, workflowid);
                        DataManager.SetData(sql);
                    }
                    scope.Complete();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static WorkflowDescription GetWorkflows(int orgid)
        {
            WorkflowDescription workflowdescription = new WorkflowDescription();
            List<Workflow> workflows = new List<Workflow>();
            List<Method> methods = null;
            Workflow workflow = null;
            Method method = null;
            try
            {
                string sql = string.Format("select * from dbo.Workflow where OrgID = '{0}'", orgid);
                DataTable dt = DataManager.GetData(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    workflow = new Workflow();
                    methods = new List<Method>();
                    int workflowid = Convert.ToInt32(dr["WorkflowID"]);
                    sql = string.Format("select * from dbo.Method where WorkflowID = '{0}'", workflowid);
                    DataTable dt1 = DataManager.GetData(sql);
                    foreach (DataRow dr1 in dt1.Rows)
                    {
                        method = new Method();
                        method.MethodID = Convert.ToInt32(dr1["MethodID"]);
                        method.WorkflowID = Convert.ToInt32(dr1["WorkflowID"]);
                        method.MethodName = dr1["MethodName"].ToString();
                        method.URL = (dr1["URL"] != DBNull.Value) ? dr1["URL"].ToString() : string.Empty;
                        method.ReturnType = (dr1["ReturnType"] != DBNull.Value) ? dr1["ReturnType"].ToString() : string.Empty;
                        method.Parameters = (dr1["Parameters"] != DBNull.Value) ? dr1["Parameters"].ToString() : null;
                        method.Sequence = (dr1["Sequence"] != DBNull.Value) ? Convert.ToInt32(dr1["Sequence"]) : -1;
                        methods.Add(method);
                    }
                    workflow.WorkflowID = workflowid;
                    workflow.WorkflowName = dr["WorkflowName"].ToString();
                    workflow.Methods = methods;
                    workflows.Add(workflow);
                }
                workflowdescription.OrgID = orgid;
                workflowdescription.Workflows = workflows;
            }
            catch (Exception)
            {
                return null;
            }
            return workflowdescription;
        }
    }
}