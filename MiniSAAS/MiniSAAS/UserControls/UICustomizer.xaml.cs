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

namespace MiniSAAS.UserControls
{
    public partial class UICustomizer : UserControl
    {
        public UICustomizer(int orgid)
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
            MessageBox.Show("hi");
        }

        private void uiTitle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("hello");
        }
    }
}
