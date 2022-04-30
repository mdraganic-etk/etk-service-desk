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
using System.Windows.Navigation;
using BusinessSystemsApp.Web;
using System.ServiceModel.DomainServices.Client;
using System.Collections.ObjectModel;
using BusinessSystemsApp.Helpers;

namespace BusinessSystemsApp.Views
{
    public partial class CSRReport : Page
    {
        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        Int32 companyId = 0;

        Web.User _currentUser = null;

        ObservableCollection<Web.Company> selectedCompany = new ObservableCollection<Web.Company>();

        List<System.IO.FileStream> fileStreams = new List<System.IO.FileStream>();

        List<Attachment> attachmentList = new List<Attachment>();

        System.Windows.Threading.DispatcherTimer timer = null;

        public bool Added = false;

        Csr csr;

        public CSRReport()
        {
            InitializeComponent();

            Web.Csr csr = new Csr();
            LayoutRoot.DataContext = csr;
        }

       
        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                if (WebContext.Current.User.IsInRole("Customer"))
                {
                    CsrReport_Label_CommunicationChannel.Visibility = System.Windows.Visibility.Collapsed;
                    communicationChannelComboBox.Visibility = System.Windows.Visibility.Collapsed;

                    CsrReport_Label_Caller.Visibility = System.Windows.Visibility.Collapsed;
                    userComboBox.Visibility = System.Windows.Visibility.Collapsed;
                }


                productsInCompanyComboBox.ItemsSource = _context.ProductsInCompanies;

                companyPrioritiesComboBox.ItemsSource = _context.CompanyPriorities;

                siteComboBox.ItemsSource = _context.Sites;

                userComboBox.ItemsSource = _context.Users;

                requesterTypeInCompanyComboBox.ItemsSource = _context.RequesterTypeInCompanies;

                issueDomainInCompanyComboBox.ItemsSource = _context.IssueDomainInCompanies;

                //selectedCompanyComboBox.ItemsSource = selectedCompany;

                sendTimeTextBox.Text = MainPage.ServerDateTime.ToString();

                userDomainDataSource1.QueryName = "GetUserWithUsername";
                userDomainDataSource1.QueryParameters.Clear();

                String username = WebContext.Current.Authentication.User.Identity.Name;

                var par = new Parameter();
                par.ParameterName = "_userName";
                par.Value = username;

                userDomainDataSource1.QueryParameters.Add(par);

                userDomainDataSource1.Load();

                userDomainDataSource1.LoadedData += new EventHandler<LoadedDataEventArgs>(userDomainDataSource1_LoadedData);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
          
        }


        private void userDomainDataSource1_LoadedData(object sender, LoadedDataEventArgs e)
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
                    if (userDomainDataSource1.DataView.Count > 0)
                    {
                        _currentUser = userDomainDataSource1.DataView[0] as Web.User;

                        UserNameTextBox.Text = _currentUser.FirstName + " " + _currentUser.LastName;
                    }

                    companyDomainDataSource.QueryParameters.Clear();

                    if (WebContext.Current.User.IsInRole("Admin"))
                    {
                        companyDomainDataSource.QueryName = "GetCompanyQuery";
                        //companyDomainDataSource.QueryParameters.Clear();

                        companyComboBox.SelectedValuePath = "Id";
                        companyComboBox.DisplayMemberPath = "CompanyName";
                    }

                    else if (WebContext.Current.User.IsInRole("Support engineer"))
                    {
                        companyDomainDataSource.QueryName = "GetCompaniesAssignmentQuery";
                        
                        Parameter par = new Parameter();
                        par.ParameterName = "_companyId";
                        par.Value = _currentUser.CompanyId;

                        companyDomainDataSource.QueryParameters.Add(par);

                        companyComboBox.SelectedValuePath = "Company1.Id";
                        companyComboBox.DisplayMemberPath = "Company1.CompanyName";

                    }

                    else if (WebContext.Current.User.IsInRole("Customer"))
                    {
                        companyDomainDataSource.QueryName = "GetCompaniesAssignmentQuery";

                        Parameter par1 = new Parameter();
                        par1.ParameterName = "_companyId";
                        par1.Value = _currentUser.CompanyId;

                        companyDomainDataSource.QueryParameters.Add(par1);

                        companyComboBox.SelectedValuePath = "Company1.Id";
                        companyComboBox.DisplayMemberPath = "Company1.CompanyName";
                    }

