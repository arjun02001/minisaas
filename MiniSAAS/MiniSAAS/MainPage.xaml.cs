﻿using System;
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
        List<ObjectDescription> ods = null;
        DataDescription dd = null;
        int orgid = 0;

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
            this.orgid = orgid;
            GetTenantData();
        }

        private void GetTenantData()
        {
            try
            {

                DataServiceClient client = new DataServiceClient();
                client.GetObjectCollectionCompleted += new EventHandler<GetObjectCollectionCompletedEventArgs>(client_GetObjectCollectionCompleted);
                client.GetObjectCollectionAsync(orgid);
                uiBusyIndicator.IsBusy = true;
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }


        private void DeleteData(int orgid)
        {
            try
            {
                DataDescription dd = new DataDescription();
                dd.OrgID = orgid;
                dd.ObjName = "shirt";

                List<String> fields = new List<string>();
                fields.Add("id");
                fields.Add("color");
                
                
                dd.Fields = fields;

                List<List<String>> datacollection = new List<List<string>>();
                
                List<String> data1 = new List<string>();
                data1.Add("1");
                data1.Add("red");
                datacollection.Add(data1);

                List<String> data2 = new List<string>();
                data2.Add("2");
                data2.Add("blue");
                datacollection.Add(data2);

                dd.Data = datacollection;

                            
                DataServiceClient client = new DataServiceClient();
                client.DeleteDataCompleted += new EventHandler<DeleteDataCompletedEventArgs>(client_DeleteDataCompleted);
                client.DeleteDataAsync(dd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_DeleteDataCompleted(object sender, DeleteDataCompletedEventArgs e)
        {
            MessageBox.Show(e.Result + " Rows deleted");
        }


        void client_GetObjectCollectionCompleted(object sender, GetObjectCollectionCompletedEventArgs e)
        {
            uiBusyIndicator.IsBusy = false;
            try
            {
                uiListOfObjects.Items.Clear();
                ods = e.Result;
                uiListOfObjects.Items.Add("Please Select");
                foreach (ObjectDescription od in ods)
                {
                    uiListOfObjects.Items.Add(od.ObjName);
                }
                uiListOfObjects.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiListOfObjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (uiListOfObjects.Items.Count == 0)
                {
                    return;
                }
                if (uiListOfObjects.SelectedItem.ToString().ToLower().Equals("please select"))
                {
                    return;
                }
                string objectname = uiListOfObjects.SelectedItem.ToString();
                ObjectDescription od = (from p in ods
                                        where p.ObjName.Equals(objectname)
                                        select p).Single();
                uiObjectSchema.Items.Clear();
                foreach (KeyValuePair<string, string> pair in od.Fields)
                {
                    uiObjectSchema.Items.Add(pair.Key + "  " + pair.Value);
                }

                od = new ObjectDescription();
                od.OrgID = orgid;
                od.ObjName = objectname;

                DataServiceClient client = new DataServiceClient();
                client.ViewDataCompleted +=new EventHandler<ViewDataCompletedEventArgs>(client_ViewDataCompleted);
                client.ViewDataAsync(od);
                uiBusyIndicator.IsBusy = true;
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_ViewDataCompleted(object sender, ViewDataCompletedEventArgs e)
        {
            uiBusyIndicator.IsBusy = false;
            try
            {
                dd = e.Result;
                uiDataGrid.ItemsSource = MasterObject.GetMasterObject(dd);
                uiDataGrid.LayoutUpdated += new EventHandler(uiDataGrid_LayoutUpdated);
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void uiDataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (string field in dd.Fields)
                {
                    uiDataGrid.Columns[count++].Header = field;
                }
                int columncount = uiDataGrid.Columns.Count;
                for (int i = dd.Fields.Count; i < columncount; i++)
                {
                    uiDataGrid.Columns.RemoveAt(i);
                }
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                //MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiCreateObject_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CreateObject createobject = new CreateObject(orgid);
                createobject.ObjectCreated += new ChildWindows.CreateObject.ObjectCreatedHandler(createobject_ObjectCreated);
                createobject.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void createobject_ObjectCreated(bool status)
        {
            if (!status)
            {
                MessageBox.Show("Object could not be created");
            }
            GetTenantData();
        }

        private void uiDeleteObject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void uiAddData_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                AddData adddata = new AddData((from p in ods
                                               where p.ObjName == uiListOfObjects.SelectedItem
                                               select p).Single());
                adddata.DataAdded += new AddData.AddDataHandler(adddata_DataAdded);
                adddata.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void adddata_DataAdded(int status)
        {
            MessageBox.Show(status.ToString() + " rows inserted");
            GetTenantData();
        }

        private void uiDeleteData_Click(object sender, RoutedEventArgs e)
        {

        }





        private void ViewData(int orgid)
        {
            ObjectDescription od = new ObjectDescription();
            od.OrgID = 1;
            od.ObjName = "laptop";
            DataServiceClient client = new DataServiceClient();
            client.ViewDataCompleted += new EventHandler<ViewDataCompletedEventArgs>(client_ViewDataCompleted);
            client.ViewDataAsync(od);
        }

        

        private void InsertData(int orgid)
        {
            DataDescription dd = new DataDescription();
            dd.ObjName = "laptop";
            dd.OrgID = orgid;
            List<String> fields = new List<string>();
            fields.Add("model");
            fields.Add("laptopid");

            List<List<String>> data = new List<List<string>>();
            
            List<String> row1 = new List<string>();
            row1.Add("model3");
            row1.Add("31");
            data.Add(row1);

            List<String> row2 = new List<string>();
            row2.Add(null);
            row2.Add("41");
            data.Add(row2);
            dd.Fields = fields;
            dd.Data = data;
            DataServiceClient client = new DataServiceClient();
            client.InsertDataCompleted += new EventHandler<InsertDataCompletedEventArgs>(client_InsertDataCompleted);
            client.InsertDataAsync(dd);
        }

        void client_InsertDataCompleted(object sender, InsertDataCompletedEventArgs e)
        {
            MessageBox.Show(e.Result + " rows inserted");
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
