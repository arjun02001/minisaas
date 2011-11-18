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
using MiniSAAS.Classes;
using MiniSAAS.ChildWindows;
using System.Diagnostics;

namespace MiniSAAS.Parts
{
    public partial class Login : UserControl
    {
        public Login()
        {
            InitializeComponent();
        }

        private void uiLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Utility.ValidateEmail(uiEmail.Text) || string.IsNullOrEmpty(uiEmail.Text) || string.IsNullOrEmpty(uiPassword.Password))
                {
                    return;
                }
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.LoginCompleted += (sender1, e1) =>
                    {
                        if (e1.Result == -1)
                        {
                            new Message("Invalid Credentials").Show();
                        }
                        else
                        {
                            this.Content = new AvailableProducts(UserType.User, e1.Result);
                        }
                    };
                client.LoginAsync(App.orgid, uiEmail.Text.Trim(), uiPassword.Password.Trim());
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new Register().Show();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                new ForgotPassword().Show();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }
    }
}
