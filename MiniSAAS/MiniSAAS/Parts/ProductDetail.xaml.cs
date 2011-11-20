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
        public string ProductID { get; set; }

        public delegate void BuyNowClickedHandler(ProductDetail productdetail);
        public event BuyNowClickedHandler BuyNowClicked;
        public event BuyNowClickedHandler AddedToCart;

        public ProductDetail(List<string> product, int userid)
        {
            InitializeComponent();
            try
            {
                ProductID = product[1];
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

        public ProductDetail(ProductDetail productdetail)
        {
            InitializeComponent();
            this.ProductID = productdetail.ProductID;
            this.userid = productdetail.userid;
            this.uiName.Text = productdetail.uiName.Text;
            this.uiPrice.Text = productdetail.uiPrice.Text;
            this.uiImage.Source = productdetail.uiImage.Source;
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
                            if (AddedToCart != null)
                            {
                                AddedToCart(this);
                            }
                        }
                    };
                client.AddToCartAsync(App.orgid, userid.ToString(), ProductID);
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
                    BuyNowClicked(this);
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }
    }
}
