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
using MiniSAAS.Classes;

namespace MiniSAAS.UserControls
{
    public partial class Header : UserControl
    {
        public Header()
        {
            InitializeComponent();
        }

        private void uiData_Click(object sender, RoutedEventArgs e)
        {
            App.GoToXAML(new DataViewer(App.orgid));
        }

        private void uiWorkflow_Click(object sender, RoutedEventArgs e)
        {
            App.GoToXAML(new WorkflowViewer(App.orgid));
        }

        private void uiUICustomizer_Click(object sender, RoutedEventArgs e)
        {
            App.GoToXAML(new UICustomizer());
        }

        private void uiUIViewer_Click(object sender, RoutedEventArgs e)
        {
            App.GoToXAML(new UIViewer(UserType.Tenant));
        }

        private void uiLogout_Click(object sender, RoutedEventArgs e)
        {
            App.GoToXAML(new MainPage());
        }
    }
}
