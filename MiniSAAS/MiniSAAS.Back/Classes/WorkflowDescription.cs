using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Data;

namespace MiniSAAS.Back.Classes
{
    [DataContract]
    public class WorkflowDescription
    {
        [DataMember]
        public int OrgID;
        [DataMember]
        public List<Workflow> Workflows;

        public static WorkflowDescription GetWorkflow(int orgid)
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
                        method.Parameters = (dr1["Parameters"] != DBNull.Value) ? dr1["Parameters"].ToString().Split(',').ToList<string>() : null;
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