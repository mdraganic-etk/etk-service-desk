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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
///using System.Windows.Controls
using BusinessSystemsApp.Web;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Browser;
using System.Threading;

namespace BusinessSystemsApp
{
    public partial class MainPage : UserControl
    {
        public static DateTime ServerDateTime = DateTime.Now;

        public static string AttachmentPath = null;

        public static string ReportPath = null;

        public static string ApplicationUrl = null;

        System.Windows.Threading.DispatcherTimer timer = null;

        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        LoadOperation loUsers = null;
        LoadOperation loUserAssignment = null;

        private bool isDirty = true;

        public MainPage()
        {

            InitializeComponent();

            LayoutRoot.Height = App.Current.Host.Content.ActualHeight;

            scrollViewer1.Height = App.Current.Host.Content.ActualHeight - 100;

            scrollViewer1.Width = App.Current.Host.Content.ActualWidth - 250;

            App.Current.Host.Content.Resized += new EventHandler(Content_Resized);

            try
            {
                //Connect to web service for get configurations from server
                System.ServiceModel.BasicHttpBinding bindingConf = new System.ServiceModel.BasicHttpBinding();

                Uri address = null;

                if (App.Current.Host.Source.AbsoluteUri.StartsWith("https"))
                {
                   bindingConf.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

                   address = new Uri(Application.Current.Host.Source, "../Configuration.svc");
                }
                else
                {
                    address = new Uri(Application.Current.Host.Source, "../Configuration.svc");
                }

                System.ServiceModel.EndpointAddress addressConf = new System.ServiceModel.EndpointAddress(address.AbsoluteUri);

               

                ConfigurationServiceReference.ConfigurationClient confClient = new ConfigurationServiceReference.ConfigurationClient(bindingConf, addressConf);
                confClient.GetAppSettingsValueAsync("attachPath");
                confClient.GetAppSettingsValueCompleted += new EventHandler<ConfigurationServiceReference.GetAppSettingsValueCompletedEventArgs>(confClient_GetAppSettingsValueCompleted);

                ConfigurationServiceReference.ConfigurationClient confClient2 = new ConfigurationServiceReference.ConfigurationClient(bindingConf, addressConf);
                confClient2.GetAppSettingsValueAsync("reportPath");
                confClient2.GetAppSettingsValueCompleted += new EventHandler<ConfigurationServiceReference.GetAppSettingsValueCompletedEventArgs>(confClient2_GetAppSettingsValueCompleted);

                ConfigurationServiceReference.ConfigurationClient confClient3 = new ConfigurationServiceReference.ConfigurationClient(bindingConf, addressConf);
                confClient3.GetAppSettingsValueAsync("applicationUrl");
                confClient3.GetAppSettingsValueCompleted += new EventHandler<ConfigurationServiceReference.GetAppSettingsValueCompletedEventArgs>(confClient3_GetAppSettingsValueCompleted);


                //Conect to web service to get server datetime
                System.ServiceModel.BasicHttpBinding bindingDate = new System.ServiceModel.BasicHttpBinding();

                Uri addressDateTime = null;

                if (App.Current.Host.Source.AbsoluteUri.StartsWith("https"))
                {
                    bindingDate.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                   
                    addressDateTime = new Uri(Application.Current.Host.Source, "../DateTimeService.svc");
                }
                else
                {
                    addressDateTime = new Uri(Application.Current.Host.Source, "../DateTimeService.svc");
                }

                System.ServiceModel.EndpointAddress addressDate = new System.ServiceModel.EndpointAddress(addressDateTime.AbsoluteUri);

                DateTimeServiceReference.DateTimeServiceClient client = new DateTimeServiceReference.DateTimeServiceClient(bindingDate, addressDate);

                client.GetDateTimeAsync();
                client.GetDateTimeCompleted += new EventHandler<DateTimeServiceReference.GetDateTimeCompletedEventArgs>(client_GetDateTimeCompleted);


                //RegisterOnBeforeUnload();

                this.LayoutRoot.Visibility = System.Windows.Visibility.Collapsed;

                WebContext.Current.Authentication.LoggedOut += new EventHandler<System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs>(Authentication_LoggedOut);

                Loaded += OnLoaded;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }

        }


