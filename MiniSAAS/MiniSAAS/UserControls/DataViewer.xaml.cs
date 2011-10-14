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

namespace MiniSAAS.UserControls
{
    public partial class DataViewer : UserControl
    {
        List<ObjectDescription> ods = null;
        DataDescription dd = null;
        int orgid = 0;

        public DataViewer(int orgid)
        {
            InitializeComponent();
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
            SchemaRefresh();
            DataRefresh();
        }

        private void SchemaRefresh()
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
                    uiObjectSchema.Items.Add(pair.Key + "  " + pair.Value + ((pair.Key.Equals(od.PrimaryKey)) ? "  [Primary Key]" : string.Empty));
                }
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void DataRefresh()
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
                ObjectDescription od = (from p in ods
                                        where p.ObjName.Equals(uiListOfObjects.SelectedItem.ToString())
                                        select p).Single();

                DataServiceClient client = new DataServiceClient();
                client.ViewDataCompleted += new EventHandler<ViewDataCompletedEventArgs>(client_ViewDataCompleted);
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
                dd.OrgID = orgid;
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
                if (uiDataGrid.Columns.Count != 0 && uiDataGrid.Columns[0].Header.ToString().ToLower().Equals("guid"))
                {
                    uiDataGrid.Columns[0].Visibility = System.Windows.Visibility.Collapsed;
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
                createobject.ObjectCreated += new CreateObject.ObjectCreatedHandler(createobject_ObjectCreated);
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
            try
            {
                DataServiceClient client = new DataServiceClient();
                client.DeleteObjectCompleted += new EventHandler<DeleteObjectCompletedEventArgs>(client_DeleteObjectCompleted);
                client.DeleteObjectAsync((from p in ods
                                          where p.ObjName == uiListOfObjects.SelectedItem
                                          select p).Single());
                uiBusyIndicator.IsBusy = true;
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_DeleteObjectCompleted(object sender, DeleteObjectCompletedEventArgs e)
        {
            uiBusyIndicator.IsBusy = false;
            if (e.Result)
            {
                MessageBox.Show("Table deleted");
            }
            else
            {
                MessageBox.Show("An error occurred. Please try again.");
            }
            GetTenantData();
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
            if (status == -1)
            {
                MessageBox.Show("Data type mismatch during insertion");
            }
            else if (status == -2)
            {
                MessageBox.Show("Primary Key constraint violated");
            }
            else
            {
                MessageBox.Show(status.ToString() + " rows inserted");
            }
            DataRefresh();
        }

        private void uiDeleteData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> fields = new List<string>();
                List<List<string>> data = new List<List<string>>();
                List<string> datarow = new List<string>();
                int guid = Convert.ToInt32(((MasterObject)uiDataGrid.SelectedItem).Val0);
                DataDescription dd = new DataDescription();
                dd.OrgID = orgid;
                dd.ObjName = uiListOfObjects.SelectedItem.ToString();
                fields.Add("guid");
                dd.Fields = fields;
                datarow.Add(guid.ToString());
                data.Add(datarow);
                dd.Data = data;

                DataServiceClient client = new DataServiceClient();
                client.DeleteDataCompleted += new EventHandler<DeleteDataCompletedEventArgs>(client_DeleteDataCompleted);
                client.DeleteDataAsync(dd);
                uiBusyIndicator.IsBusy = true;
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_DeleteDataCompleted(object sender, DeleteDataCompletedEventArgs e)
        {
            uiBusyIndicator.IsBusy = true;
            MessageBox.Show(e.Result + " Rows deleted");
            DataRefresh();
        }

        private void uiUpdateData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string guid = ((MasterObject)uiDataGrid.SelectedItem).Val0;
                UpdateData updatedata = new UpdateData(dd, guid);
                updatedata.DataUpdated += new UpdateData.UpdateDataHandler(updatedata_DataUpdated);
                updatedata.Show();
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void updatedata_DataUpdated(string status)
        {
            DataRefresh();
        }
    }
}
