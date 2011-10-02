using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using MiniSAAS.DataServiceReference;
using System.Diagnostics;
using MiniSAAS.ChildWindows;
using MiniSAAS.Classes;

namespace MiniSAAS
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            try
            {
                Login login = new Login();
                login.LoginSuccess += new Login.LoginSuccessHandler(login_LoginSuccess);
                login.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void login_LoginSuccess(int orgid)
        {
            try
            {
                ObjectDescription od = new ObjectDescription();
                Dictionary<string, string> fields = new Dictionary<string, string>();
                fields.Add("laptopid", DataType.INT);
                fields.Add("model", DataType.VARCHAR);
                od.ObjName = "Laptop";
                od.Fields = fields;
                od.OrgID = orgid;
                od.PrimaryKey = "laptopid";
                DataServiceClient client = new DataServiceClient();
                client.CreateTableCompleted += new EventHandler<CreateTableCompletedEventArgs>(client_CreateTableCompleted);
                client.CreateTableAsync(od);

                //MessageBox.Show("orgid =" + orgid.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_CreateTableCompleted(object sender, CreateTableCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
