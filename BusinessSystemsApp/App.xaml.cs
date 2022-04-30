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
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Threading;

namespace BusinessSystemsApp
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += this.Application_UnhandledException;

            InitializeComponent();

            WebContext context = new WebContext();
            context.Authentication = new FormsAuthentication();
            ApplicationLifetimeObjects.Add(context);

            // Attach the event handler.
            System.Windows.Browser.HtmlPage.Document.AttachEvent("oncontextmenu", this.OnContextMenu);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.RootVisual = new MainPage();
        }


        // Here is the event handler that intercepts the default context 
        // menu and display our dialog box.
        private void OnContextMenu(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            //MessageBox.Show("You clicked at " + e.OffsetX + "," + e.OffsetY);

            e.PreventDefault();
        }


        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // If the app is running outside of the debugger then report the exception using
            // a ChildWindow control.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTE: This will allow the application to continue running after an exception has been thrown
                // but not handled. 
                // For production applications this error handling should be replaced with something that will 
                // report the error to the website and stop the application.
                e.Handled = true;
                ChildWindow errorWin = new Helpers.MessageBox("Error", "Unhandled error occured", e.ExceptionObject.Message);
                errorWin.Show();
            }
        }
    }
}