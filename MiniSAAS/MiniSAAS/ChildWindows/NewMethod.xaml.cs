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
    public partial class NewMethod : ChildWindow
    {
        int workflowid;
        List<Method> methods = new List<Method>();

        public NewMethod(int workflowid)
        {
            InitializeComponent();
            this.workflowid = workflowid;
            GetCommonMethods();
        }

        private void GetCommonMethods()
        {
            try
            {
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.GetWorkflowsCompleted += new EventHandler<GetWorkflowsCompletedEventArgs>(client_GetWorkflowsCompleted);
                client.GetWorkflowsAsync(0);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_GetWorkflowsCompleted(object sender, GetWorkflowsCompletedEventArgs e)
        {
            try
            {
                uiCommonMethods.Items.Clear();
                foreach (Workflow w in e.Result.Workflows)
                {
                    foreach (Method m in w.Methods)
                    {
                        methods.Add(m);
                        uiCommonMethods.Items.Add(m.ReturnType + " " + m.MethodName + " (" + m.Parameters + ")");
                    }
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void GetURLMethods()
        {
            try
            {
                if (uiURL.Text.Trim().Equals(string.Empty))
                {
                    return;
                }
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.GetURLMethodsCompleted += new EventHandler<GetURLMethodsCompletedEventArgs>(client_GetURLMethodsCompleted);
                client.GetURLMethodsAsync(uiURL.Text.Trim());
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_GetURLMethodsCompleted(object sender, GetURLMethodsCompletedEventArgs e)
        {
            try
            {
                uiURLMethods.Items.Clear();
                foreach (Method m in e.Result)
                {
                    methods.Add(m);
                    uiURLMethods.Items.Add(m.ReturnType + " " + m.MethodName + " (" + m.Parameters + ")");
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiGetURLMethods_Click(object sender, RoutedEventArgs e)
        {
            GetURLMethods();
        }

        private void uiSelectCommonMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uiSelectedMethods.Items.Add(uiCommonMethods.SelectedItem.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void uiSelectURLMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uiSelectedMethods.Items.Add(uiURLMethods.SelectedItem.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void uiDone_Click(object sender, RoutedEventArgs e)
        {
            List<Method> selectedmethods = new List<Method>();
            try
            {
                if (uiSelectedMethods.Items.Count == 0)
                {
                    return;
                }
                foreach (string s in uiSelectedMethods.Items)
                {
                    string methodname = s.Split(' ')[1];
                    Method m = (from p in methods
                                where p.MethodName == methodname
                                select p).Single();
                    selectedmethods.Add(m);
                }
                List<Workflow> workflows = new List<Workflow>();
                workflows.Add(new Workflow() { Methods = selectedmethods, WorkflowID = workflowid });
                WorkflowDescription workflowdescription = new WorkflowDescription() { Workflows = workflows };
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.AddMethodsCompleted += delegate(object sender1, AddMethodsCompletedEventArgs e1)
                {
                    if (e1.Result)
                    {
                        this.DialogResult = true;
                    }
                };
                client.AddMethodsAsync(workflowdescription);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void uiRemoveSelectedMethod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uiSelectedMethods.Items.RemoveAt(uiSelectedMethods.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }
    }
}

