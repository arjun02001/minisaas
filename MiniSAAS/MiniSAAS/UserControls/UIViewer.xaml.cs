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
using MiniSAAS.UIServiceReference;
using System.Windows.Media.Imaging;

namespace MiniSAAS.UserControls
{
    public partial class UIViewer : UserControl
    {
        public UIViewer()
        {
            InitializeComponent();
            GetControls();
        }

        private void GetControls()
        {
            try
            {
                UIServiceClient client = new UIServiceClient();
                client.GetControlsCompleted += new EventHandler<GetControlsCompletedEventArgs>(client_GetControlsCompleted);
                client.GetControlsAsync(App.orgid, ControlLocation.Header, ControlLocation.Header);
                client.GetControlsAsync(App.orgid, ControlLocation.Footer, ControlLocation.Footer);
                client.GetControlsAsync(App.orgid, ControlLocation.Sidebar, ControlLocation.Sidebar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void client_GetControlsCompleted(object sender, GetControlsCompletedEventArgs e)
        {
            if ((ControlLocation)e.UserState == ControlLocation.Header)
            {
                ProcessHeader(e.Result);
            }
            if ((ControlLocation)e.UserState == ControlLocation.Footer)
            {
                ProcessFooter(e.Result);
            }
            if ((ControlLocation)e.UserState == ControlLocation.Sidebar)
            {
                ProcessSidebar(e.Result);
            }
        }

        private void ProcessHeader(List<MiniSAAS.UIServiceReference.Control> controls)
        {
            try
            {
                if (controls.Count > 0)
                {
                    uiTitle.Text = controls[0].Text;
                    uiHeaderGrid.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(controls[0].BackgroundImage, UriKind.Absolute)) };
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ProcessFooter(List<MiniSAAS.UIServiceReference.Control> controls)
        {
            try
            {
                uiFooterPanel.Children.Clear();
                foreach (MiniSAAS.UIServiceReference.Control control in controls)
                {
                    HyperlinkButton hlb = new HyperlinkButton();
                    hlb.Margin = new Thickness(5);
                    hlb.Content = control.Text;
                    hlb.Tag = control.RedirectURL;
                    hlb.Click += delegate(object sender, RoutedEventArgs e)
                    {
                        uiBodyGrid.Children.Clear();
                        WebBrowser webbrowser = new WebBrowser();
                        uiBodyGrid.Children.Add(webbrowser);
                        webbrowser.Navigate(new Uri((sender as HyperlinkButton).Tag.ToString(), UriKind.RelativeOrAbsolute));
                    };
                    uiFooterPanel.Children.Add(hlb);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ProcessSidebar(List<MiniSAAS.UIServiceReference.Control> controls)
        {
            try
            {
                uiSidebarPanel.Children.Clear();
                foreach (MiniSAAS.UIServiceReference.Control control in controls)
                {
                    HyperlinkButton hlb = new HyperlinkButton();
                    hlb.Margin = new Thickness(5);
                    hlb.Content = control.Text;
                    hlb.Tag = control.RedirectURL;
                    hlb.Click += delegate(object sender, RoutedEventArgs e)
                    {
                        uiBodyGrid.Children.Clear();
                        WebBrowser webbrowser = new WebBrowser();
                        uiBodyGrid.Children.Add(webbrowser);
                        webbrowser.Navigate(new Uri((sender as HyperlinkButton).Tag.ToString(), UriKind.RelativeOrAbsolute));
                    };
                    uiSidebarPanel.Children.Add(hlb);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
