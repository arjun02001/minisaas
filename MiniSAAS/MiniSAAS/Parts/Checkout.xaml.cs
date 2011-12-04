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
using MiniSAAS.WorkflowServiceReference;
using MiniSAAS.ChildWindows;
using System.Diagnostics;

namespace MiniSAAS.Parts
{
    public partial class Checkout : UserControl
    {
        double totalprice = 0;
        int userid = 0;
        List<string> methods = new List<string>();

        public Checkout()
        {
            InitializeComponent();
            uiCheckoutGrid.Visibility = Visibility.Collapsed;
        }

        public Checkout(ProductDetail productdetail, int userid)
        {
            InitializeComponent();
            try
            {
                this.userid = userid;
                uiStaticProduct.Visibility = Visibility.Collapsed;
                uiProductsPanel.Children.Clear();
                productdetail.uiAddToCart.Visibility = Visibility.Collapsed;
                productdetail.uiBuyNow.Visibility = Visibility.Collapsed;
                uiProductsPanel.Children.Add(productdetail);
                totalprice = Convert.ToDouble(productdetail.uiPrice.Text.Substring(1));
                GetCustomizedMethods();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        public Checkout(List<ProductDetail> productdetails, int userid)
        {
            InitializeComponent();
            this.userid = userid;
            uiStaticProduct.Visibility = Visibility.Collapsed;
            try
            {
                List<string> productids = (from p in productdetails
                                           select p.ProductID).ToList<string>();
                new WorkflowServiceClient().DeleteCartAsync(App.orgid, userid.ToString(), productids);
                uiProductsPanel.Children.Clear();
                foreach (ProductDetail product in productdetails)
                {
                    product.uiAddToCart.Visibility = Visibility.Collapsed;
                    product.uiBuyNow.Visibility = Visibility.Collapsed;
                    uiProductsPanel.Children.Add(product);
                    totalprice += Convert.ToDouble(product.uiPrice.Text.Substring(1));
                }
                GetCustomizedMethods();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
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
                    if(w.WorkflowName.ToLower().Equals("checkout") || w.WorkflowName.ToLower().Equals("shipping"))
                    {
                        foreach (Method m in w.Methods)
                        {
                            methods.Add(m.MethodName.ToLower());
                        }
                    }
                }
                PerformCheckout();
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void PerformCheckout()
        {
            try
            {
                uiTotal.Text = "$" + totalprice;
                if (methods.Contains("calculatetax"))
                {
                    uiTaxPanel.Visibility = Visibility.Visible;
                    WorkflowServiceClient client = new WorkflowServiceClient();
                    client.CalculateTaxCompleted += (sender, e) =>
                        {
                            uiAfterTax.Text = "$" + e.Result;
                        };
                    client.CalculateTaxAsync(totalprice);
                }
                else
                {
                    uiTaxPanel.Visibility = Visibility.Collapsed;
                }

                if (methods.Contains("applycoupon"))
                {
                    uiCouponPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    uiCouponPanel.Visibility = Visibility.Collapsed;
                }

                if (methods.Contains("verifycreditcard"))
                {
                    uiCreditCardPanel.Visibility = Visibility.Visible;
                }
                else
                {
                    uiCreditCardPanel.Visibility = Visibility.Collapsed;
                }

                if (methods.Contains("getshippingvendors"))
                {
                    uiVendorPanel.Visibility = Visibility.Visible;
                    WorkflowServiceClient client = new WorkflowServiceClient();
                    client.GetShippingVendorsCompleted += (sender, e) =>
                        {
                            uiVendor.ItemsSource = e.Result;
                            uiVendor.SelectedIndex = 0;
                        };
                    client.GetShippingVendorsAsync();
                }
                else
                {
                    uiVendorPanel.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (methods.Contains("verifycreditcard"))
                {
                    WorkflowServiceClient client = new WorkflowServiceClient();
                    client.VerifyCreditCardCompleted += (sender1, e1) =>
                        {
                            if (e1.Result.ToLower().Equals("invalid card"))
                            {
                                new Message("Invalid Card").Show();
                            }
                        };
                    if (String.IsNullOrEmpty(uiCard.Text) || string.IsNullOrEmpty(uiExpiry.Text))
                    {
                        new Message("Please enter credit card information").Show();
                        return;
                    }
                    client.VerifyCreditCardAsync(uiCard.Text.Trim(), new DateTime(Convert.ToInt32(uiExpiry.Text.Split('/')[1]), Convert.ToInt32(uiExpiry.Text.Split('/')[0]), 1));
                }
                if (methods.Contains("validatezip"))
                {
                    WorkflowServiceClient client = new WorkflowServiceClient();
                    client.ValidateZIPCompleted += (sender1, e1) =>
                        {
                            if (!e1.Result)
                            {
                                new Message("Invalid zip").Show();
                            }
                        };
                    client.ValidateZIPAsync(uiZip.Text.Trim());
                }
                if (string.IsNullOrEmpty(uiName.Text))
                {
                    new Message("Enter name").Show();
                    return;
                }
                if (string.IsNullOrEmpty(uiAddress.Text))
                {
                    new Message("Enter address").Show();
                    return;
                }
                if (string.IsNullOrEmpty(uiZip.Text))
                {
                    new Message("Enter zip").Show();
                    return;
                }
                new Message("Thank you for your order").Show();
                this.Content = new AvailableProducts(Classes.UserType.User, userid);
            }
            catch (Exception ex)
            {
                 new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
            
        }

        private void uiCoupon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ( uiCoupon == null || uiCoupon.SelectedItem == null || uiCoupon.SelectedItem.ToString().ToLower().Equals("select"))
                {
                    return;
                }
                WorkflowServiceClient client = new WorkflowServiceClient();
                client.ApplyCouponCompleted += (sender1, e1) =>
                    {
                        uiAfterCoupon.Text = "$" + e1.Result;
                    };
                client.ApplyCouponAsync(totalprice, ((ComboBoxItem)uiCoupon.SelectedItem).Content.ToString());
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }
    }
}
