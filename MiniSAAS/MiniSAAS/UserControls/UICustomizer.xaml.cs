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

namespace MiniSAAS.UserControls
{
    public partial class UICustomizer : UserControl
    {
        public UICustomizer()
        {
            InitializeComponent();
        }

        private void ui_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void ui_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void uiHeaderGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new HeaderCustomizer().Show();
        }

        private void uiSideBarGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new SidebarCustomizer().Show();
        }

        private void uiFooterGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            new FooterCustomizer().Show();
        }
    }
}
