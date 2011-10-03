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
using MiniSAAS.Classes;

namespace MiniSAAS.ChildWindows
{
    public partial class Field : ChildWindow
    {
        public delegate void FieldAddedHandler(string fieldname, string datatype);
        public event FieldAddedHandler FieldAdded;

        public Field()
        {
            InitializeComponent();
            PopulateDataType();
        }

        private void PopulateDataType()
        {
            try
            {
                uiDataType.Items.Add(DataType.VARCHAR);
                uiDataType.Items.Add(DataType.INT);
                uiDataType.Items.Add(DataType.DECIMAL);
                uiDataType.Items.Add(DataType.DATE);
                uiDataType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (FieldAdded != null)
            {
                FieldAdded(uiFieldName.Text, uiDataType.SelectedItem.ToString());
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (FieldAdded != null)
            {
                FieldAdded(string.Empty, string.Empty);
            }
            this.DialogResult = false;
        }
    }
}

