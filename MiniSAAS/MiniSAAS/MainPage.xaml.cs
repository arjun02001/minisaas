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
                //DeleteObject(orgid);
                //CreateObject(orgid);
                //ViewObject(orgid);
                InsertData(orgid);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void InsertData(int orgid)
        {
            DataDescription dd = new DataDescription();
            dd.ObjName = "laptop";
            dd.OrgID = orgid;
            List<String> fields = new List<string>();
            fields.Add("laptopid");
            fields.Add("model");

            List<List<String>> data = new List<List<string>>();
            
            List<String> row1 = new List<string>();
            row1.Add("11");
            row1.Add("model1");
            data.Add(row1);

            List<String> row2 = new List<string>();
            row2.Add("21");
            row2.Add("model2");
            data.Add(row2);
            dd.Fields = fields;
            dd.Data = data;
            DataServiceClient client = new DataServiceClient();
            client.InsertDataCompleted += new EventHandler<InsertDataCompletedEventArgs>(client_InsertDataCompleted);
            client.InsertDataAsync(dd);
        }

        void client_InsertDataCompleted(object sender, InsertDataCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void ViewObject(int orgid)
        {
            try
            {
                DataServiceClient client = new DataServiceClient();
                client.GetObjectCollectionCompleted += new EventHandler<GetObjectCollectionCompletedEventArgs>(client_GetObjectCollectionCompleted);
                client.GetObjectCollectionAsync(12);
            }
            catch (Exception)
            {
                throw;
            }
        }

        void client_GetObjectCollectionCompleted(object sender, GetObjectCollectionCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void DeleteObject(int orgid)
        {
            try
            {
                DataServiceClient client = new DataServiceClient();
                client.DeleteObjectCompleted += new EventHandler<DeleteObjectCompletedEventArgs>(client_DeleteObjectCompleted);
                ObjectDescription od = new ObjectDescription();
                od.ObjName = "LAPTOP";
                od.OrgID = orgid;
                client.DeleteObjectAsync(od);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_DeleteObjectCompleted(object sender, DeleteObjectCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("Table deleted");
            }
            else
            {
                MessageBox.Show("An error occurred. Please try again.");
            }
        }

        void CreateObject(int orgid)
        {
            try
            {
                ObjectDescription od = new ObjectDescription();
                Dictionary<string, string> fields = new Dictionary<string, string>();
                fields.Add("isbn", DataType.VARCHAR);
                fields.Add("author", DataType.VARCHAR);
                od.ObjName = "book";
                od.Fields = fields;
                od.OrgID = orgid;
                od.PrimaryKey = "isbn";
                DataServiceClient client = new DataServiceClient();
                client.CreateObjectCompleted += new EventHandler<CreateObjectCompletedEventArgs>(client_CreateObjectCompleted);
                client.CreateObjectAsync(od);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_CreateObjectCompleted(object sender, CreateObjectCompletedEventArgs e)
        {
            if (e.Result)
            {
                MessageBox.Show("Table created successfully");
            }
            else
            {
                MessageBox.Show("An error occured. Please try again");
            }
        }
    }
}