        void Content_Resized(object sender, EventArgs e)
        {
            LayoutRoot.Height = App.Current.Host.Content.ActualHeight;

            scrollViewer1.Height = App.Current.Host.Content.ActualHeight - 100;

            scrollViewer1.Width = App.Current.Host.Content.ActualWidth - 250;
        }

        void  confClient3_GetAppSettingsValueCompleted(object sender, ConfigurationServiceReference.GetAppSettingsValueCompletedEventArgs e)
        {
            try
            {
                ApplicationUrl = e.Result;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
          
        }

        void confClient2_GetAppSettingsValueCompleted(object sender, ConfigurationServiceReference.GetAppSettingsValueCompletedEventArgs e)
        {
            try
            {
                ReportPath = e.Result;
            
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        void confClient_GetAppSettingsValueCompleted(object sender, ConfigurationServiceReference.GetAppSettingsValueCompletedEventArgs e)
        {
            try
            {
                 AttachmentPath = e.Result;
            
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }
        
        //GetServer time
        void client_GetDateTimeCompleted(object sender, DateTimeServiceReference.GetDateTimeCompletedEventArgs e)
        {
            try
            {
                ServerDateTime = e.Result;

                timer = new System.Windows.Threading.DispatcherTimer();
                timer.Interval = new TimeSpan(0, 0, 15);
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //Increase server date time every 15 seconds
        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                ServerDateTime = ServerDateTime.AddSeconds(15);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


        void Authentication_LoggedOut(object sender, System.ServiceModel.DomainServices.Client.ApplicationServices.AuthenticationEventArgs e)
        {

            try
            {
                if (!WebContext.Current.Authentication.User.Identity.IsAuthenticated)
                {
                    //When user is loged out then colapse tree view and naviget to null
                    ContentFrame.Navigate(new Uri("/Views/Home.xaml", UriKind.Relative));
                    
                    //treeView1.CollapseAll();


                    LayoutRoot.Visibility = System.Windows.Visibility.Collapsed;

                    ChildForms.Login login = new ChildForms.Login();

                    login.Closed += (s, e2) =>
                    {
                        userDomainDataSource.QueryName = "GetUserWithUsername";
                        userDomainDataSource.QueryParameters.Clear();

                        String username = WebContext.Current.Authentication.User.Identity.Name;

                        var par = new Parameter();
                        par.ParameterName = "_userName";
                        par.Value = username;

                        userDomainDataSource.QueryParameters.Clear();
                        userDomainDataSource.QueryParameters.Add(par);

                        userDomainDataSource.Load();
                    };

                    login.Show();
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!WebContext.Current.Authentication.User.Identity.IsAuthenticated)
                {
                    ChildForms.Login login = new ChildForms.Login();

                    login.Closed += (s, e2) =>
                    {
                        userDomainDataSource.QueryName = "GetUserWithUsername";
                        userDomainDataSource.QueryParameters.Clear();

                        String username = WebContext.Current.Authentication.User.Identity.Name;

                        var par = new Parameter();
                        par.ParameterName = "_userName";
                        par.Value = username;

                        userDomainDataSource.QueryParameters.Clear();
                        userDomainDataSource.QueryParameters.Add(par);

                        userDomainDataSource.Load();

                    };

                    login.Show();
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

      


        // After the Frame navigates, ensure the HyperlinkButton representing the current page is selected
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
           

        }

        // If an error occurs during navigation, show an error window
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.Exception.Message, e.Exception.ToString());
            msg.Show();
        }


        private void TreeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Users.xaml", UriKind.Relative));
        }

        private void UserTypesNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/UserTypes.xaml", UriKind.Relative));
        }

        private void CompaniesNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Company.xaml", UriKind.Relative));
        }

        private void SitesNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Site.xaml", UriKind.Relative));
        }

        private void ProductsNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Product.xaml", UriKind.Relative));
        }

        private void StatusNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Status.xaml", UriKind.Relative));
        }

        private void PriorityNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Priority.xaml", UriKind.Relative));
        }

