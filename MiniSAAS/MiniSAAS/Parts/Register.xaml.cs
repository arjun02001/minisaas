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
    public partial class Register : ChildWindow
    {
        public Register()
        {
            InitializeComponent();
        }

        private void uiSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Utility.ValidateEmail(uiEmail.Text) || (uiPassword.Password != uiConfirmPassword.Password) || string.IsNullOrEmpty(uiPassword.Password))
                {
                    new Message("Please verify Email / Password").Show();
                    return;
                }
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.RegisterCompleted += (sender1, e1) =>
                    {
                        if (!e1.Result)
                        {
                            new Message("An error occured").Show();
                            return;
                        }
                        this.DialogResult = true;
                    };
                client.RegisterAsync(App.orgid, uiEmail.Text, uiPassword.Password);
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

