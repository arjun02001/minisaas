﻿using System;
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

namespace MiniSAAS.Parts
{
    public partial class ProductDetail : UserControl
    {
        public int ProductID { get; set; }
        int userid;

        public ProductDetail(int productid, int userid)
        {
            InitializeComponent();
            ProductID = productid;
            this.userid = userid;
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
                client.AddToCartAsync(App.orgid, userid.ToString(), ProductID.ToString());
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

            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }
    }
}
