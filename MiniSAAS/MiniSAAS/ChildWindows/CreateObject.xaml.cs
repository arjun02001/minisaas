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
using System.Diagnostics;
using MiniSAAS.DataServiceReference;

namespace MiniSAAS.ChildWindows
{
    public partial class CreateObject : ChildWindow
    {
        int orgid = 0;
        public delegate void ObjectCreatedHandler(bool status);
        public event ObjectCreatedHandler ObjectCreated;

        public CreateObject(int orgid)
        {
            InitializeComponent();
            this.orgid = orgid;
        }

        private void uiSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(uiObjectName.Text) || uiFields.Items.Count == 0)
                {
                    return;
                }
                ObjectDescription od = new ObjectDescription();
                od.OrgID = orgid;
                od.ObjName = uiObjectName.Text.ToLower();
                od.PrimaryKey = uiPrimaryKey.Text.ToLower();
                Dictionary<string, string> fields = new Dictionary<string, string>();
                foreach (string s in uiFields.Items)
                {
                    fields.Add(s.Split(' ')[0].ToLower(), s.Split(' ')[1]);
                }
                od.Fields = fields;
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
            try
            {
                if (ObjectCreated != null)
                {
                    ObjectCreated(e.Result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
            this.DialogResult = true;
        }

        private void uiCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void uiAddField_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Field field = new Field();
                field.FieldAdded += new Field.FieldAddedHandler(field_FieldAdded);
                field.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void field_FieldAdded(string fieldname, string datatype)
        {
            try
            {
                if(fieldname.Equals(string.Empty) || datatype.Equals(string.Empty))
                {
                    return;
                }
                uiFields.Items.Add(fieldname.ToLower() + " " + datatype);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiMakePrimary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (uiFields.SelectedItem == null)
                {
                    return;
                }
                uiPrimaryKey.Text = uiFields.SelectedItem.ToString().Split(' ')[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }
    }
}

