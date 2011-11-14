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
using MiniSAAS.WorkflowServiceReference;

namespace MiniSAAS.ChildWindows
{
    public partial class NewWorkflow : ChildWindow
    {
        int orgid;
        public NewWorkflow(int orgid)
        {
            InitializeComponent();
            this.orgid = orgid;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (uiWorkflowName.Text.Trim().Equals(string.Empty))
            {
                return;
            }
            WorkflowServiceClient client = new WorkflowServiceClient();
            client.AddWorkflowCompleted += new EventHandler<AddWorkflowCompletedEventArgs>(client_AddWorkflowCompleted);
            client.AddWorkflowAsync(uiWorkflowName.Text.Trim(), orgid);
        }

        void client_AddWorkflowCompleted(object sender, AddWorkflowCompletedEventArgs e)
        {
            if (!e.Result)
            {
                new Message("A Workflow with this name already exists.").Show();
            }
            else
            {
                this.DialogResult = true;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

