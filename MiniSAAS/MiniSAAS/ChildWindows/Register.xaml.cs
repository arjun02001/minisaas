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
    public partial class Register : ChildWindow
    {
        public Register()
        {
            InitializeComponent();
        }

        private void uiRegister_Click(object sender, RoutedEventArgs e)
        {
            PerformRegistration();
        }

        private void PerformRegistration()
        {
            try
            {
                if(!Utility.ValidateEmail(uiEmail.Text) || (!uiPassword.Password.Equals(uiReTypePassword.Password)))
                {
                    new Message("Verify EmailID and Password").Show();
                    return;
                }
                DataServiceClient client = new DataServiceClient();
                client.RegisterTenantCompleted += new EventHandler<RegisterTenantCompletedEventArgs>(client_RegisterTenantCompleted);
                client.RegisterTenantAsync(uiEmail.Text.ToLower(), uiPassword.Password);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_RegisterTenantCompleted(object sender, RegisterTenantCompletedEventArgs e)
        {
            try
            {
                if (e.Result == true)
                {
                    this.DialogResult = true;
                }
                else
                {
                    new Message("Email already exists").Show();
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiReTypePassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformRegistration();
            }
        }
    }
}

