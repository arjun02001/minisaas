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

        public bool UpdateHeader(int orgid, Control control)
        {
            try
            {
                string sql = string.Format(" select * from dbo.Control c, dbo.ControlLocation cl ") +
                            string.Format(" where c.ControlLocation = cl.Value and c.OrgID = '{0}' and c.ControlLocation = '{1}' ", orgid, (int)ControlLocation.Header);
                DataTable dt = DataManager.GetData(sql);
                if (dt.Rows.Count > 0)
                {
                    sql = string.Format(" update dbo.Control set Text = '{0}', BackgroundImage = '{1}' where ControlID = '{2}' ", control.Text.Trim(), control.BackgroundImage.Trim(), control.ControlID);

                }
                else
                {
                    sql = string.Format(" insert into dbo.Control (OrgID, ControlLocation, Text, BackgroundImage) ") +
                          string.Format(" values ('{0}', '{1}', '{2}', '{3}') ", orgid, (int)ControlLocation.Header, control.Text.Trim(), control.BackgroundImage.Trim());
                }
                DataManager.SetData(sql);
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
    }
}
