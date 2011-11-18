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
using MiniSAAS.ChildWindows;
using System.Diagnostics;
using MiniSAAS.Classes;
using MiniSAAS.WorkflowServiceReference;

namespace MiniSAAS.Parts
{
    public partial class ForgotPassword : ChildWindow
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void uiSendPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Utility.ValidateEmail(uiEmail.Text))
                {
                    return;
                }
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.ForgotPasswordCompleted += (sender1, e1) =>
                    {
                        if (!e1.Result)
                        {
                            new Message("An error occured").Show();
                            return;
                        }
                        new Message("Password sent to " + uiEmail.Text).Show();
                        this.DialogResult = true;
                    };
                client.ForgotPasswordAsync(App.orgid, uiEmail.Text);
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
    }
}