        private void FrequencyNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Frequency.xaml", UriKind.Relative));
        }

        private void SeverityNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Severity.xaml", UriKind.Relative));
        }

        private void CommunicationChannelNode_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/CommunicationChannel.xaml", UriKind.Relative));
        }

        private void CsrReport_Node_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/CSRReport.xaml", UriKind.Relative));
        }

        private void CSRReview_Node_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/CSRReview.xaml", UriKind.Relative));
        }

        private void KnowledgeBase_Node_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/KnowledgeBase.xaml", UriKind.Relative));
        }

        private void Settings_Node_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/ChangeSettings.xaml", UriKind.Relative));
        }

        private void MainMenu_Node_Language_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/Culture.xaml", UriKind.Relative));
        }

        private void TreeViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            try
            {
                loUsers = _context.Load(_context.GetUserWithUsernameQuery(WebContext.Current.User.Name));
                loUsers.Completed += new EventHandler(lo_Completed);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        void lo_Completed(object sender, EventArgs e)
        {
            try
            {
                loUserAssignment = _context.Load(_context.GetUserLoggingForUserQuery((loUsers.Entities.ElementAt(0) as User).Id));
                loUserAssignment.Completed += new EventHandler(loUserAssignment_Completed);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        void loUserAssignment_Completed(object sender, EventArgs e)
        {
            try
            {
                Web.UserLogging loging = (loUserAssignment.Entities.ElementAt(loUserAssignment.Entities.Count() - 1) as Web.UserLogging);
                loging.LogOutTime = ServerDateTime;
                _context.SubmitChanges(OnSubmittedChanges, null);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void OnSubmittedChanges(SubmitOperation so)
        {
            try
            {
                WebContext.Current.Authentication.Logout(false);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void TreeViewItem_Selected_2(object sender, RoutedEventArgs e)
        {
            
        }


      public void RegisterOnBeforeUnload()
      {
            //Register Silverlight object for availability in Javascript.
            const string scriptableObjectName = "Bridge";
           HtmlPage.RegisterScriptableObject(scriptableObjectName, this);
            //Start listening to Javascript event.
           string pluginName = "silverlightControlHost";//HtmlPage.Plugin.Id;
            HtmlPage.Window.Eval(string.Format(
                @"window.onbeforeunload = function () {{
                    var slApp = document.getElementById('{0}');
                    var result = slApp.Content.{1}.OnBeforeUnload();
                    if(result.length > 0)
                        return result;
                }}",pluginName,scriptableObjectName)
               );
        }
    

        [ScriptableMember]
        public string OnBeforeUnload()
        {
            if (IsDirty)
                return "The page is dirty. Are you sure you want to close?";
            return string.Empty;
        }
    

        public bool IsDirty
        {
            get
            {
                bool tempDirty = this.isDirty;
                this.isDirty = false;
                return tempDirty;
            }
        }

        private void userDomainDataSource_LoadedData(object sender, System.Windows.Controls.LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    if (userDomainDataSource.DataView.Count > 0)
                    {
                        Web.User user = userDomainDataSource.DataView[0] as Web.User;

                        //Set user localization (language) depending on dat in database
                        if (user.Localization != null)
                        {
                            if (user.Localization.CultureCode != "en-EN" && user.Localization.CultureCode != null)
                            {

                                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(user.Localization.CultureCode);
                                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(user.Localization.CultureCode);


                                ((Resources.CustomResources)this.Resources["BS_RES"]).LocalizedStrings = new Resources.BS_Resources();

                            }
                        }


                        this.LayoutRoot.Visibility = System.Windows.Visibility.Visible;


                        //Depending on logged user role show or hide parts of the application
                        if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("Super Admin"))
                        {
                            AdminNode.Visibility = System.Windows.Visibility.Visible;
                            MainMenu_Node_KB.Visibility = System.Windows.Visibility.Visible;
                        }
                        else if (WebContext.Current.User.IsInRole("Customer"))
                        {
                            AdminNode.Visibility = System.Windows.Visibility.Collapsed;
                            MainMenu_Node_KB.Visibility = System.Windows.Visibility.Collapsed;
                        }
                        else if (WebContext.Current.User.IsInRole("Support engineer"))
                        {
                            AdminNode.Visibility = System.Windows.Visibility.Collapsed;
                            MainMenu_Node_KB.Visibility = System.Windows.Visibility.Visible;
                        }

                        //When user is logged on show CSRReview form
                        MainMenu_Node_CsrReview.IsSelected = true;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        private void MainMenu_Node_TypeOfRequesters_Selected(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/TypeOfRequester.xaml", UriKind.Relative));
        }

        private void MainMenu_Node_IssueDomains_Selected_1(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri("/Views/IssueDomain.xaml", UriKind.Relative));
        }
       
    }
}