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
using MiniSAAS.DataServiceReference;

namespace MiniSAAS.ChildWindows
{
    public partial class Login : ChildWindow
    {
        public delegate void LoginSuccessHandler(int orgid);
        public event LoginSuccessHandler LoginSuccess;
        public event LoginSuccessHandler TenantSelected;
        public Login()
        {
            InitializeComponent();
            GetOrgs();
        }

        private void uiRegister_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Register register = new Register();
                register.Show();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiLogin_Click(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }

        private void uiPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLogin();
            }
        }

        private void PerformLogin()
        {
            try
            {
                if (!Utility.ValidateEmail(uiEmail.Text))
                {
                    new Message("Verify EmailID and Password").Show();
                    return;
                }
                DataServiceClient client = new DataServiceClient();
                client.LoginTenantCompleted += new EventHandler<LoginTenantCompletedEventArgs>(client_LoginTenantCompleted);
                client.LoginTenantAsync(uiEmail.Text.ToLower(), uiPassword.Password);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_LoginTenantCompleted(object sender, LoginTenantCompletedEventArgs e)
        {
            try
            {
                if (e.Result > 0)
                {
                    if (LoginSuccess != null)
                    {
                        LoginSuccess(e.Result);
                    }
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Verify EmailID and Password");
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void GetOrgs()
        {
            try
            {
                DataServiceClient client = new DataServiceClient();
                client.GetOrgsCompleted += delegate(object sender, GetOrgsCompletedEventArgs e)
                {
                    foreach (string s in e.Result)
                    {
                        uiTenants.Items.Add(s);
                    }
                    if (e.Result.Count > 0)
                    {
                        uiTenants.SelectedIndex = 0;
                    }
                };
                client.GetOrgsAsync();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiUserLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TenantSelected != null)
                {
                    TenantSelected(Convert.ToInt32(uiTenants.SelectedItem));
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }
    }
}

