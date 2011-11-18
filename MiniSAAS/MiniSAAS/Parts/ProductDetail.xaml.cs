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

namespace MiniSAAS.Parts
{
    public partial class ProductDetail : UserControl
    {
        public string ProductID { get; set; }

        public ProductDetail()
        {
            InitializeComponent();
        }

        private void uiAddToCart_Click(object sender, RoutedEventArgs e)
        {

        }

        private void uiBuyNow_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
