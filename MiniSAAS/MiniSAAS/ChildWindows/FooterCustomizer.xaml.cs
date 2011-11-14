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

namespace MiniSAAS.ChildWindows
{
    public partial class FooterCustomizer : ChildWindow
    {
        public FooterCustomizer()
        {
            InitializeComponent();
            Refresh();
        }

        private void Refresh()
        {
            try
            {
                UIServiceClient client = new UIServiceClient();
                client.GetControlsCompleted += delegate(object sender, GetControlsCompletedEventArgs e)
                {
                    uiFooterLinksGrid.ItemsSource = e.Result;
                    uiText.Text = string.Empty;
                    uiRedirectURL.Text = string.Empty;
                };
                client.GetControlsAsync(App.orgid, ControlLocation.Footer);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiAddLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(uiText.Text.Trim()) || string.IsNullOrEmpty(uiRedirectURL.Text.Trim()))
                {
                    return;
                }
                MiniSAAS.UIServiceReference.Control control = new UIServiceReference.Control()
                {
                    ControlLocation = ControlLocation.Footer,
                    Text = uiText.Text.Trim(),
                    RedirectURL = uiRedirectURL.Text.Trim()
                };
                UIServiceClient client = new UIServiceClient();
                client.AddLinksCompleted += delegate(object sender1, AddLinksCompletedEventArgs e1)
                {
                    if (e1.Result)
                    {
                        Refresh();
                    }
                };
                client.AddLinksAsync(App.orgid, control);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiRemoveLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int controlid = ((MiniSAAS.UIServiceReference.Control)uiFooterLinksGrid.SelectedItem).ControlID;
                MiniSAAS.UIServiceReference.Control control = new UIServiceReference.Control()
                {
                    ControlID = controlid,
                    ControlLocation = ControlLocation.Footer
                };
                UIServiceClient client = new UIServiceClient();
                client.RemoveLinksCompleted += delegate(object sender1, RemoveLinksCompletedEventArgs e1)
                {
                    if (e1.Result)
                    {
                        Refresh();
                    }
                };
                client.RemoveLinksAsync(App.orgid, control);
            }
            catch (Exception ex)
            {
                new Message(new StackFrame().GetMethod().Name + Environment.NewLine + ex).Show();
            }
        }

        private void uiDone_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

