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
    public partial class AddData : ChildWindow
    {
        public delegate void AddDataHandler(int status);
        public event AddDataHandler DataAdded;

        ObjectDescription od = null;
        List<string> fields = new List<string>();
        List<List<string>> data = new List<List<string>>();

        public AddData(ObjectDescription od)
        {
            InitializeComponent();
            try
            {
                int i = 0;
                this.od = od;
                foreach (KeyValuePair<string, string> field in od.Fields)
                {
                    fields.Add(field.Key);

                    TextBlock tbl = new TextBlock();
                    tbl.Text = field.Key;
                    tbl.SetValue(Grid.RowProperty, i++);

                    RowDefinition rd = new RowDefinition();
                    rd.Height = GridLength.Auto;
                    LayoutRoot.RowDefinitions.Add(rd);
                    LayoutRoot.Children.Add(tbl);

                    TextBox tb = new TextBox();
                    tb.Name = field.Key;
                    tb.Width = 75;
                    tb.SetValue(Grid.RowProperty, i++);

                    rd = new RowDefinition();
                    rd.Height = GridLength.Auto;
                    LayoutRoot.RowDefinitions.Add(rd);
                    LayoutRoot.Children.Add(tb);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DataDescription dd = new DataDescription();
                List<string> datarow = new List<string>();
                dd.Fields = fields;
                foreach (string s in fields)
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
                dd.OrgID = od.OrgID;
                dd.ObjName = od.ObjName;

                DataServiceClient client = new DataServiceClient();
                client.InsertDataCompleted += new EventHandler<InsertDataCompletedEventArgs>(client_InsertDataCompleted);
                client.InsertDataAsync(dd);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
            this.DialogResult = true;
        }

        void client_InsertDataCompleted(object sender, InsertDataCompletedEventArgs e)
        {
            if (DataAdded != null)
            {
                DataAdded(e.Result);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

