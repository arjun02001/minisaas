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
using MiniSAAS.WorkflowServiceReference;
using System.Windows.Media.Imaging;

namespace MiniSAAS.Parts
{
    public partial class ProductDetail : UserControl
    {
        int userid;
        List<string> product;

        public delegate void BuyNowClickedHandler(List<string> product);
        public event BuyNowClickedHandler BuyNowClicked;

        public ProductDetail(List<string> product, int userid)
        {
            InitializeComponent();
            try
            {
                this.product = product;
                this.userid = userid;
                uiName.Text = product[2];
                uiPrice.Text = "$" + product[3];
                uiImage.Source = new BitmapImage(new Uri(product[4], UriKind.Absolute));
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiAddToCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.AddToCartCompleted += (sender1, e1) =>
                    {
                        if (!e1.Result)
                        {
                            new Message("An error occured").Show();
                        }
                        else
                        {
                            new Message("Product added to cart").Show();
                        }
                    };
                client.AddToCartAsync(App.orgid, userid.ToString(), product[1]);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiBuyNow_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BuyNowClicked != null)
                {
                    BuyNowClicked(product);
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }
    }
}
