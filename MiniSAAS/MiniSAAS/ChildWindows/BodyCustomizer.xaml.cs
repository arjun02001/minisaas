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
using System.Diagnostics;
using System.Reflection;
using MiniSAAS.UIServiceReference;

namespace MiniSAAS.ChildWindows
{
    public partial class BodyCustomizer : ChildWindow
    {
        public BodyCustomizer()
        {
            InitializeComponent();
            Refresh();
            PopulateAvailablePages();
        }

        private void PopulateAvailablePages()
        {
            try
            {
                List<Page> pages = new List<Page>();
                pages.Add(new Page() { Text = "AvailableProducts", RedirectURL = "AvailableProducts" });
                pages.Add(new Page() { Text = "Checkout", RedirectURL = "Checkout" });
                pages.Add(new Page() { Text = "Login", RedirectURL = "Login" });
                pages.Add(new Page() { Text = "Registration", RedirectURL = "Registration" });
                uiAvailablePagesGrid.ItemsSource = pages;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void Refresh()
        {
            try
            {
                UIServiceClient client = new UIServiceClient();
                client.GetControlsCompleted += delegate(object sender, GetControlsCompletedEventArgs e)
                {
                    uiExistingPagesGrid.ItemsSource = e.Result;
                };
                client.GetControlsAsync(App.orgid, ControlLocation.Body);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void uiRemovePage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MiniSAAS.UIServiceReference.Control control = uiExistingPagesGrid.SelectedItem as MiniSAAS.UIServiceReference.Control;
                UIServiceClient client = new UIServiceClient();
                client.RemovePageCompleted += delegate(object sender1, RemovePageCompletedEventArgs e1)
                {
                    Refresh();
                };
                client.RemovePageAsync(App.orgid, control);
            }
            catch (Exception)
            {
            }
        }

        private void uiAddPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Page page = uiAvailablePagesGrid.SelectedItem as Page;
                MiniSAAS.UIServiceReference.Control control = new UIServiceReference.Control()
                {
                    ControlLocation = ControlLocation.Body,
                    Text = page.Text,
                    RedirectURL = page.RedirectURL
                };
                UIServiceClient client = new UIServiceClient();
                client.AddPageCompleted += delegate(object sender1, AddPageCompletedEventArgs e1)
                {
                    Refresh();
                };
                client.AddPageAsync(App.orgid, control);
            }
            catch (Exception ex)
            {
            }
        }

        private void uiPreviewPage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string page = (uiAvailablePagesGrid.SelectedItem as Page).RedirectURL;
                Message message = new Message(string.Empty);
                message.Width = 600;
                message.Height = 500;
                message.LayoutRoot.Children.Clear();
                message.LayoutRoot.Children.Add((UIElement)System.Activator.CreateInstance(Type.GetType("MiniSAAS.Parts." + page)));
                message.Show();
            }
            catch (Exception ex)
            {
            }
        }

        private void uiDone_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }

    public class Page
    {
        public string Text { get; set; }
        public string RedirectURL { get; set; }
    }
}

