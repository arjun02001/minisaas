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
using MiniSAAS.ChildWindows;

namespace MiniSAAS.UserControls
{
    public partial class WorkflowViewer : UserControl
    {
        int orgid;
        WorkflowDescription workflowdescription = null;

        public WorkflowViewer(int orgid)
        {
            InitializeComponent();
            this.orgid = orgid;
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.GetWorkflowsCompleted += new EventHandler<GetWorkflowsCompletedEventArgs>(client_GetWorkflowsCompleted);
                client.GetWorkflowsAsync(orgid);
                uiBusyIndicator.IsBusy = true;
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_GetWorkflowsCompleted(object sender, GetWorkflowsCompletedEventArgs e)
        {
            uiBusyIndicator.IsBusy = false;
            try
            {
                workflowdescription = e.Result;
                uiWorkflow.Items.Clear();
                foreach (Workflow w in workflowdescription.Workflows)
                {
                    uiWorkflow.Items.Add(w.WorkflowName);
                }
                uiWorkflow.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                uiBusyIndicator.IsBusy = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiAddWorkflow_Click(object sender, RoutedEventArgs e)
        {
            NewWorkflow newworkflow = new NewWorkflow(orgid);
            newworkflow.Closed += delegate(object sender1, EventArgs e1)
            {
                NewWorkflow nf = (NewWorkflow)sender1;
                if (nf.DialogResult == true)
                {
                    Refresh();
                }
            };
            newworkflow.Show();
        }

        private void uiDeleteWorkflow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int workflowid = (from p in workflowdescription.Workflows
                                 where p.WorkflowName == uiWorkflow.SelectedItem.ToString()
                                 select p.WorkflowID).Single();
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.DeleteWorkflowCompleted += delegate(object sender1, DeleteWorkflowCompletedEventArgs e1)
                {
                    if (e1.Result)
                    {
                        Refresh();
                    }
                };
                client.DeleteWorkflowAsync(orgid, workflowid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiUpdateSequence_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int methodid = ((Method)(uiMethodGrid.uiMethodDataGrid.SelectedItem)).MethodID;
                UpdateSequence updatesequence = new UpdateSequence(methodid);
                updatesequence.Closed += delegate(object sender1, EventArgs e1)
                {
                    UpdateSequence us = (UpdateSequence)sender1;
                    if (us.DialogResult == true)
                    {
                        Refresh();
                    }
                };
                updatesequence.Show();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiAddMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int workflowid = (from p in workflowdescription.Workflows
                                 where p.WorkflowName == uiWorkflow.SelectedItem.ToString()
                                 select p.WorkflowID).Single();
                NewMethod newmethod = new NewMethod(workflowid);
                newmethod.Closed += delegate(object sender1, EventArgs e1)
                {
                    NewMethod nm = (NewMethod)sender1;
                    if (nm.DialogResult == true)
                    {
                        Refresh();
                    }
                };
                newmethod.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiDeleteMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int methodid = ((Method)uiMethodGrid.uiMethodDataGrid.SelectedItem).MethodID;
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.DeleteMethodCompleted += delegate(object sender1, DeleteMethodCompletedEventArgs e1)
                {
                    if (e1.Result)
                    {
                        Refresh();
                    }
                };
                client.DeleteMethodAsync(methodid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiWorkflow_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (uiWorkflow.Items.Count == 0)
                {
                    return;
                }
                uiMethodGrid.uiMethodDataGrid.ItemsSource = (from p in workflowdescription.Workflows
                                                            where p.WorkflowName == uiWorkflow.SelectedItem.ToString()
                                                            select p).Single<Workflow>().Methods;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }
    }
}
