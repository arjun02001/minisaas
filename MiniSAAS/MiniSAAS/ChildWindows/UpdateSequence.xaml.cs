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
using MiniSAAS.WorkflowServiceReference;

namespace MiniSAAS.ChildWindows
{
    public partial class UpdateSequence : ChildWindow
    {
        int methodid;
        public UpdateSequence(int methodid)
        {
            InitializeComponent();
            this.methodid = methodid;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (uiNewSequence.Text.Trim().Equals(string.Empty))
                {
                    return;
                }
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.UpdateMethodSequenceCompleted += delegate(object sender1, UpdateMethodSequenceCompletedEventArgs e1)
                {
                    if (e1.Result)
                    {
                        this.DialogResult = true;
                    }
                };
                client.UpdateMethodSequenceAsync(methodid, Convert.ToInt32(uiNewSequence.Text.Trim()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

