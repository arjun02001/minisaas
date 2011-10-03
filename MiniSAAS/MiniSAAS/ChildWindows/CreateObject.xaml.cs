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

namespace MiniSAAS.ChildWindows
{
    public partial class CreateObject : ChildWindow
    {
        public CreateObject()
        {
            InitializeComponent();
        }

        private void uiSave_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void uiCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

