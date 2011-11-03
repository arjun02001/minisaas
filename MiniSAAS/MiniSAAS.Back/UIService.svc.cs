using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MiniSAAS.Back.Classes;
using System.Data;

namespace MiniSAAS.Back
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UIService" in code, svc and config file together.
    public class UIService : IUIService
    {
        public List<Control> GetControls(int orgid, ControlLocation controllocation)
        {
            List<Control> controls = new List<Control>();
            try
            {
                string sql = string.Format(" select c.ControlID, c.ControlLocation, c.Text, c.RedirectURL, c.BackgroundImage ") +
                            string.Format(" from dbo.Control c, dbo.ControlLocation cl ") +
                            string.Format(" where c.ControlLocation = cl.Value and cl.Location = '{0}' and c.OrgID = '{1}' ", controllocation, orgid);
                DataTable dt = DataManager.GetData(sql);
                foreach (DataRow dr in dt.Rows)
                {
                    controls.Add(new Control()
                    {
                        ControlID = dr["ControlID"] != DBNull.Value ? Convert.ToInt32(dr["ControlID"]) : 0,
                        ControlLocation = controllocation,
                        Text = dr["Text"] != DBNull.Value ? dr["Text"].ToString() : string.Empty,
                        RedirectURL = dr["RedirectURL"] != DBNull.Value ? dr["RedirectURL"].ToString() : string.Empty,
                        BackgroundImage = dr["BackgroundImage"] != DBNull.Value ? dr["BackgroundImage"].ToString() : string.Empty
                    });
                }
            }
            catch (Exception)
            {
            }
            return controls;
        }
    }
}
