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
using MiniSAAS.DataServiceReference;
using MiniSAAS.Classes;
using System.Windows.Media.Imaging;
using MiniSAAS.WorkflowServiceReference;

namespace MiniSAAS.Parts
{
    public partial class AvailableProducts : UserControl
    {
        UserType usertype;
        int userid;
        bool iscartflow = false;

        public AvailableProducts(UserType usertype, int userid)
        {
            InitializeComponent();
            this.usertype = usertype;
            this.userid = userid;
            uiStaticProducts.Visibility = System.Windows.Visibility.Collapsed;
            GetCustomizedMethods();
        }

        public AvailableProducts()
        {
            InitializeComponent();
            uiProductsGrid.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void GetCustomizedMethods()
        {
            try
            {
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.GetWorkflowsCompleted += new EventHandler<GetWorkflowsCompletedEventArgs>(client_GetWorkflowsCompleted);
                client.GetWorkflowsAsync(App.orgid);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_GetWorkflowsCompleted(object sender, GetWorkflowsCompletedEventArgs e)
        {
            try
            {
                foreach (Workflow w in e.Result.Workflows)
                {
                    if (w.WorkflowName.ToLower().Equals("cart"))
                    {
                        iscartflow = true;
                        break;
                    }
                }
                GetProducts();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void GetProducts()
        {
            try
            {
                DataServiceClient client = new DataServiceClient();
                client.GetObjectCollectionCompleted += new EventHandler<GetObjectCollectionCompletedEventArgs>(client_GetObjectCollectionCompleted);
                client.GetObjectCollectionAsync(App.orgid);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_GetObjectCollectionCompleted(object sender, GetObjectCollectionCompletedEventArgs e)
        {
            try
            {
                ObjectDescription od = (from p in e.Result
                                        where p.ObjName.ToLower().Equals("product")
                                        select p).Single();
                DataServiceClient client = new DataServiceClient();
                client.ViewDataCompleted += new EventHandler<ViewDataCompletedEventArgs>(client_ViewDataCompleted);
                client.ViewDataAsync(od);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        void client_ViewDataCompleted(object sender, ViewDataCompletedEventArgs e)
        {
            try
            {
                DataDescription dd = e.Result;
                uiProductsPanel.Children.Clear();

                foreach (List<string> p in dd.Data)
                {
                    ProductDetail pd = new ProductDetail(p, userid);
                    pd.BuyNowClicked += (product) =>
                        {
                            this.Content = new Checkout(product);
                        };
                    pd.uiAddToCart.Visibility = (iscartflow) ? Visibility.Visible : Visibility.Collapsed;
                    uiProductsPanel.Children.Add(pd);
                }
                uiCheckout.Visibility = (iscartflow) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiCheckout_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