                    companyDomainDataSource.Load();

                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        private void companyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    CompaniesAssignment compAss = new CompaniesAssignment();

                    //If user is customer or support engineer then load only them company and assigned companies and if user is Admin then loads all companies
                    if ((WebContext.Current.User.IsInRole("Customer") || WebContext.Current.User.IsInRole("Support engineer")) && !Added)
                    {
                        Web.Company comp = new Web.Company();
                        comp.Id = 0;
                        comp.CompanyName = "";

                        Web.Company comp1 = new Web.Company();
                        comp1.Id = _currentUser.CompanyId;
                        comp1.CompanyName = _currentUser.Company.CompanyName;

                        compAss.CompanyId = 0;
                        compAss.Company = comp;
                        compAss.AssignedCommpanyId = _currentUser.CompanyId;
                        compAss.Company1 = comp1;

                        
                        companyDomainDataSource.DataView.Add(compAss);
                        Added = true;
                    }

                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        private void companyPrioritiesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void siteDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void productsInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void userDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        
        private void companiesAssignmentDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
           
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //Update combo boxes based on selected company
        private void companyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((WebContext.Current.User.IsInRole("Customer")) || (WebContext.Current.User.IsInRole("Support engineer")))
                {
                    companyId = (companyComboBox.SelectedItem as BusinessSystemsApp.Web.CompaniesAssignment).Company1.Id;

                }
                else
                {
                    companyId = (companyComboBox.SelectedItem as BusinessSystemsApp.Web.Company).Id;

                    //selectedCompany.Clear();
                    //selectedCompany.Add(companyComboBox.SelectedItem as Web.Company);
                }

                _context.ProductsInCompanies.Clear();
                _context.Load(_context.GetProductsInCompanyQuery(companyId));

                _context.CompanyStatuses.Clear();
                _context.Load(_context.GetCompanyStatusesQuery(companyId));

                _context.CompanyPriorities.Clear();
                _context.Load(_context.GetCompanyPrioritiesQuery(companyId));

                _context.Sites.Clear();
                _context.Load(_context.GetSitesForCompanyQuery(companyId));

                _context.Users.Clear();
                _context.Load(_context.GetUsersForCompanyQuery(companyId));

                _context.RequesterTypeInCompanies.Clear();
                _context.Load(_context.GetRequesterTypeInCompanyQuery(companyId));

                _context.IssueDomainInCompanies.Clear();
                _context.Load(_context.GetIssueDomainInCompanyQuery(companyId));
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// Save csr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        //Save csr button is selected
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(fileUploaderControl1.Files.Count > 0)
                {
                    if (CheckRequiredInput())
                    {
                        fileUploaderControl1.UploadFilesMethod();

                        busyIndicator1.BusyContent = BusinessSystemsApp.Resources.BS_Resources.CSRReport_BusyIndicator_Uploading;
                        busyIndicator1.IsEnabled = true;
                        busyIndicator1.Visibility = System.Windows.Visibility.Visible;
                        busyIndicator1.IsBusy = true;

                        fileUploaderControl1.Files.AllFilesFinished += new EventHandler(Files_AllFilesFinished);

                        fileUploaderControl1.Files.ErrorOccurred += new EventHandler<Vci.Silverlight.FileUploader.UploadErrorOccurredEventArgs>(Files_ErrorOccurred);
                    }
                }
                else
                {
                    if(CheckRequiredInput())
                        CreateAndSaveCSR();
                }

            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();

