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
using MiniSAAS.DataServiceReference;
using System.Diagnostics;
using MiniSAAS.ChildWindows;
using MiniSAAS.Classes;
using MiniSAAS.UserControls;

namespace MiniSAAS
{
    public partial class MainPage : UserControl
    {
        public MainPage()
        {
            InitializeComponent();
            try
            {
                Login login = new Login();
                login.LoginSuccess += delegate(int orgid)
                {
                    App.orgid = orgid;
                    App.GoToXAML(new DataViewer(orgid));
                };
                login.TenantSelected += delegate(int orgid)
                {
                    App.orgid = orgid;
                    App.GoToXAML(new UIViewer());
                };
                login.Show();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }
    }
}
