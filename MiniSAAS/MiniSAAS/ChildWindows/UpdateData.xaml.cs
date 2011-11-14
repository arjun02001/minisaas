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

namespace MiniSAAS.ChildWindows
{
    public partial class UpdateData : ChildWindow
    {
        public delegate void UpdateDataHandler(string status);
        public event UpdateDataHandler DataUpdated;

        DataDescription dd = null;
        string guid;
        List<string> selecteddatarow = new List<string>();

        public UpdateData(DataDescription dd, string guid)
        {
            InitializeComponent();
            try
            {
                this.dd = dd;
                this.guid = guid;
                foreach (List<string> datarow in dd.Data)
                {
                    if (datarow[0].Equals(this.guid))
                    {
                        selecteddatarow = datarow;
                        break;
                    }
                }

                int rowcount = 0;
                for (int i = 1; i < dd.Fields.Count; i++)
                {
                    TextBlock tbl = new TextBlock();
                    tbl.Text = dd.Fields[i];
                    tbl.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    tbl.SetValue(Grid.RowProperty, rowcount);

                    RowDefinition rd = new RowDefinition();
                    rd.Height = GridLength.Auto;
                    LayoutRoot.RowDefinitions.Add(rd);
                    LayoutRoot.Children.Add(tbl);

                    TextBox tb = new TextBox();
                    tb.Name = dd.Fields[i];
                    tb.Text = (selecteddatarow[i] == null) ? string.Empty : selecteddatarow[i];
                    tb.Width = 75;
                    tb.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    tb.SetValue(Grid.RowProperty, rowcount);

                    LayoutRoot.Children.Add(tb);
                    rowcount++;
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataDescription dd = new DataDescription();
                dd.OrgID = this.dd.OrgID;
                dd.ObjName = this.dd.ObjName;
                List<string> fields = new List<string>();
                fields.Add("guid");
                dd.Fields = fields;

                List<List<string>> data = new List<List<string>>();
                List<string> datarow = new List<string>();
                datarow.Add(this.guid);
                data.Add(datarow);
                dd.Data = data;

                DataServiceClient client = new DataServiceClient();
                client.DeleteDataCompleted += new EventHandler<DeleteDataCompletedEventArgs>(client_DeleteDataCompleted);
                client.DeleteDataAsync(dd);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_DeleteDataCompleted(object sender, DeleteDataCompletedEventArgs e)
        {
            try
            {
                if (e.Result == 0)
                {
                    new Message("Updation Failed").Show();
                    return;
                }

                DataDescription dd = new DataDescription();
                dd.OrgID = this.dd.OrgID;
                dd.ObjName = this.dd.ObjName;
                dd.Fields = this.dd.Fields;
                dd.Fields.RemoveAt(0);

                List<List<string>> data = new List<List<string>>();
                List<string> datarow = new List<string>();
                foreach (string s in dd.Fields)
                {
                    TextBox tb = FindName(s) as TextBox;
                    if (string.IsNullOrEmpty(tb.Text))
                    {
                        datarow.Add(null);
                    }
                    else
                    {
                        datarow.Add(tb.Text);
                    }
                }
                data.Add(datarow);
                dd.Data = data;

                DataServiceClient client = new DataServiceClient();
                client.InsertDataCompleted += new EventHandler<InsertDataCompletedEventArgs>(client_InsertDataCompleted);
                client.InsertDataAsync(dd);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_InsertDataCompleted(object sender, InsertDataCompletedEventArgs e)
        {
            if (e.Result == 1)
            {
                if (DataUpdated != null)
                {
                    DataUpdated(e.Result.ToString());
                    this.DialogResult = true;
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