                HideBusyIndicator();
            }
      
        }

        //If file upload did not succedeed
        void Files_ErrorOccurred(object sender, Vci.Silverlight.FileUploader.UploadErrorOccurredEventArgs e)
        {
            try
            {
                HideBusyIndicator();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


        private bool CheckRequiredInput()
        {
            bool isFormValid = true;

            companyComboBox.ClearValidationError();
            //selectedCompanyComboBox.ClearValidationError();
            sendTimeTextBox.ClearValidationError();
            UserNameTextBox.ClearValidationError();
            CsrHeadingTextBox.ClearValidationError();
            CsrDescriptionTextBox.ClearValidationError();
            companyPrioritiesComboBox.ClearValidationError();
            siteComboBox.ClearValidationError();
            productsInCompanyComboBox.ClearValidationError();
            userComboBox.ClearValidationError();
            communicationChannelComboBox.ClearValidationError();

            string textWarning = BusinessSystemsApp.Resources.BS_Resources.CsrReport_RequiredInput;

            if (companyComboBox.SelectedItem == null)
            {
                companyComboBox.SetValidation(textWarning);
                companyComboBox.RaiseValidationError();
                isFormValid = false;

            }

            //if (selectedCompanyComboBox.SelectedItem == null)
            //{
            //    selectedCompanyComboBox.SetValidation(textWarning);
            //    selectedCompanyComboBox.RaiseValidationError();
            //    isFormValid = false;
            //}

            if (sendTimeTextBox.Text == String.Empty)
            {
                sendTimeTextBox.SetValidation(textWarning);
                sendTimeTextBox.RaiseValidationError();
                isFormValid = false;
            }

            if (UserNameTextBox.Text == String.Empty)
            {
                UserNameTextBox.SetValidation(textWarning);
                UserNameTextBox.RaiseValidationError();
                isFormValid = false;
            }

            if(CsrHeadingTextBox.Text == String.Empty)
            {
                CsrHeadingTextBox.SetValidation(textWarning);
                CsrHeadingTextBox.RaiseValidationError();
                isFormValid = false;
            }

            if (CsrDescriptionTextBox.Text == String.Empty)
            {
                CsrDescriptionTextBox.SetValidation(textWarning);
                CsrDescriptionTextBox.RaiseValidationError();
                isFormValid = false;
            }

            if (companyPrioritiesComboBox.SelectedItem == null)
            {
                companyPrioritiesComboBox.SetValidation(textWarning);
                companyPrioritiesComboBox.RaiseValidationError();
                isFormValid = false;
            }

            if (siteComboBox.SelectedItem == null)
            {
                siteComboBox.SetValidation(textWarning);
                siteComboBox.RaiseValidationError();
                isFormValid = false;
            }

            if (productsInCompanyComboBox.SelectedItem == null)
            {
                productsInCompanyComboBox.SetValidation(textWarning);
                productsInCompanyComboBox.RaiseValidationError();
                isFormValid = false;
            }

            if (userComboBox.SelectedItem == null && !WebContext.Current.User.IsInRole("Customer"))
            {
                userComboBox.SetValidation(textWarning);
                userComboBox.RaiseValidationError();
                isFormValid = false;
            }

            //if (communicationChannelComboBox.SelectedItem == null || (communicationChannelComboBox.SelectedItem as Web.CommunicationChannel).Id == 0)
            //{
            //    communicationChannelComboBox.SetValidation(textWarning);
            //    communicationChannelComboBox.RaiseValidationError();
            //    isFormValid = false;
            //}

            return isFormValid;
        }


        void Files_AllFilesFinished(object sender, EventArgs e)
        {
            try
            {

                fileUploaderControl1.Files.AllFilesFinished -= new EventHandler(Files_AllFilesFinished);

               
                CreateAndSaveCSR();

            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void CreateAndSaveCSR()
        {
            try
            {
                busyIndicator1.BusyContent = BusinessSystemsApp.Resources.BS_Resources.CSRReport_BusyIndicator_Saving;
                busyIndicator1.Visibility  = System.Windows.Visibility.Visible;
                busyIndicator1.IsEnabled = true;
                busyIndicator1.IsBusy = true;
                
                
                csr = new Csr();

                if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("Super Admin"))
                    csr.CompanyId = (companyComboBox.SelectedItem as Web.Company).Id;
                else
                    csr.CompanyId = (companyComboBox.SelectedItem as Web.CompaniesAssignment).Company1.Id;

                csr.UserId = _currentUser.Id;

                int statusCSR = 0;

                //Initial status marks that CSR is registered. Set status depending on culture. Temporary SOLUTION.
               foreach(CompanyStatuses status in _context.CompanyStatuses)
               {
                   if(status.Csr_Status.StatusName.StartsWith("RE"))
                       statusCSR = status.StatusId;
               }

               if (statusCSR != 0)
                   csr.StatusId = statusCSR;
               else
                   throw new Exception("This company does not contain proper statuses. Check with the administrator statuses for this company!");

                ///////////////////////////////////////

                csr.SiteId = (siteComboBox.SelectedItem as Web.Site).Id;

                //csr.RegisterDate = DateTime.Now;

                csr.ProductId = (productsInCompanyComboBox.SelectedItem as Web.ProductsInCompany).ProductId;

                csr.PriorityId = (companyPrioritiesComboBox.SelectedItem as Web.CompanyPriorities).PriorityId;

                if (requesterTypeInCompanyComboBox.SelectedItem != null)
                {
                    csr.RequesterTypeId = (requesterTypeInCompanyComboBox.SelectedItem as Web.RequesterTypeInCompany).RequesterTypeId;
                }

                if (issueDomainInCompanyComboBox.SelectedItem != null)
                {
                    csr.IssueDomainId = (issueDomainInCompanyComboBox.SelectedItem as Web.IssueDomainInCompany).IssueDomainId;
                }

                //csr.CallDate = DateTime.Now;

                csr.LastUserId = _currentUser.Id;

                csr.Heading = CsrHeadingTextBox.Text;

                csr.Description = CsrDescriptionTextBox.Text;

                if((Int32)communicationChannelComboBox.SelectedValue != 0 && communicationChannelComboBox.SelectedValue != null)
                    csr.CommunicationId = (communicationChannelComboBox.SelectedItem as Web.CommunicationChannel).Id;

                if (userComboBox.SelectedItem != null)
                    csr.CallerId = (userComboBox.SelectedItem as User).Id;
                else
                    csr.CallerId = _currentUser.Id;
                    


                _context.Csrs.Add(csr);

                _context.SubmitChanges(OnSubmitCompleted, null);


               

            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();

                Dispatcher.BeginInvoke(HideBusyIndicator);
            }
        }
      
        private void OnSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", "Submitt error", so.Error.ToString());
                msg.Show();
                so.MarkErrorAsHandled();

                Dispatcher.BeginInvoke(HideBusyIndicator);
            }
            else
            {
                try
                {
                    if (fileUploaderControl1.Files.Count > 0)
                    {
                        foreach (Vci.Silverlight.FileUploader.UserFile file in fileUploaderControl1.Files)
                        {

                            Attachment attach = new Attachment();
                            attach.AttachmentName = file.FileName;
                            attach.Url = file.Guid;

                            attachmentList.Add(attach);

                        }

                        foreach (Attachment attach in attachmentList)
                        {
                            _context.Attachments.Add(attach);
                        }
                        _context.SubmitChanges(OnAttachmentSubmitCompleted, null);
                    }
                    else
                    {
                        Dispatcher.BeginInvoke(HideBusyIndicator);

                        CsrSaved_Id_Label.Content = "( " + csr.Id.ToString() + " )";

                        grid2.Visibility = System.Windows.Visibility.Visible;

                        timer = new System.Windows.Threading.DispatcherTimer();
                        timer.Interval = new TimeSpan(0,0,3);
                        timer.Tick +=new EventHandler(timer_Tick);
                        timer.Start();
                        
                       

                        //Send emails to users to notify

                        userNotificationsDomainDataSource.QueryParameters[0].Value = companyComboBox.SelectedValue;
                        userNotificationsDomainDataSource.Load();

                        ///////////////
                    }

                    //CSR_list solidusNotify = new CSR_list();
                   
                    //solidusNotify.Source = "PS-2";
                    //solidusNotify.CSR = csr.Id.ToString();
                    //solidusNotify.Priority = csr.Priority.PriorityName;
                    //if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("Super Admin"))
                    //    solidusNotify.Company = (companyComboBox.SelectedItem as Web.Company).CompanyName;
                    //else
                    //    solidusNotify.Company = (companyComboBox.SelectedItem as Web.CompaniesAssignment).Company1.CompanyName;
                    //solidusNotify.Product = csr.Product.ProductName;
                    //solidusNotify.RegistrationTime = MainPage.ServerDateTime;
                    //solidusNotify.RegistrationPerson = _currentUser.FirstName + " " + _currentUser.LastName;
                    //solidusNotify.Subject = csr.Heading;
                    //solidusNotify.Description = csr.Description;

                    //cSR_listDomainDataSource.DataView.Add(solidusNotify);
                    
                    //cSR_listDomainDataSource.SubmitChanges();

                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();

                    Dispatcher.BeginInvoke(HideBusyIndicator);
                }
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            grid2.Visibility = System.Windows.Visibility.Collapsed;

            timer.Stop();
           
        }


        private void OnAttachmentSubmitCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", "Submitt error", so.Error.ToString());
                msg.Show();
                so.MarkErrorAsHandled();

                Dispatcher.BeginInvoke(HideBusyIndicator);
            }
            else
            {
                try
                {
                    foreach (Attachment attach in attachmentList)
                    {
                        AttachmenttAssign attachAssign = new AttachmenttAssign();
                        attachAssign.AttachmentId = attach.Id;
                        attachAssign.CsrId = csr.Id;

                        _context.AttachmenttAssigns.Add(attachAssign);
                    }

                    _context.SubmitChanges(OnAttachmentAssignSubmittCompleted, null);

                    attachmentList.Clear();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();

                    Dispatcher.BeginInvoke(HideBusyIndicator);
                }
            }
        }

        private void OnAttachmentAssignSubmittCompleted(SubmitOperation so)
        {
            if (so.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", "Submitt error", so.Error.ToString());
                msg.Show();
                so.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    //Send emails to users to notify

                    userNotificationsDomainDataSource.QueryParameters[0].Value = companyComboBox.SelectedValue;
                    userNotificationsDomainDataSource.Load();

                    ///////////////

                    Dispatcher.BeginInvoke(HideBusyIndicator);

                    CsrSaved_Id_Label.Content = "( " + csr.Id.ToString() + " )";

                    grid2.Visibility = System.Windows.Visibility.Visible;

                    timer = new System.Windows.Threading.DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 3);
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Start();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();

                    Dispatcher.BeginInvoke(HideBusyIndicator);
                }
            }
        }


        private void HideBusyIndicator()
        {
            busyIndicator1.IsEnabled = false;
            busyIndicator1.Visibility = System.Windows.Visibility.Collapsed;
            busyIndicator1.IsBusy = false;
        }

        private void CsrReport_Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (CsrReport_Expander != null)
            {

                CsrReport_Expander.Height = Double.NaN;
            }
        }

        private void CsrReport_Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            if (CsrReport_Expander != null)
            {
                CsrReport_Expander.Height = 22;
            }
        }


        /// <summary>
        /// When users data are loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userNotificationsDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    List<Int32> contactList = new List<Int32>();

                    //Take contacts Id from data source and fill list with contact Ids
                    foreach(Web.UserNotifications notification in userNotificationsDomainDataSource.DataView)
                    {
                        if(notification.User.ContactId != null)
                        {
                            contactList.Add(notification.User.ContactId);
                        }
                    }

                    //Add current user e-mail to list
                    contactList.Add(_currentUser.ContactId);
                    
                    //Add caller to mail list if exists
                    if(userComboBox.SelectedItem != null)
                        contactList.Add((userComboBox.SelectedItem as User).ContactId);

                    //If list of contacts is not empty put contact ids in parameter of data source and load data source
                    if(contactList.Count > 0)
                    {
                        contactDomainDataSource1.QueryParameters[0].Value = contactList;
                        contactDomainDataSource1.Load();

                        contactList.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

       
        /// <summary>
        /// When contacts are loaded fill mail parameters and send e-mail to users
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contactDomainDataSource1_LoadedData(object sender, LoadedDataEventArgs e)
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

                    MailServiceReference.MailServiceClient client = new MailServiceReference.MailServiceClient("BasicHttpBinding_MailService");

                    ObservableCollection<String> addressList = new ObservableCollection<String>();

                    Helpers.HelperClasses.Util util = new Helpers.HelperClasses.Util();

                    //For every entity in data source take e-mail address and put it to list
                    foreach(Contact con in contactDomainDataSource1.DataView)
                    {
                        if(con.Email != null && con.Email != String.Empty)
                        {
                            if(util.ValidEmail(con.Email))
                                addressList.Add(con.Email);
                        }
                    }

                    String Company = null;

                    //Get company name
                    if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("Super Admin"))
                    {
                        Company = (companyComboBox.SelectedItem as Web.Company).CompanyName;
                    }
                    else
                    {
                        Company = (companyComboBox.SelectedItem as Web.CompaniesAssignment).Company1.CompanyName;
                    }

                    String userPhones = null;

                    if (_currentUser.Contact.Fix1 != null)
                        userPhones += _currentUser.Contact.Fix1 + "    ";
                    if (_currentUser.Contact.Fix2 != null)
                        userPhones += _currentUser.Contact.Fix2 + "    ";
                    if (_currentUser.Contact.Mobile1 != null)
                        userPhones += _currentUser.Contact.Mobile1 + "    ";
                    if (_currentUser.Contact.Mobile2 != null)
                        userPhones += _currentUser.Contact.Mobile2 + "    ";


                    String caller = null;

                    if (userComboBox.SelectedItem != null)
                        caller = (userComboBox.SelectedItem as User).FirstName + " " + (userComboBox.SelectedItem as User).LastName;

                    //Send e-mail
                    Helpers.ComposeAndSendEmail mail = new ComposeAndSendEmail(addressList,
                                                                                csr.Id.ToString(),
                                                                                CsrHeadingTextBox.Text,
                                                                                CsrDescriptionTextBox.Text,
                                                                                (companyPrioritiesComboBox.SelectedItem as CompanyPriorities).Priority.PriorityName,
                                                                                Company,
                                                                                DateTime.Now.ToString(),
                                                                                UserNameTextBox.Text,
                                                                                caller,
                                                                                (productsInCompanyComboBox.SelectedItem as ProductsInCompany).Product.ProductName,
                                                                                "There are : " + fileUploaderControl1.Files.Count.ToString() + "  attachments for this CSR.", 
                                                                                userPhones,
                                                                                _currentUser.Contact.Email,
                                                                                Helpers.ComposeAndSendEmail.MailType.NewCSR,
                                                                                null);
                    mail.Send();
                    /////////////                                                            

                    addressList.Clear();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }



        private void communicationChannelDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    Web.CommunicationChannel com = new Web.CommunicationChannel();
                    com.CommunicationChannelName = " - None -";
                    com.Id = 0;

                    communicationChannelDomainDataSource.DataView.Add(com);

                    communicationChannelComboBox.SelectedValue = 0;
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }


        private void csrDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        private void companyPrioritiesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (companyPrioritiesComboBox.SelectedItem != null)
                {
                    if ((companyPrioritiesComboBox.SelectedItem as CompanyPriorities).PriorityId == 8)
                        CSRReport_Label_Notify.Visibility = System.Windows.Visibility.Visible;
                    else
                        CSRReport_Label_Notify.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                    CSRReport_Label_Notify.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


        private void CSRReport_Button_Clear_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                sendTimeTextBox.Text = DateTime.Now.ToString("dd.MM.yyyy  hh:mm");
                CsrHeadingTextBox.Text = String.Empty;
                CsrDescriptionTextBox.Text = String.Empty;

                if (companyPrioritiesComboBox.ItemsSource != null)
                    companyPrioritiesComboBox.SelectedIndex = -1;

                if (siteComboBox.ItemsSource != null)
                    siteComboBox.SelectedIndex = -1;

                if (productsInCompanyComboBox.ItemsSource != null)
                    productsInCompanyComboBox.SelectedIndex = -1;

                if (userComboBox.ItemsSource != null)
                    userComboBox.SelectedIndex = -1;

                if (requesterTypeInCompanyComboBox.ItemsSource != null)
                    requesterTypeInCompanyComboBox.SelectedIndex = -1;

                if (issueDomainInCompanyComboBox.ItemsSource != null)
                    issueDomainInCompanyComboBox.SelectedIndex = -1;

                communicationChannelComboBox.SelectedValue = 0;


                companyComboBox.ClearValidationError();
                //selectedCompanyComboBox.ClearValidationError();
                sendTimeTextBox.ClearValidationError();
                UserNameTextBox.ClearValidationError();
                CsrHeadingTextBox.ClearValidationError();
                CsrDescriptionTextBox.ClearValidationError();
                companyPrioritiesComboBox.ClearValidationError();
                siteComboBox.ClearValidationError();
                productsInCompanyComboBox.ClearValidationError();
                userComboBox.ClearValidationError();
                //communicationChannelComboBox.ClearValidationError();

                HideBusyIndicator();

                fileUploaderControl1.Files.Clear();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void companyStatusesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void cSR_listDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void cSR_listDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
               
                e.MarkErrorAsHandled();
            }
        }

        private void issueDomainInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void requesterTypeInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
    }
}
