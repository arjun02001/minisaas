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
    public partial class HeaderCustomizer : ChildWindow
    {
        MiniSAAS.UIServiceReference.Control control;
        public HeaderCustomizer()
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
                    if (e.Result.Count > 0)
                    {
                        control = e.Result[0];
                        uiTitle.Text = control.Text;
                        uiBackgroundImageURL.Text = control.BackgroundImage;
                    }
                };
                client.GetControlsAsync(App.orgid, ControlLocation.Header);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (control == null)
                {
                    control = new UIServiceReference.Control();
                }
                control.Text = uiTitle.Text.Trim();
                control.BackgroundImage = uiBackgroundImageURL.Text.Trim();
                control.ControlLocation = ControlLocation.Header;
                UIServiceClient client = new UIServiceClient();
                client.UpdateHeaderCompleted += delegate(object sender1, UpdateHeaderCompletedEventArgs e1)
                {
                    Refresh();
                    new Message("Update " + ((e1.Result) ? "successful" : "failed")).Show();
                };
                client.UpdateHeaderAsync(App.orgid, control);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

