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
using System.Windows.Browser;

namespace BusinessSystemsApp.Helpers
{
    public partial class MessageBox : ChildWindow
    {
        public MessageBox()
        {
            InitializeComponent();
        }

        public MessageBox(String _title, String _message, String _details)
        {
            InitializeComponent();
            
            if (WebContext.Current.User.IsAuthenticated)
            {
                //Connect to web service for get configurations from server
                System.ServiceModel.BasicHttpBinding bindingConf = new System.ServiceModel.BasicHttpBinding();

                Uri address = null;

                if (App.Current.Host.Source.AbsoluteUri.StartsWith("https"))
                {
                    bindingConf.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

                    address = new Uri(Application.Current.Host.Source, "../Logger.svc");
                }
                else
                {
                    address = new Uri(Application.Current.Host.Source, "../Logger.svc");
                }

                System.ServiceModel.EndpointAddress addressLog = new System.ServiceModel.EndpointAddress(address.AbsoluteUri);

                LoggerServiceReference.LoggerClient logClient = new LoggerServiceReference.LoggerClient(bindingConf, addressLog);
                logClient.LoggExceptionAsync(HtmlPage.BrowserInformation.Name + " v" + HtmlPage.BrowserInformation.BrowserVersion, _message + " (" + _details + ")", WebContext.Current.User.Name);
                logClient.LoggExceptionCompleted += new EventHandler<LoggerServiceReference.LoggExceptionCompletedEventArgs>(logClient_LoggExceptionCompleted);

           
                this.Title = _title;
                expander1.Header = _message;
                errorTextBox.Text = _details;
                expander1.IsExpanded = false;
            }
            else
            {
                //expander1.Header = "Authentication expired!";
                //errorTextBox.Text = "Timeout period of 20 minutes expiren and user is automatically logged of. Press OK button to reload and then sign up again.";
                expander1.Header = _message;
                errorTextBox.Text = _details;
 
            }

            CancelButton.Visibility = System.Windows.Visibility.Collapsed;
        }

        void logClient_LoggExceptionCompleted(object sender, LoggerServiceReference.LoggExceptionCompletedEventArgs e)
        {
            
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (WebContext.Current.User.IsAuthenticated)
                this.DialogResult = true;
            else
                System.Windows.Browser.HtmlPage.Document.Submit();
                
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

