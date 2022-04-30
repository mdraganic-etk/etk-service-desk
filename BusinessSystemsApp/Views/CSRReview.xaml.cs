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
using System.ServiceModel.DomainServices.Client;
using BusinessSystemsApp.Web;
using System.Windows.Data;
using System.Windows.Browser;
using System.Collections.ObjectModel;



namespace BusinessSystemsApp.Views
{
    public partial class CSRReview : Page
    {
        # region Initialize
        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        BusinessSystemsDomainContext _helperContext = new BusinessSystemsDomainContext();

        //Keeps currently selected note
        public Notes currentNote = null;

        //Keeps currently selected KB note
        public KB_Notes currentKBNote = null;

        //Keeps list of attachments for CSR
        List<Attachment> attachmentList = new List<Attachment>();

        public bool FirstLoad = false;

        //Keeps list of statuses for filering CSR-s
        public List<Int32> statuses = new List<Int32>();

        //Keeps data about currently logged user
        User _currentUser = null;

        bool _newNote = false;

        private bool riskAnalysisInserted = false;

        Web.Csr csr;

        String csrChanges = null;

        List<Int32> companyList = null;

        public CSRReview()
        {
            InitializeComponent();

            //Set dates on filter module. Default start date is 1.1.2003 and default end dates are current date 
            Filter_ReportedDate_From_datePicker.SelectedDate = new DateTime(2003, 1,1);
            Filter_ReportedDate_To_datePicker.SelectedDate = DateTime.Now;
            Filter_FinishedDate_From_datePicker.SelectedDate = new DateTime(2003, 1, 1);
            Filter_FinishedDate_To_datePicker.SelectedDate = DateTime.Now;

            //Show progress bar
            progressBar1.Visibility = System.Windows.Visibility.Visible;
            progressBar1.IsEnabled = true;

        }


        //Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Set labels for grid column headers
            csrDataGrid.Columns[11].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_CSRGrid_ReportedDate;
            csrDataGrid.Columns[10].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_CSRGrid_Id;
            csrDataGrid.Columns[12].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_CSRGrid_Status;
            csrDataGrid.Columns[13].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_CSRGrid_CompanyName;
            csrDataGrid.Columns[14].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_CSRGrid_Heading;

            notesDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_NotesGrid_Date;
            notesDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_NotesGrid_Id;
            notesDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_NotesGrid_Heading;
            notesDataGrid.Columns[3].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_NotesGrid_AuthorFirstName;
            notesDataGrid.Columns[4].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_NotesGrid_AuthorLastName;

            kB_NotesDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_KBGrid_Id;
            kB_NotesDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_KBGrid_Date;
            kB_NotesDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_KBGrid_Heading;
            kB_NotesDataGrid.Columns[3].Header = BusinessSystemsApp.Resources.BS_Resources.CSRReview_KBGrid_Product;
            
            /////


            //Check user role for edit visibilities on this page
            if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("Support engineer") || WebContext.Current.User.IsInRole("Super Admin"))
            {
                CSRReview_Button_Edit.Visibility = System.Windows.Visibility.Visible;
                CSRReview_TabItem_KnowledgeBase.Visibility = System.Windows.Visibility.Visible;
                CSRReview_TabItem_RiskAnalysis.Visibility = System.Windows.Visibility.Visible;
                CSRReview_Report_LabelButton.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                CSRReview_Button_Edit.Visibility = System.Windows.Visibility.Collapsed;
                CSRReview_TabItem_KnowledgeBase.Visibility = System.Windows.Visibility.Collapsed;
                CSRReview_TabItem_RiskAnalysis.Visibility = System.Windows.Visibility.Collapsed;
                
                //If user type is customer then make "Only my csr-s"  selected and not visible
                //CSRReview_CheckBox_MyCSRs.IsChecked = true;
                //CSRReview_CheckBox_MyCSRs.Visibility = System.Windows.Visibility.Collapsed;
            }
            ////

            if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("Super Admin"))
            {
                reportLink.Visibility = System.Windows.Visibility.Visible;
            }

            //Get user data with username
            userDomainDataSource.QueryName = "GetUserWithUsername";
          
            String username = WebContext.Current.Authentication.User.Identity.Name;

            userDomainDataSource.QueryParameters[0].Value = username;

            userDomainDataSource.Load();
            
        }


        #endregion

       
        //When selection of CSR is changed
        private void csrDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                csr = null;

                //if (FirstLoad)
                //{
                //    csrDataGrid.SelectedItem = -1;

                //    FirstLoad = false;
                //}
                //else
                //{
                    if (csrDataGrid.SelectedItem != null)
                    {
                        //Clear controls content
                        ClearCSRFrom();

                        csr = (csrDataGrid.SelectedItem as Web.Csr);

                        //Load posible priorities and statuses for company of CSR
                        companyPrioritiesDomainDataSource.QueryParameters[0].Value = csr.CompanyId;
                        companyPrioritiesDomainDataSource.Load();

                        companyStatusesDomainDataSource.QueryParameters[0].Value = csr.CompanyId;
                        companyStatusesDomainDataSource.Load();

                        requesterTypeInCompanyDomainDataSource.QueryParameters[0].Value = csr.CompanyId;
                        requesterTypeInCompanyDomainDataSource.Load();

                        issueDomainInCompanyDomainDataSource.QueryParameters[0].Value = csr.CompanyId;
                        issueDomainInCompanyDomainDataSource.Load();


                        ////TEST
                        //Helpers.MessageBox msg = new Helpers.MessageBox("Error", "TEST", (csrDataGrid.SelectedItem as Web.Csr).CompanyId.ToString() + " - " + (csrDataGrid.SelectedItem as Web.Csr).CompanyId.ToString());
                        //msg.Show();
                        /////////


                        //Fill controls with CSR data
                        CsrIdTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Id.ToString();

                        CsrTitleTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Heading.ToString();

                        CsrCompanyTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Company.CompanyName.ToString();
                        companyComboBox1.SelectedValue = (csrDataGrid.SelectedItem as Web.Csr).CompanyId;

                        CsrCallTimeTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).CallDate.ToString();

                        CsrRegisteredTimeTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).RegisterDate.ToString();

                        CsrDescriptionTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Description.ToString();

                        if ((csrDataGrid.SelectedItem as Web.Csr).User2 != null)
                        {
                            CsrAuthorTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).User2.FirstName.ToString() + " " + (csrDataGrid.SelectedItem as Web.Csr).User2.LastName.ToString();
                        }

                        CsrFinishedTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).FinishDate.ToString();

                        if ((csrDataGrid.SelectedItem as Web.Csr).Answer != null)
                            CsrAnswerTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Answer.ToString();

                        CsrProductTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Product.ProductName;

                        RiskAnalysisCsrIdTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Id.ToString();

                        RiskAnalysisCsrHeadingTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Heading;

                        if ((csrDataGrid.SelectedItem as Web.Csr).User1 != null)
                        {
                            CsrUserTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).User1.FirstName + " " + (csrDataGrid.SelectedItem as Web.Csr).User1.LastName;
                        }

                        if ((csrDataGrid.SelectedItem as Web.Csr).FrequencyId != null)
                            frequencyComboBox.SelectedValue = (csrDataGrid.SelectedItem as Web.Csr).FrequencyId;

                        if ((csrDataGrid.SelectedItem as Web.Csr).SeverityId != null)
                            severityComboBox.SelectedValue = (csrDataGrid.SelectedItem as Web.Csr).SeverityId;
                        //dodano 05.12.2017 zbog zatjeva da se vidi lokacija kod pregleda CSR-a
                        
                        if ((csrDataGrid.SelectedItem as Web.Csr).Site != null)
                        {
                            CsrLocationTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Site.SiteName;
                        }

                       

                        //remedy
                        if ((csrDataGrid.SelectedItem as Web.Csr).Remedy != null)
                        {
                            RemedyTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).Remedy;
                        }
                        
                        if ((csrDataGrid.SelectedItem as Web.Csr).RemedyTime != null)
                        {
                            RemedyTimeTextBox.Text = (csrDataGrid.SelectedItem as Web.Csr).RemedyTime.ToString();
                        }

                        //Get notes for selected CSR
                        notesDomainDataSource.QueryParameters[0].Value = csr.Id;
                        notesDomainDataSource.Load();


                        //Get KB notes for selected CSR
                        kB_NotesDomainDataSource.QueryParameters[0].Value = csr.Id;
                        kB_NotesDomainDataSource.Load();


                        //Get atachments for selected CSR
                        attachmenttAssignDomainDataSource.QueryParameters[0].Value = (csrDataGrid.SelectedItem as Web.Csr).Id;
                        attachmenttAssignDomainDataSource.Load();

                        //Get contact for caller to show contact details for caller on popup frame
                        if ((csrDataGrid.SelectedItem as Web.Csr).User2 != null)
                            contactDomainDataSource.QueryParameters[0].Value = (csrDataGrid.SelectedItem as Web.Csr).User2.ContactId;

                        contactDomainDataSource.Load();


                        //Enable edit buttons when CSR is selected
                        CSRReview_Button_Edit.IsEnabled = true;
                        CSRReview_Notes_Button_Add.IsEnabled = true;
                        CSRReview_KB_Button_Add.IsEnabled = true;
                        CSRReview_RiskAnalysis_Button_EnableEditing.IsEnabled = true;

                    }
                    else
                    {
                        //If selected CSR is null then disable edit buttons for CSR
                        CSRReview_Button_Edit.IsEnabled = false;
                        CSRReview_Notes_Button_Add.IsEnabled =false;
                        CSRReview_KB_Button_Add.IsEnabled = false;
                        CSRReview_RiskAnalysis_Button_EnableEditing.IsEnabled = false;
                    }
               // }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


       /// <summary>
       /// Clears labele on CSRReview form
       /// </summary>
        private void ClearCSRFrom()
        {
            try
            {
                CsrIdTextBox.Text = String.Empty;

                CsrTitleTextBox.Text = String.Empty;

                CsrCompanyTextBox.Text = String.Empty;

                CsrCallTimeTextBox.Text = String.Empty;

                CsrRegisteredTimeTextBox.Text = String.Empty;

                CsrDescriptionTextBox.Text = String.Empty;

                CsrAuthorTextBox.Text = String.Empty;

                CsrFinishedTextBox.Text = String.Empty;

                CsrUserTextBox.Text = String.Empty;

                CsrAnswerTextBox.Text = String.Empty;

                CsrProductTextBox.Text = String.Empty;

                RiskAnalysisCsrIdTextBox.Text = String.Empty;

                RiskAnalysisCsrHeadingTextBox.Text = String.Empty;

                companyStatusesComboBox.SelectedValue = -1;

                companyPrioritiesComboBox.SelectedValue = -1;

                requesterTypeInCompanyComboBox.SelectedValue = -1;

                issueDomainInCompanyComboBox.SelectedValue = -1;

                severityComboBox.SelectedValue = -1;

                frequencyComboBox.SelectedValue = -1;

                AttachmentsListBox.Items.Clear();

                CSRReview_Button_Edit.IsEnabled = false;
                
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        #region Notes

        //When notes are loaded
        private void notesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                notesDataGrid.SelectedIndex = -1;
            }
        }

        //When notes is selected
        private void notesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CSRReview_Notes_Button_Edit.IsEnabled = false;

                //Clear Note controls
                NotesHeadingTextBox.Text = String.Empty;

                NotesAuthorTextBox.Text = String.Empty;

                NoteDateTextBox.Text = String.Empty;

                NoteNoteTextBox.Text = String.Empty;

                Note_Attachments_ListBox.Items.Clear();

                //Fill Note controls with data of selected Note
                if (notesDataGrid.SelectedItem != null)
                {
                    NotesHeadingTextBox.Text = (notesDataGrid.SelectedItem as Web.Notes).Heading;

                    NotesAuthorTextBox.Text = (notesDataGrid.SelectedItem as Web.Notes).User.FirstName + "  " + (notesDataGrid.SelectedItem as Web.Notes).User.LastName;

                    NoteDateTextBox.Text = (notesDataGrid.SelectedItem as Web.Notes).Date.ToString();

                    NoteNoteTextBox.Text = (notesDataGrid.SelectedItem as Web.Notes).Note;

                    //Load selected note attachments
                    notesAttachmentAssignDomainDataSource.QueryParameters[0].Value = (notesDataGrid.SelectedItem as Web.Notes).Id;

                    notesAttachmentAssignDomainDataSource.Load();

                    //If Note is selected in grid then enable edit button

                    if (((notesDataGrid.SelectedItem as Web.Notes).AutorId == _currentUser.Id) && ((notesDataGrid.SelectedItem as Web.Notes).Description != "A"))
                        CSRReview_Notes_Button_Edit.IsEnabled = true;

                }
                else
                {
                    //If there is no selected note then disable edit button
                    CSRReview_Notes_Button_Edit.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //Opens child window for editing note
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.NoteDetails nd = new ChildForms.NoteDetails((notesDataGrid.SelectedItem as Web.Notes));

                nd.Show();

                nd.Closed += new EventHandler(nd_Closed);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //Open new window for add new note to CSR
        private void btnAddNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.NoteDetails nd = new ChildForms.NoteDetails((csrDataGrid.SelectedItem as Web.Csr).Id);

                nd.Show();

                nd.Closed += new EventHandler(nd_Closed);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //When child window for editing or adding Note is closed
        void nd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.NoteDetails nd = (ChildForms.NoteDetails)sender;

                if (nd.NewNote != null)
                {
                    //If is new note entered
                    if (nd.isNew)
                    {
                        _newNote = true;

                        _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                        _context.Notes.Add(nd.NewNote);
                       
                        notesDomainDataSource.SubmitChanges();

                        currentNote = nd.NewNote;
                        attachmentList = nd.attachmentList;

                        notesDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(notesDomainDataSource_SubmittedChanges);

                    }
                    //If editing existing site
                    else
                    {
                        _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Notes type = _context.Notes.Where(t => t.Id == nd.NewNote.Id).First();

                        type.Heading = nd.NewNote.Heading;
                        type.Note = nd.NewNote.Note;

                        notesDomainDataSource.SubmitChanges();

                        currentNote = nd.NewNote;
                        attachmentList = nd.attachmentList;

                        notesDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(notesDomainDataSource_SubmittedChanges);

                        //////////////////////
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

       

        /// <summary>
        /// When attachments for notes are submitted then we have to connect Note and attachments in databse table
        /// </summary>
        /// <param name="so"></param>
        private void OnAttachmentSubmitCompleted(SubmitOperation so)
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
                    //Foreach attachment which is inserted for Note we add attachment assign to Note in database
                    foreach (Attachment attach in attachmentList)
                    {
                        AttachmenttAssign attachAssign = new AttachmenttAssign();
                        attachAssign.AttachmentId = attach.Id;
                        attachAssign.NoteId = currentNote.Id;

                        _context.AttachmenttAssigns.Add(attachAssign);
                    }

                    _context.SubmitChanges(OnNoteAttachmentAssignSubmitCompleted, null);

                    attachmentList.Clear();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();

                    
                }
            }
        }


        private void OnNoteAttachmentAssignSubmitCompleted(SubmitOperation so)
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
                    notesDomainDataSource.Load();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();


                }
            }
        }

        /// <summary>
        /// When Notes are submitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void  notesDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();

                notesDomainDataSource.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(notesDomainDataSource_SubmittedChanges);
            }
            else
            {
                try
                {
                    //If note is added for csr Changes the send e-mail for csr changes 
                    if (csrChanges != null)
                    {
                        //Load users to notify and when loaded send them e-mail of notifications
                        userNotificationsDomainDataSource.QueryParameters[0].Value = csr.CompanyId;
                        userNotificationsDomainDataSource.Load();

                        CSRReview_Button_Save.IsEnabled = true;

                        notesDomainDataSource.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(notesDomainDataSource_SubmittedChanges);
                    }
                    else
                    {
                        if (_newNote)
                        {
                            notesDomainDataSource.Load();

                            //Load users to notify and when loaded send them e-mail of notifications
                            userNotificationsDomainDataSource.QueryParameters[0].Value = csr.CompanyId;
                            userNotificationsDomainDataSource.Load();

                            _newNote = false;
                        }

                        //Add atachments into database

                        if (attachmentList.Count > 0)
                        {
                            foreach (Attachment attach in attachmentList)
                            {
                                _helperContext.Attachments.Add(attach);
                            }

                            _helperContext.SubmitChanges(OnAttachmentSubmitCompleted, null);
                        }

                        /////////////////////////////////////////////////////////////////////////////////////

                        notesDomainDataSource.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(notesDomainDataSource_SubmittedChanges);

                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        //Removes selected note
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var k = notesDataGrid.SelectedItem;

                _context.Notes.Remove(k as Web.Notes);

                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //Show selected note attachments in list
        private void notesAttachmentAssignDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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

                    foreach (AttachmenttAssign attach in notesAttachmentAssignDomainDataSource.DataView)
                    {
                        ListBoxItem lbi = new ListBoxItem();

                        //For new added attachments
                        if (attach.Attachment.Url != String.Empty && attach.Attachment.Url != null)
                        {
                            lbi.Name = attach.Attachment.Url;
                            lbi.Content = attach.Attachment.AttachmentName;
                        }
                        //For migrated attachments
                        else
                        {
                            lbi.Name = attach.Attachment.AttachmentName;
                            lbi.Content = attach.Attachment.AttachmentName;
                        }

                        Note_Attachments_ListBox.Items.Add(lbi);
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        //When Note attachment is selected try to find it and open
        private void Note_Attachments_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Note_Attachments_ListBox.SelectedItem != null)
                {
                    var hostingWindow = System.Windows.Browser.HtmlPage.Window;

                    String path = "/Data/";

                    if (MainPage.AttachmentPath != null)
                        path = MainPage.AttachmentPath;

                    hostingWindow.Navigate(new Uri(path + (Note_Attachments_ListBox.SelectedItem as ListBoxItem).Name, UriKind.RelativeOrAbsolute), "_blank");


                    if (Note_Attachments_ListBox.Items.Count == 1)
                        Note_Attachments_ListBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        #endregion 


        //When selcted button for editing CSR
        void btnEditCsr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                companyStatusesComboBox.IsEnabled = true;
                companyPrioritiesComboBox.IsEnabled = true;

                requesterTypeInCompanyComboBox.IsEnabled = true;
                issueDomainInCompanyComboBox.IsEnabled = true;

                companyComboBox1.IsEnabled = true;
                productsInCompanyComboBox.IsEnabled = true;
                siteComboBox.IsEnabled = true;

                //CsrAnswerTextBox.Text = "";

                CSRReview_Button_Save.Visibility = System.Windows.Visibility.Visible;
                CSRReview_Button_Cancel.Visibility = System.Windows.Visibility.Visible;

                CsrDescriptionTextBox.IsReadOnly = false;
                CsrAnswerTextBox.IsReadOnly = false;
                RemedyTextBox.IsReadOnly = false;
                RemedyTextBox.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
            
        }

        //When statuses are loaded then file list of check box in filter module
        private void csr_StatusDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    //If user is Admin add all statuses to his status list for filtering
                    if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("SupportEngineer") || WebContext.Current.User.IsInRole("Super Admin"))
                    {
                        //Foreach loaded status add check box in list for filtering CSR-s
                        foreach (Web.Csr_Status status in csr_StatusDomainDataSource.DataView)
                        {
                            CheckBox cb = new CheckBox();
                            cb.Name = status.Id.ToString();
                            cb.Content = status.StatusName;

                            Filter_CsrStatuses_StackPanel.Children.Add(cb);
                        }
                    }
                        
                }
                 
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show(); 
                }
            }
        }


        //When severities are loaded select severity of selected CSR
        private void severityDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    severityComboBox.SelectedIndex = -1;

                    if (csr != null)
                    {
                        if (csr.SeverityId != null)
                            severityComboBox.SelectedValue = csr.SeverityId;
                    }
                }
                 
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show(); 
                }
            }
        }


        //When frequencies are loaded select frequency of selected CSR
        private void frequencyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    frequencyComboBox.SelectedIndex = -1;

                    if (csr != null)
                    {
                        if (csr.FrequencyId != null)
                            frequencyComboBox.SelectedValue = csr.FrequencyId;
                    }
                }
                 
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show(); 
                }
            }
        }


        //Enable changing risk analysis fields
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frequencyComboBox.IsEnabled = true;

                severityComboBox.IsEnabled = true;

                CSRReview_RiskAnalysis_Button_SaveChanges.Visibility = System.Windows.Visibility.Visible;

                CSRReview_RiskAnalysis_Button_CancelChanging.Visibility = System.Windows.Visibility.Visible;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


        #region KB Notes

        //When KB notes are loaded
        private void kB_NotesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                kB_NotesDataGrid.SelectedIndex = -1;
            }
        }

        //Opens child window for adding new KB note to CSR
        private void btnAddKBNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.KBNotesDetails kbd = new ChildForms.KBNotesDetails((csrDataGrid.SelectedItem as Web.Csr).Id);

                kbd.Closed += new EventHandler(kbd_Closed);

                kbd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //When child window for add/edit KB is closed
        void kbd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.KBNotesDetails kbnd = (ChildForms.KBNotesDetails)sender;

                if (kbnd.NewNote != null)
                {
                    //If is new site entered
                    if (kbnd.isNew)
                    {
                        _context = (BusinessSystemsDomainContext)(kB_NotesDomainDataSource.DomainContext);
                        _context.KB_Notes.Add(kbnd.NewNote);
                        kB_NotesDomainDataSource.SubmitChanges();

                        currentKBNote = kbnd.NewNote;
                        attachmentList = kbnd.attachmentList;

                        kB_NotesDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(kB_NotesDomainDataSource_SubmittedChanges);
                    }
                    //If editing existing site
                    else
                    {
                        _context = (BusinessSystemsDomainContext)(kB_NotesDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.KB_Notes type = _context.KB_Notes.Where(t => t.Id == kbnd.NewNote.Id).First();

                        type.Heading = kbnd.NewNote.Heading;
                        type.Note = kbnd.NewNote.Note;
                        type.Description = kbnd.NewNote.Description;
                        type.ProductId = kbnd.NewNote.ProductId;

                        kB_NotesDomainDataSource.SubmitChanges();

                        currentKBNote = kbnd.NewNote;
                        attachmentList = kbnd.attachmentList;
                        
                        kB_NotesDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(kB_NotesDomainDataSource_SubmittedChanges);

                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

     

        //When KB note attachments are saved then we have to assign attachments to KB note
        private void OnKBAttachmentSubmitCompleted(SubmitOperation so)
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
                    //Foreach inserted attachment whe insert pair of attachemnt and KB note Id
                    foreach (Attachment attach in attachmentList)
                    {
                        AttachmenttAssign attachAssign = new AttachmenttAssign();
                        attachAssign.AttachmentId = attach.Id;
                        attachAssign.KBNoteId = currentKBNote.Id;

                        _context.AttachmenttAssigns.Add(attachAssign);
                    }

                    _context.SubmitChanges(OnKBAttachmentsAssignSubmitted, null);

                    attachmentList.Clear();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();


                }
            }
        }


        private void OnKBAttachmentsAssignSubmitted(SubmitOperation so)
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
                    kB_NotesDomainDataSource.Load();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();


                }
            }
        }

        //When KB otes are submitted
        void kB_NotesDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
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
                    //Add atachment into database

                    if (attachmentList.Count > 0)
                    {
                        foreach (Attachment attach in attachmentList)
                        {
                            _helperContext.Attachments.Add(attach);
                        }

                        _helperContext.SubmitChanges(OnKBAttachmentSubmitCompleted, null);
                    }
                    else
                    {
                        kB_NotesDomainDataSource.Load();
                    }

                    /////////////////////////////////////////////////////////////////////////////////////

                    kB_NotesDomainDataSource.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(kB_NotesDomainDataSource_SubmittedChanges);
                }
                 
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show(); 
                }
            }
           
        }

        //Opens child window for editing KB note
        private void btnEditKBNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.KBNotesDetails kbd = new ChildForms.KBNotesDetails(kB_NotesDataGrid.SelectedItem as Web.KB_Notes);

                kbd.Closed += new EventHandler(kbd_Closed);

                kbd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //When selection of KB record is changed
        private void kB_NotesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //Clear KB note controls
                KBIdTextBox.Text = String.Empty;
                KBTitleTextBox.Text = String.Empty;
                KBDescriptionTextBox.Text = String.Empty;
                KBSolutionTextBox.Text = String.Empty;
                KBAuthorTextBox.Text = String.Empty;
                KBNotesAttachment_ListBox.Items.Clear();

                //Fill KB note controls with data of selected KB note
                if (kB_NotesDataGrid.SelectedItem != null)
                {
                    KBIdTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Id.ToString();
                    KBTitleTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Heading;
                    KBDescriptionTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Note;
                    KBSolutionTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Description;
                    KBAuthorTextBox.Text = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).User.FirstName + " " + (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).User.LastName;

                    //Load selected KB note attachments
                    kBAttachmenttAssignDomainDataSource.QueryParameters[0].Value = (kB_NotesDataGrid.SelectedItem as Web.KB_Notes).Id;

                    kBAttachmenttAssignDomainDataSource.Load();

                    CSRReview_KB_Button_Edit.IsEnabled = true;
                }
                else
                {
                    CSRReview_KB_Button_Edit.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //Removes selected KB note
        private void btnRemoveKbNote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var k = kB_NotesDataGrid.SelectedItem;

                _context.KB_Notes.Remove(k as Web.KB_Notes);

                _context.SubmitChanges();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //When KB Note attachment is selected then tra to find it and open
        private void KBNotesAttachment_ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (KBNotesAttachment_ListBox.SelectedItem != null)
                {
                    var hostingWindow = System.Windows.Browser.HtmlPage.Window;

                    String path = "/Data/";

                    if (MainPage.AttachmentPath != null)
                        path = MainPage.AttachmentPath;

                    hostingWindow.Navigate(new Uri(path + (KBNotesAttachment_ListBox.SelectedItem as ListBoxItem).Name, UriKind.RelativeOrAbsolute), "_blank");

                    if (KBNotesAttachment_ListBox.Items.Count == 1)
                        KBNotesAttachment_ListBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        #endregion

      
        //Cancel update csr
        private void btnCsrUpdateCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CSRReview_Button_Save.Visibility = System.Windows.Visibility.Collapsed;
                CSRReview_Button_Cancel.Visibility = System.Windows.Visibility.Collapsed;

                companyStatusesComboBox.IsEnabled = false;
                companyPrioritiesComboBox.IsEnabled = false;

                requesterTypeInCompanyComboBox.IsEnabled = false;
                issueDomainInCompanyComboBox.IsEnabled = false;

                companyComboBox1.IsEnabled = false;
                productsInCompanyComboBox.IsEnabled = false;
                siteComboBox.IsEnabled = false;

                RemedyTextBox.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }


        /// <summary>
        /// Inserts new Note when CSR status is changed
        /// </summary>
        private void InsertNoteForCsrChanges()
        {
            try
            {

                if (csrChanges.Contains("Status changed to"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Status changed to " + (companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.StatusName;
                    note.Note = "Status changed to " + (companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.StatusName;
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }
                
                if (csrChanges.Contains("RequesterType changed to"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Type of requester changed to " + (requesterTypeInCompanyComboBox.SelectedItem as Web.RequesterTypeInCompany).RequesterType.Name;
                    note.Note = "Type of requester changed to " + (requesterTypeInCompanyComboBox.SelectedItem as Web.RequesterTypeInCompany).RequesterType.Name;
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }
                
                if (csrChanges.Contains("IssueDomain changed to"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Issue domain changed to " + (issueDomainInCompanyComboBox.SelectedItem as Web.IssueDomainInCompany).IssueDomain.Name;
                    note.Note = "Issue domain changed to " + (issueDomainInCompanyComboBox.SelectedItem as Web.IssueDomainInCompany).IssueDomain.Name;
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }
                //promjena kompanije
                if (csrChanges.Contains("Company changed to"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Company changed to " + (companyComboBox1.SelectedItem as Web.Company).CompanyName;
                    note.Note = "Company changed to " + (companyComboBox1.SelectedItem as Web.Company).CompanyName;
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }
                //promjena lokacije
                if (csrChanges.Contains("Location changed to"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Location changed to " + (siteComboBox.SelectedItem as Web.Site).SiteName;
                    note.Note = "Location changed to " + (issueDomainInCompanyComboBox.SelectedItem as Web.Site).SiteName;
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }
                //promjena proizvoda
                if (csrChanges.Contains("Product changed to"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Product changed to " + (productsInCompanyComboBox.SelectedItem as Web.ProductsInCompany).Product.ProductName;
                    note.Note = "Product changed to " + (productsInCompanyComboBox.SelectedItem as Web.ProductsInCompany).Product.ProductName;
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }

                //dodavanje remedy-a
                if (csrChanges.Contains("Remedy added"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Remedy added ";
                    note.Note = "Remedy added ";
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }

                //promjena remedy-a
                if (csrChanges.Contains("Remedy changed"))
                {
                    Notes note = new Notes();
                    note.AutorId = _currentUser.Id;
                    note.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                    note.Date = DateTime.Now;
                    note.Heading = "Remedy changed ";
                    note.Note = "Remedy changed ";
                    note.Description = "A";

                    _context = (BusinessSystemsDomainContext)(notesDomainDataSource.DomainContext);
                    _context.Notes.Add(note);
                }

                notesDomainDataSource.SubmitChanges();

                notesDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(notesDomainDataSource_SubmittedChanges);
                
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }





        //Save changes made od CSR
        private void btnSaveCsr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                 ///Ako je promjenjen status unesi zapis u notes
                if ((companyStatusesComboBox.SelectedItem != null) && ((csrDataGrid.SelectedItem as Csr).StatusId != (companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.Id))
                {
                    csrChanges += Environment.NewLine + "Status changed to: " + (companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.StatusName;

                }

                ///Ako je promjenjen prioritet unesi zapis u notes
                if ((companyPrioritiesComboBox.SelectedItem != null) && ((csrDataGrid.SelectedItem as Csr).PriorityId != (companyPrioritiesComboBox.SelectedItem as Web.CompanyPriorities).Priority.Id))
                {
                    csrChanges += Environment.NewLine + "Priority changed to: " + (companyPrioritiesComboBox.SelectedItem as Web.CompanyPriorities).Priority.PriorityName;

                }

                ///Ako je promjenjen tip podnositelja zahtjeva unesi zapis u notes
                if (requesterTypeInCompanyComboBox.SelectedItem != null)
                {
                    if (((csrDataGrid.SelectedItem as Csr).RequesterTypeId != (requesterTypeInCompanyComboBox.SelectedItem as Web.RequesterTypeInCompany).RequesterType.Id))
                    {
                        csrChanges += Environment.NewLine + "RequesterType changed to: " + (requesterTypeInCompanyComboBox.SelectedItem as Web.RequesterTypeInCompany).RequesterType.Name;

                    }
                }

                ///Ako je promjenjena domena zahtjeva unesi zapis u notes
                if (issueDomainInCompanyComboBox.SelectedItem != null) {
                  
                    if (((csrDataGrid.SelectedItem as Csr).IssueDomainId != (issueDomainInCompanyComboBox.SelectedItem as Web.IssueDomainInCompany).IssueDomain.Id))
                    {
                        csrChanges += Environment.NewLine + "IssueDomain changed to: " + (issueDomainInCompanyComboBox.SelectedItem as Web.IssueDomainInCompany).IssueDomain.Name;

                    }
                }
              

                ///Ako je promjenjena kompanija zahtjeva unesi zapis u notes
                if (companyComboBox1.SelectedItem != null)
                {
                    if (((csrDataGrid.SelectedItem as Csr).CompanyId != (companyComboBox1.SelectedItem as Web.Company).Id))
                    {
                        csrChanges += Environment.NewLine + "Company changed to: " + (companyComboBox1.SelectedItem as Web.Company).CompanyName;

                    }
                }

               
                ///Ako je promjenjena lokacija zahtjeva unesi zapis u notes
                if (siteComboBox.SelectedItem != null)
                {
                    if (((csrDataGrid.SelectedItem as Csr).SiteId != (siteComboBox.SelectedItem as Web.Site).Id))
                    {
                        csrChanges += Environment.NewLine + "Location changed to: " + (siteComboBox.SelectedItem as Web.Site).SiteName;

                    }
                }

                

                ///Ako je promjenjen proizvod zahtjeva unesi zapis u notes
                if (productsInCompanyComboBox.SelectedItem != null)
                {
                    if (((csrDataGrid.SelectedItem as Csr).ProductId != (productsInCompanyComboBox.SelectedItem as Web.ProductsInCompany).ProductId))
                    {
                        csrChanges += Environment.NewLine + "Product changed to: " + (productsInCompanyComboBox.SelectedItem as Web.ProductsInCompany).Product.ProductName;

                    }
                }

                //If answer field is changed the add to string that CSR answer was changed
                if (((csrDataGrid.SelectedItem as Csr).Answer != CsrAnswerTextBox.Text) && CsrAnswerTextBox.Text != String.Empty )
                {
                    //csrChanges += Environment.NewLine + "Answer changed: " + CsrAnswerTextBox.Text;
                }

              
                _context = (BusinessSystemsDomainContext)(csrDomainDataSource1.DomainContext);

                BusinessSystemsApp.Web.Csr type = _context.Csrs.Where(t => t.Id == Convert.ToInt32(CsrIdTextBox.Text)).First();
                type.StatusId = (companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.Id;
                type.PriorityId = (companyPrioritiesComboBox.SelectedItem as Web.CompanyPriorities).Priority.Id;
                if (requesterTypeInCompanyComboBox.SelectedItem != null)
                {
                    type.RequesterTypeId = (requesterTypeInCompanyComboBox.SelectedItem as Web.RequesterTypeInCompany).RequesterType.Id;
                }
                if (issueDomainInCompanyComboBox.SelectedItem != null)
                {
                    type.IssueDomainId = (issueDomainInCompanyComboBox.SelectedItem as Web.IssueDomainInCompany).IssueDomain.Id;
                }
                //update kompanije
                if (companyComboBox1.SelectedItem != null)
                {
                    type.CompanyId = (companyComboBox1.SelectedItem as Web.Company).Id;
                }
                //update proizvoda
                if (productsInCompanyComboBox.SelectedItem != null)
                {
                    type.ProductId = (productsInCompanyComboBox.SelectedItem as Web.ProductsInCompany).ProductId;
                }
                //update lokacije
                if (siteComboBox.SelectedItem != null)
                {
                    type.SiteId = (siteComboBox.SelectedItem as Web.Site).Id;
                }
                
                type.Answer = CsrAnswerTextBox.Text;

                if ((csrDataGrid.SelectedItem as Csr).Remedy != null)
                {
                    if (!(csrDataGrid.SelectedItem as Csr).Remedy.Equals(RemedyTextBox.Text))
                    {
                        csrChanges += Environment.NewLine + "Remedy changed!";

                        //change remedy
                        if (RemedyTextBox.Text != "")
                        {
                            type.Remedy = RemedyTextBox.Text;
                            type.RemedyTime = MainPage.ServerDateTime;
                        }
                    }
                }
                else {
                    
                        csrChanges += Environment.NewLine + "Remedy added!";

                        //change remedy
                        if (RemedyTextBox.Text != "")
                        {
                            csrChanges += Environment.NewLine + "Remedy added!";

                            type.Remedy = RemedyTextBox.Text;
                            type.RemedyTime = MainPage.ServerDateTime;
                        }
                    
                }
                
                
                
                //TODO change when Statuses are uniform

                //If CSR is answered
                if ((companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.StatusName.StartsWith("AW") && csrChanges != null)
                    //This date marks sql procedure to enter GetDate() datetime for answerdate
                    type.AnswerDate = MainPage.ServerDateTime;
                
                //If status is finished
                if (((companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.StatusName.StartsWith("FI") || (companyStatusesComboBox.SelectedItem as Web.CompanyStatuses).Csr_Status.StatusName.StartsWith("CLO")) && csrChanges != null)
                    //This date marks sql procedure to enter GetDate() datetime for answerdate
                    type.FinishDate = MainPage.ServerDateTime;
                    
                 /////////
                type.LastUserId = _currentUser.Id;


                CSRReview_Button_Save.IsEnabled = false;

                csrDomainDataSource1.SubmitChanges();
                csrDomainDataSource1.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(csrDomainDataSource1_SubmittedChanges);


                //Disable editing
                CSRReview_Button_Save.Visibility = System.Windows.Visibility.Collapsed;
                CSRReview_Button_Cancel.Visibility = System.Windows.Visibility.Collapsed;
                companyStatusesComboBox.IsEnabled = false;
                companyPrioritiesComboBox.IsEnabled = false;
                requesterTypeInCompanyComboBox.IsEnabled = false;
                issueDomainInCompanyComboBox.IsEnabled = false;
                companyComboBox1.IsEnabled = false;
                productsInCompanyComboBox.IsEnabled = false;
                siteComboBox.IsEnabled = false;

                
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }



        void csrDomainDataSource1_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();

                riskAnalysisInserted = false;

                csrDomainDataSource1.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(csrDomainDataSource1_SubmittedChanges);
            }
            else
            {
                try
                {
                   

                    //If CSR is saved for risk analysis then don't enter note just send notifications for users
                    if (riskAnalysisInserted)
                    {
                        //if (csrChanges != null)
                        //{
                        //    userNotificationsDomainDataSource.QueryParameters[0].Value = csr.CompanyId;
                        //    userNotificationsDomainDataSource.Load();
                        //}

                        riskAnalysisInserted = false;
                    }

                    else
                    {
                        if (csrChanges != null)
                        {
                            //Add note with description that CSR status was changed
                            InsertNoteForCsrChanges();

                        }
                        else
                        {
                            CSRReview_Button_Save.IsEnabled = true;
                        }
                        
                    }

                    csrDomainDataSource1.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(csrDomainDataSource1_SubmittedChanges);

                }

                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

       

        //Save changes on risk analysis of CSR
        private void btnRiskAnalysisSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ///Ako je promjenjen frequency spremi promjenu u string koji će se slati korisnicima
                if (frequencyComboBox.SelectedItem != null && ((csrDataGrid.SelectedItem as Csr).FrequencyId != (frequencyComboBox.SelectedItem as Web.Frequency).Id))
                {
                    //csrChanges += Environment.NewLine + "Frequency changed to: " + (frequencyComboBox.SelectedItem as Web.Frequency).FrequencyName;

                }

                ///Ako je promjenjen severity
                if (severityComboBox.SelectedItem != null && ((csrDataGrid.SelectedItem as Csr).SeverityId != (severityComboBox.SelectedItem as Web.Severity).Id))
                {
                    //csrChanges += Environment.NewLine + "Severity changed to: " + (severityComboBox.SelectedItem as Web.Severity).SeverityName;

                }

                _context = (BusinessSystemsDomainContext)(csrDomainDataSource1.DomainContext);

                BusinessSystemsApp.Web.Csr type = _context.Csrs.Where(t => t.Id == Convert.ToInt32(CsrIdTextBox.Text)).First();
                if(frequencyComboBox.SelectedItem != null)
                    type.FrequencyId = (frequencyComboBox.SelectedItem as Web.Frequency).Id;
                if(severityComboBox.SelectedItem != null)
                    type.SeverityId = (severityComboBox.SelectedItem as Web.Severity).Id;
                type.LastUserId = _currentUser.Id;

                

                riskAnalysisInserted = true;

                csrDomainDataSource1.SubmitChanges();
                csrDomainDataSource1.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(csrDomainDataSource1_SubmittedChanges);

                frequencyComboBox.IsEnabled = false;

                severityComboBox.IsEnabled = false;

                CSRReview_RiskAnalysis_Button_SaveChanges.Visibility = System.Windows.Visibility.Collapsed;

                CSRReview_RiskAnalysis_Button_CancelChanging.Visibility = System.Windows.Visibility.Collapsed;

                
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //Cancel risk analsis editing
        private void btnRiskAnalysisCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                frequencyComboBox.IsEnabled = false;

                severityComboBox.IsEnabled = false;

                CSRReview_RiskAnalysis_Button_SaveChanges.Visibility = System.Windows.Visibility.Collapsed;

                CSRReview_RiskAnalysis_Button_CancelChanging.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        #region LoadFilter

        //When companies list for filter is loaded
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
                    companyComboBox.ItemsSource = companyDomainDataSource.Data;

                    //Add "none" item to combo box list
                    Web.Company comp = new Web.Company();
                    comp.Id = 0;
                    comp.CompanyName = "- None -";

                    companyDomainDataSource.DataView.Add(comp);
                    
                    companyComboBox.SelectedIndex = companyComboBox.Items.Count -1;

                    companyList = new List<int>();

                    foreach (Web.Company company in companyDomainDataSource.DataView)
                    {
                        if (company.Id != 0)
                            companyList.Add(company.Id);
                    }

                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        //WhenProducts for filter is loaded
        private void productDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    productComboBox.ItemsSource = productDomainDataSource.Data;

                    //Add "None" item to combo box list
                    Web.Product prod = new Web.Product();
                    prod.Id = 0;
                    prod.ProductName = "- None -";

                    productDomainDataSource.DataView.Add(prod);

                    productComboBox.SelectedIndex = productComboBox.Items.Count-1;
                    

                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }


        private void companyStatusesDomainDataSource1_LoadedData(object sender, LoadedDataEventArgs e)
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
                    Filter_CsrStatuses_StackPanel.Children.Clear();

                    //Else if user is customer add only statuses for his company
                   // if (WebContext.Current.User.IsInRole("Customer") || WebContext.Current.User.IsInRole("Support engineer"))
                   // {
                        //Foreach loaded status add check box in list for filtering CSR-s
                        foreach (Web.CompanyStatuses status in companyStatusesDomainDataSource1.DataView)
                        {
                            CheckBox cb = new CheckBox();
                            cb.Name = status.Csr_Status.Id.ToString();
                            cb.Content = status.Csr_Status.StatusName;

                            Filter_CsrStatuses_StackPanel.Children.Add(cb);
                        }
                   // }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
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
            else
            {
                try
                {
                    //Else if user is customer add only statuses for his company
                    if (WebContext.Current.User.IsInRole("Customer") || WebContext.Current.User.IsInRole("Support engineer"))
                    {

                        ///////////
                        productComboBox.ItemsSource = productsInCompanyDomainDataSource.Data;

                        productComboBox.DisplayMemberPath = "Product.ProductName";
                        productComboBox.SelectedValuePath = "Product.Id";


                        //Add "None" item to combo box list
                        Web.ProductsInCompany prod = new Web.ProductsInCompany();

                        Web.Product prod1 = new Web.Product();
                        prod1.Id = 0;
                        prod1.ProductName = "- None -";

                        Web.Company comp = new Web.Company();
                        comp.Id = _currentUser.CompanyId;
                        comp.CompanyName = "";

                        prod.ProductId = 0;
                        prod.Product = prod1;
                        prod.CompanyId = _currentUser.CompanyId;
                        prod.Company = comp;

                        productsInCompanyDomainDataSource.DataView.Add(prod);

                        productComboBox.SelectedIndex = productComboBox.Items.Count - 1;

                    }

                    if (csrDataGrid.SelectedItem != null)
                    {
                        productsInCompanyComboBox.SelectedValue = (csrDataGrid.SelectedItem as Web.Csr).ProductId;
                    }
                    
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        //When filter expander is expanded
        private void CSReview_Expander_Header_Expanded(object sender, RoutedEventArgs e)
        {
            if (CSReview_Expander_Header != null)
            {
                
                CSReview_Expander_Header.Height = Double.NaN;
            }
          
        }


        //When filter expander is colapsed
        private void CSReview_Expander_Header_Collapsed(object sender, RoutedEventArgs e)
        {
            if (CSReview_Expander_Header != null)
            {
                CSReview_Expander_Header.Height = 23;
            }
        }


        #endregion

        //When filter button is selected
        private void CSRReview_Filter_Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CSReview_Expander_Header.Height = 23;
                CSReview_Expander_Header.IsExpanded = false;

                ClearCSRFrom();

                statuses.Clear();

                csrDomainDataSource1.QueryParameters.Clear();

                csrDomainDataSource1.QueryName = "GetFilteredCsrQuery";

                Parameter parId = new Parameter();
                parId.ParameterName = "_csrId";
                parId.Value = Filter_CsrId_TextBox.Text;

                Parameter parOnlyMine = new Parameter();
                parOnlyMine.ParameterName = "_onlyMine";
                parOnlyMine.Value = CSRReview_CheckBox_MyCSRs.IsChecked;

                Parameter parText = new Parameter();
                parText.ParameterName = "_text";
                parText.Value = CSRReview_Filter_SearchText.Text;

                Parameter parUserId = new Parameter();
                parUserId.ParameterName = "_userId";

                Parameter parCompany = new Parameter();
                parCompany.ParameterName = "_companyId";
                parCompany.Value = companyComboBox.SelectedValue;
              

                Parameter parProduct = new Parameter();
                parProduct.ParameterName = "_productId";
                parProduct.Value = productComboBox.SelectedValue;

                Parameter parRegFrom = new Parameter();
                parRegFrom.ParameterName = "_registerFrom";
                parRegFrom.Value = Filter_ReportedDate_From_datePicker.SelectedDate;

                Parameter parRegTo = new Parameter();
                parRegTo.ParameterName = "_registerTo";
                parRegTo.Value = Filter_ReportedDate_To_datePicker.SelectedDate;

                Parameter parFinFrom = new Parameter();
                parFinFrom.ParameterName = "_finishedFrom";
                parFinFrom.Value = Filter_FinishedDate_From_datePicker.SelectedDate;

                Parameter parFinTo = new Parameter();
                parFinTo.ParameterName = "_finishedTo";
                parFinTo.Value = Filter_FinishedDate_To_datePicker.SelectedDate;

                Parameter parCust = new Parameter();
                parCust.ParameterName = "_customer";


                List<Int32> companyList = new List<int>();

                if (Convert.ToInt32(companyComboBox.SelectedValue) == 0 && (!WebContext.Current.User.IsInRole("Admin") || !WebContext.Current.User.IsInRole("Super Admin")) )
                {
                    foreach (CompaniesAssignment company in companiesAssignmentDomainDataSource.DataView)
                    {
                        if (company.Company1.Id != 0)
                            companyList.Add(company.Company1.Id);
                    }
                }
                else
                {
                    companyList = null;
                }

                Parameter parCompanies = new Parameter();
                parCompanies.ParameterName = "_companiesList";
                parCompanies.Value = companyList;


                csrDomainDataSource1.QueryParameters.Add(parId);
                csrDomainDataSource1.QueryParameters.Add(parOnlyMine);
                csrDomainDataSource1.QueryParameters.Add(parText);
                csrDomainDataSource1.QueryParameters.Add(parUserId);
                csrDomainDataSource1.QueryParameters.Add(parCompany);
                csrDomainDataSource1.QueryParameters.Add(parProduct);
                csrDomainDataSource1.QueryParameters.Add(parRegFrom);
                csrDomainDataSource1.QueryParameters.Add(parRegTo);
                csrDomainDataSource1.QueryParameters.Add(parFinFrom);
                csrDomainDataSource1.QueryParameters.Add(parFinTo);
                csrDomainDataSource1.QueryParameters.Add(parCust);
                csrDomainDataSource1.QueryParameters.Add(parCompanies);
                
                

                //Remove previous added parameter with selected statuses to be able to add another one
                if(csrDomainDataSource1.QueryParameters.Count == 13)
                    csrDomainDataSource1.QueryParameters.RemoveAt(12);

                foreach (CheckBox cb in Filter_CsrStatuses_StackPanel.Children)
                {
                    if (cb.IsChecked == true)
                    {
                        statuses.Add(Convert.ToInt32(cb.Name));
                    }
                }
                
                //Add selected statuses in params
                Parameter par = new Parameter();
                par.ParameterName = "_statuses";
                par.Value = statuses;

                csrDomainDataSource1.QueryParameters.Add(par);

                csrDomainDataSource1.QueryParameters[3].Value = _currentUser.Id;

                //if (WebContext.Current.User.IsInRole("Admin"))
                    csrDomainDataSource1.QueryParameters[10].Value = false;
                //else
                    //csrDomainDataSource1.QueryParameters[10].Value = true;

                csrDataGrid.ItemsSource = csrDomainDataSource1.Data;

                FirstLoad = true;

                progressBar1.Visibility = System.Windows.Visibility.Visible;
                progressBar1.IsEnabled = true;

                if (csrDomainDataSource1.CanLoad)
                    csrDomainDataSource1.Load();
 
            }
             
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show(); 
            }
        }


        /// <summary>
        /// When CSRs are loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void csrDomainDataSource1_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();

                progressBar1.Visibility = System.Windows.Visibility.Collapsed;
                progressBar1.IsEnabled = false;
            }
            else
            {
                try
                {
                    //After load none CSR is selected
                    //csrDataGrid.SelectedItem = -1;

                    //dataPager1.Source = csrDomainDataSource1;

                    progressBar1.Visibility = System.Windows.Visibility.Collapsed;
                    progressBar1.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

     
        //When attachemnt for csr are loaded
        private void attachmenttAssignDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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

                    foreach (AttachmenttAssign attach in attachmenttAssignDomainDataSource.DataView)
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        //For new added attachments
                        if (attach.Attachment.Url != String.Empty && attach.Attachment.Url != null)
                        {
                            lbi.Name = attach.Attachment.Url;
                            lbi.Content = attach.Attachment.AttachmentName;
                        }
                        //For migrated attachments
                        else
                        {
                            lbi.Name = attach.Attachment.AttachmentName;
                            lbi.Content = attach.Attachment.AttachmentName;
                        }

                        AttachmentsListBox.Items.Add(lbi);
                        
                        
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

     
        //When attachment in list is selected then try to find it and open
        private void AttachmentsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (AttachmentsListBox.Items.Count > 0)
                {
                    if (AttachmentsListBox.SelectedItem != null)
                    {
                        var hostingWindow = System.Windows.Browser.HtmlPage.Window;

                        String path = "/Data/";

                        if (MainPage.AttachmentPath != null)
                            path = MainPage.AttachmentPath;

                        hostingWindow.Navigate(new Uri(path + (AttachmentsListBox.SelectedItem as ListBoxItem).Name, UriKind.RelativeOrAbsolute), "_blank");

                        
                        if(AttachmentsListBox.Items.Count == 1)
                            AttachmentsListBox.SelectedIndex = -1;
                    }
                }

            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        

        //Get selected KB Note attachemnts and show them in list
        private void attachmenttAssignDomainDataSource1_LoadedData(object sender, LoadedDataEventArgs e)
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

                    foreach (AttachmenttAssign attach in kBAttachmenttAssignDomainDataSource.DataView)
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Name = attach.Attachment.Url;
                        lbi.Content = attach.Attachment.AttachmentName;

                        KBNotesAttachment_ListBox.Items.Add(lbi);

                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

       

        private void attachmenttAssignDomainDataSource1_LoadedData_1(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void CsrAuthorTextBox_MouseEnter(object sender, MouseEventArgs e)
        {
            if(CsrAuthorTextBox.Text != String.Empty)
                UserDetailsGrid.Visibility = System.Windows.Visibility.Visible;
        }

        private void CsrAuthorTextBox_MouseLeave(object sender, MouseEventArgs e)
        {
            if(CsrAuthorTextBox.Text != String.Empty)
                UserDetailsGrid.Visibility = System.Windows.Visibility.Collapsed;
        }


        //Load contact for user in selected CSR
        private void contactDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    if ((contactDomainDataSource.DataView[0] as Web.Contact).Email != null)
                        CSRReview_PopUp_Email.Content = (contactDomainDataSource.DataView[0] as Web.Contact).Email;
                    if ((contactDomainDataSource.DataView[0] as Web.Contact).Fix1 != null)
                        CSRReview_PopUp_Fix1.Content = (contactDomainDataSource.DataView[0] as Web.Contact).Fix1;
                    if ((contactDomainDataSource.DataView[0] as Web.Contact).Fix2 != null)
                        CSRReview_PopUp_Fix2.Content = (contactDomainDataSource.DataView[0] as Web.Contact).Fix2;
                    if ((contactDomainDataSource.DataView[0] as Web.Contact).Mobile1 != null)
                        CSRReview_PopUp_Mobile1.Content = (contactDomainDataSource.DataView[0] as Web.Contact).Mobile1;
                    if ((contactDomainDataSource.DataView[0] as Web.Contact).Mobile2 != null)
                        CSRReview_PopUp_Mobile2.Content = (contactDomainDataSource.DataView[0] as Web.Contact).Mobile2;
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        

        private void userDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    if (userDomainDataSource.DataView.Count > 0)
                    {
                        _currentUser = userDomainDataSource.DataView[0] as Web.User;

                        //Clear paramters
                        csr_StatusDomainDataSource.QueryParameters.Clear();
                        csrDomainDataSource1.QueryParameters.Clear();

                        //If logged user is not admin then change data source query and add parameters
                        if (WebContext.Current.User.IsInRole("Customer"))
                        {

                            //csrDomainDataSource1.QueryName = "GetCsrCustomerQuery";

                            //Parameter par = new Parameter();
                            //par.ParameterName = "_userId";
                            //par.Value = _currentUser.Id;

                            //csrDomainDataSource1.QueryParameters.Add(par);

                            //companyStatusesDomainDataSource1.QueryParameters[0].Value = _currentUser.CompanyId;
                            //companyStatusesDomainDataSource1.Load();


                            //productsInCompanyDomainDataSource.QueryParameters[0].Value = _currentUser.CompanyId;
                            //productsInCompanyDomainDataSource.Load();

                            ////Add user company to company list in filter
                            //Web.Company comp = new Web.Company();
                            //comp.Id = _currentUser.CompanyId;
                            //comp.CompanyName = _currentUser.Company.CompanyName;

                            //companyDomainDataSource.DataView.Add(comp);

                            //Web.Company comp1 = new Web.Company();
                            //comp1.Id = 0;
                            //comp1.CompanyName = "- None -";

                            //companyComboBox.ItemsSource = companyDomainDataSource.Data;

                            //companyDomainDataSource.DataView.Add(comp1);

                            //companyComboBox.SelectedIndex = companyComboBox.Items.Count - 1;

                            ////Load CSRs for user
                            //csrDomainDataSource1.Load();

                            companiesAssignmentDomainDataSource.QueryParameters[0].Value = _currentUser.CompanyId;

                            companiesAssignmentDomainDataSource.Load();
                        }
                            //If logged user is Support engineer
                        else if (WebContext.Current.User.IsInRole("Support engineer"))
                        {
                        
                            companiesAssignmentDomainDataSource.QueryParameters[0].Value = _currentUser.CompanyId;

                            companiesAssignmentDomainDataSource.Load();

                          
                        }
                            //else user is administrator
                        else
                        {
                            //Load statuses
                            csr_StatusDomainDataSource.Load();

                            //Load lists in filter module
                            companyDomainDataSource.Load();
                            productDomainDataSource.Load();

                            //Load CSRs for user
                            csrDomainDataSource1.Load();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        #region CrystalReports
        /// <summary>
        /// OPens crystal report in new browser window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CSRReview_Report_LabelButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                HtmlPopupWindowOptions options = new HtmlPopupWindowOptions()
                {
                    Directories = false,
                    Location = false,
                    Menubar = false,
                    Status = false,
                    Toolbar = false,
                    Resizeable = true,
                    Height = Convert.ToInt32(HtmlPage.Window.Eval("screen.height")),
                    Width = Convert.ToInt32(HtmlPage.Window.Eval("screen.width")),
                    Left = 0,
                    Top = 0,


                };

                String statusList = "";

                //Put selected statuses in one string so it can be set as parameter in http request
                foreach (CheckBox cb in Filter_CsrStatuses_StackPanel.Children)
                {
                    if (cb.IsChecked == true)
                    {
                        if (statusList == "")
                            statusList += cb.Name;
                        else
                            statusList += "," + cb.Name;
                    }
                }

                String compList = "";

                //Put companies in one string so it can be set as parameter in http request
                foreach (int comp in companyList)
                {
                   
                    if (compList == "")
                        compList += comp.ToString();
                    else
                        compList += "," + Convert.ToString(comp);
                    
                }

                String type = null;

                if (WebContext.Current.User.IsInRole("Admin") || WebContext.Current.User.IsInRole("Super Admin"))
                    compList = String.Empty;

                if(WebContext.Current.User.IsInRole("Customer"))
                    type = "2";
                else
                    type = "1";

                String path = "http://localhost/";

                if (MainPage.ReportPath != null)
                    path = MainPage.ReportPath;

                String product = "0";

                if (productComboBox.SelectedValue != null)
                    product = productComboBox.SelectedValue.ToString();

                if (Convert.ToInt32(companyComboBox.SelectedValue) != 0)
                    compList = String.Empty;


                //Open browser window with parameters for report
                HtmlPage.PopupWindow(new Uri(path + "Report.aspx?Id=" + Filter_CsrId_TextBox.Text + "&Text=" + CSRReview_Filter_SearchText.Text + "&CompanyId=" + companyComboBox.SelectedValue + "&CompanyList=" + compList + "&ProductId=" + product + "&ReportedFrom=" + Filter_ReportedDate_From_datePicker.SelectedDate.Value.ToString("dd.MM.yyyy") + "&ReportedTo=" + Filter_ReportedDate_To_datePicker.SelectedDate.Value.ToString("dd.MM.yyyy") + "&FinishedFrom=" + Filter_FinishedDate_From_datePicker.SelectedDate.Value.ToString("dd.MM.yyyy") + "&FinishedTo=" + Filter_FinishedDate_To_datePicker.SelectedDate.Value.ToString("dd.MM.yyyy") + "&Mine=" + CSRReview_CheckBox_MyCSRs.IsChecked.ToString() + "&UserId=" + _currentUser.Id.ToString() + "&StatusList=" + statusList + "&Type=" + type), "_blank", options);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
            
        }

        private void CSRReview_Report_LabelButton_MouseEnter(object sender, MouseEventArgs e)
        {
            CSRReview_Report_LabelButton.FontSize = 12;
        }

        private void CSRReview_Report_LabelButton_MouseLeave(object sender, MouseEventArgs e)
        {
            CSRReview_Report_LabelButton.FontSize = 11;
        }

        #endregion

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
                    foreach (Web.UserNotifications notification in userNotificationsDomainDataSource.DataView)
                    {
                        if (notification.User.ContactId != null)
                        {
                            contactList.Add(notification.User.ContactId);
                        }
                    }

                    //Add current user e-mail to list
                    contactList.Add(_currentUser.ContactId);
                    
                    //Add user who reported csr to mail list
                    if(csr.User != null)
                        contactList.Add(csr.User.ContactId);

                    if (csr.User1 != null)
                        contactList.Add(csr.User1.ContactId);

                    //Add caller if exist to mail list
                    if(csr.User2 != null)
                        contactList.Add(csr.User2.ContactId);


                    //If list of contacts is not empty put contact ids in parameter of data source and load data source
                    if (contactList.Count > 0)
                    {
                        contactDomainDataSource1.QueryParameters[0].Value = contactList;
                        contactDomainDataSource1.Load();

                        contactList.Clear();
                    }
                    else
                    {
                        //csrDomainDataSource1.Load();
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

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
                    foreach (Contact con in contactDomainDataSource1.DataView)
                    {
                        if (con.Email != null && con.Email != String.Empty)
                        {
                            if(util.ValidEmail(con.Email))
                                addressList.Add(con.Email);
                        }
                    }

                    Helpers.ComposeAndSendEmail mail;

                    if (csrChanges != null)
                    {
                        //Send e-mail
                        mail = new Helpers.ComposeAndSendEmail(addressList,
                                                                                    csr.Id.ToString(),
                                                                                    csr.Heading,
                                                                                    csr.Description,
                                                                                    null,
                                                                                    csr.Company.CompanyName,
                                                                                    csr.RegisterDate.ToString(),

                                                                                    _currentUser.FirstName + " " + _currentUser.LastName,
                                                                                    null,
                                                                                    csr.Product.ProductName,
                                                                                    null,
                                                                                    null,
                                                                                    null,
                                                                                    Helpers.ComposeAndSendEmail.MailType.CSRChanges,
                                                                                    csrChanges);
                    }
                    else
                    {
                        //Send e-mail
                        mail = new Helpers.ComposeAndSendEmail(addressList,
                                                                                    csr.Id.ToString(),
                                                                                    currentNote.Heading,
                                                                                    currentNote.Note,
                                                                                    csr.Priority.PriorityName,
                                                                                    csr.Company.CompanyName,
                                                                                    csr.RegisterDate.ToString(),
                                                                                    _currentUser.FirstName + " " + _currentUser.LastName,
                                                                                    null,
                                                                                    null,
                                                                                    null,
                                                                                    csr.Product.ProductName,
                                                                                    "There is : " + AttachmentsListBox.Items.Count.ToString() + " for this note.",
                                                                                    Helpers.ComposeAndSendEmail.MailType.NewNote,
                                                                                    null);
                    }

                    mail.Send();
                    /////////////        

                    csrChanges = null;

                    addressList.Clear();

                    
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }
        
        /// <summary>
        /// Loads list of statuses for company of selected csr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void companyStatusesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    if (csr != null)
                    {
                        if(csr.StatusId != null)
                            companyStatusesComboBox.SelectedValue = csr.StatusId;
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
        /// Loads list of priorities for company of selected csr
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void companyPrioritiesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    if (csr != null)
                    {
                        if (csr.PriorityId != null)
                            companyPrioritiesComboBox.SelectedValue = csr.PriorityId;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }

            }
        }

        //When severity combobox is filled then select none severity or selected CSR severity
        private void severityComboBox_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                if (severityComboBox.IsEnabled == false)
                {
                    if (csr != null)
                    {
                        if (csr.SeverityId != null)
                            severityComboBox.SelectedValue = csr.SeverityId;
                        else
                            severityComboBox.SelectedIndex = -1;
                    }
                    else
                        severityComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //When frequency combobox is filled then select none frequency or selected CSR frequency
        private void frequencyComboBox_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                if (frequencyComboBox.IsEnabled == false)
                {
                    if (csr != null)
                    {
                        if (csr.FrequencyId != null)
                            frequencyComboBox.SelectedValue = csr.FrequencyId;
                        else
                            frequencyComboBox.SelectedIndex = -1;
                    }
                    else
                        frequencyComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        #region GUI Helper methods

        //When is clicked on textbox description set cursor at the end of text
        private void CsrDescriptionTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(SetCursorTextBoxDescription);
        }

        //When is clicked on textbox description set cursor at the end of text - INVOKE method
        private void SetCursorTextBoxDescription()
        {
            CsrDescriptionTextBox.Focus();
            CsrDescriptionTextBox.SelectionStart = CsrDescriptionTextBox.Text.Length;
        }

        //When is clicked on textbox answer set cursor at the end of text
        private void CsrAnswerTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.BeginInvoke(SetCursorTextBoxAnswer);
        }

        //When is clicked on textbox answer set cursor at the end of text - Invoke method
        private void SetCursorTextBoxAnswer()
        {
            CsrAnswerTextBox.Focus();
            CsrAnswerTextBox.SelectionStart = CsrAnswerTextBox.Text.Length;
        }


        //Disable changes on CSr desription field
        private void CsrDescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        //If is not edit mode selected disable changes to CSR Answer filed
        private void CsrAnswerTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (CSRReview_Button_Save.Visibility == System.Windows.Visibility.Collapsed)
                e.Handled = true;
        }

        //Disable changes to note field
        private void NoteNoteTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        //Disable changes on KBnote description
        private void KBDescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        //Disable changes to KB solution field
        private void KBSolutionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

       

        private void companyStatusesComboBox_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                if (companyStatusesComboBox.IsEnabled == false)
                {
                    if (csr != null)
                    {
                        if (csr.StatusId != null)
                            companyStatusesComboBox.SelectedValue = csr.StatusId;
                        else
                            companyStatusesComboBox.SelectedIndex = -1;
                    }
                    else
                        companyStatusesComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void companyPrioritiesComboBox_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                if (companyPrioritiesComboBox.IsEnabled == false)
                {
                    if (csr != null)
                    {
                        if (csr.PriorityId != null)
                            companyPrioritiesComboBox.SelectedValue = csr.PriorityId;
                        else
                            companyPrioritiesComboBox.SelectedIndex = -1;
                    }
                    else
                        companyPrioritiesComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        //
        private void AttachmentsListBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (AttachmentsListBox.Items.Count == 1)
                {
                    if (AttachmentsListBox.SelectedItem != null)
                    {
                        var hostingWindow = System.Windows.Browser.HtmlPage.Window;

                        String path = "/Data/";

                        if (MainPage.AttachmentPath != null)
                            path = MainPage.AttachmentPath;

                        hostingWindow.Navigate(new Uri(path + (AttachmentsListBox.SelectedItem as ListBoxItem).Name, UriKind.RelativeOrAbsolute), "_blank");
                    }       
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
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
            else
            {
                try
                {
                    if(companyList != null)
                        companyList.Clear();

                    companyComboBox.SelectedValuePath = "Company1.Id";
                    companyComboBox.DisplayMemberPath = "Company1.CompanyName";

                    companyComboBox.ItemsSource = companiesAssignmentDomainDataSource.Data;

                    CompaniesAssignment AssComp = new CompaniesAssignment();
                    Web.Company comp = new Web.Company();
                    comp.CompanyName = _currentUser.Company.CompanyName;
                    comp.Id = _currentUser.CompanyId;
                    AssComp.Company1 = comp;

                    Web.Company comp1 = new Web.Company();
                    comp1.CompanyName = "";
                    comp1.Id = 0;
                    AssComp.Company = comp1;

                    try
                    {
                        companiesAssignmentDomainDataSource.DataView.Add(AssComp);
                    }
                    catch
                    { }



                    CompaniesAssignment AssComp1 = new CompaniesAssignment();
                    Web.Company comp2 = new Web.Company();
                    comp2.CompanyName = "- None -";
                    comp2.Id = 0;
                    AssComp1.Company1 = comp2;

                    Web.Company comp12 = new Web.Company();
                    comp12.CompanyName = "";
                    comp12.Id = 0;
                    AssComp1.Company = comp12;

                    companiesAssignmentDomainDataSource.DataView.Add(AssComp1);

                    companyList = new List<int>();

                    foreach (CompaniesAssignment company in companiesAssignmentDomainDataSource.DataView)
                    {
                        if(company.Company1.Id != 0)
                            companyList.Add(company.Company1.Id);
                    }

                    Parameter par = new Parameter();
                    par.ParameterName = "_companiesList";
                    par.Value = companyList;

                    csrDomainDataSource1.QueryName = "GetCsrsForAllCompaniesQuery";

                    csrDomainDataSource1.QueryParameters.Add(par);

                    csrDomainDataSource1.Load();


                    //Parameter parComp = new Parameter();
                    //parComp.ParameterName = "_companiesList";
                    //parComp.Value = companyList;

                    //productsInCompanyDomainDataSource.QueryParameters.Clear();

                    //productsInCompanyDomainDataSource.QueryName = "GetProductsForAllCompaniesQuery";

                    //productsInCompanyDomainDataSource.QueryParameters.Add(parComp);

                    //productsInCompanyDomainDataSource.Load();

                 



                    companyComboBox.SelectionChanged +=new SelectionChangedEventHandler(companyComboBox_SelectionChanged);
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        private void companyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (Convert.ToInt32(companyComboBox.SelectedValue) != 0)
                {
                    productsInCompanyDomainDataSource.RejectChanges();

                    productsInCompanyDomainDataSource.QueryParameters[0].Value = companyComboBox.SelectedValue;
                    productsInCompanyDomainDataSource.Load();

                    Filter_CsrStatuses_StackPanel.Children.Clear();

                    companyStatusesDomainDataSource1.QueryParameters[0].Value = companyComboBox.SelectedValue;
                    companyStatusesDomainDataSource1.Load();

                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void CsrDescriptionTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            CsrDescriptionTextBox.Text = csr.Description;
        }

        private void CsrDescriptionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void CsrAnswerTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (CSRReview_Button_Save.Visibility == System.Windows.Visibility.Collapsed)
            {
                CsrAnswerTextBox.Text = csr.Answer;
            }
        }

        private void issueDomainInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    if (csr != null)
                    {
                        if (csr.IssueDomainId != null)
                            issueDomainInCompanyComboBox.SelectedValue = csr.IssueDomainId;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }

            }
        }

        private void requesterTypeInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {
                    if (csr != null)
                    {
                        if (csr.RequesterTypeId != null)
                            requesterTypeInCompanyComboBox.SelectedValue = csr.RequesterTypeId;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }

            }
        }

        private void requesterTypeInCompanyComboBox_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                if (requesterTypeInCompanyComboBox.IsEnabled == false)
                {
                    if (csr != null)
                    {
                        if (csr.RequesterTypeId != null)
                            requesterTypeInCompanyComboBox.SelectedValue = csr.RequesterTypeId;
                        else
                            requesterTypeInCompanyComboBox.SelectedIndex = -1;
                    }
                    else
                        requesterTypeInCompanyComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void issueDomainInCompanyComboBox_LayoutUpdated(object sender, EventArgs e)
        {
            try
            {
                if (issueDomainInCompanyComboBox.IsEnabled == false)
                {
                    if (csr != null)
                    {
                        if (csr.IssueDomainId != null)
                            issueDomainInCompanyComboBox.SelectedValue = csr.IssueDomainId;
                        else
                            issueDomainInCompanyComboBox.SelectedIndex = -1;
                    }
                    else
                        issueDomainInCompanyComboBox.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void requesterTypeInCompanyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void siteDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
            else
            {
                if (csrDataGrid.SelectedItem != null)
                {
                    siteComboBox.SelectedValue = (csrDataGrid.SelectedItem as Web.Csr).SiteId;
                }
            }
        }

        private void productsInCompanyDomainDataSource1_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void companyComboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (companyComboBox1.SelectedItem != null)
            {
                productsInCompanyDomainDataSource.QueryParameters[0].Value = (companyComboBox1.SelectedItem as Web.Company).Id;
                productsInCompanyDomainDataSource.Load();

                siteDomainDataSource.QueryParameters[0].Value = (companyComboBox1.SelectedItem as Web.Company).Id;
                siteDomainDataSource.Load();
            }
        }

        private void lblCompleteReport_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //csrDataGrid.Export();
        }

        private void lblCompleteReport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //DataGridExtensions.Export(csrDataGrid, dataPager1);
        }

        private void reportLink_Click(object sender, RoutedEventArgs e)
        {

            Uri pageUri = System.Windows.Browser.HtmlPage.Document.DocumentUri;

            
            UriBuilder baseUri = new UriBuilder();
            baseUri.Scheme = pageUri.Scheme;
            baseUri.Host = pageUri.Host;
            baseUri.Path = "BusinessSystemsApp.Web/Reporting.aspx";


            string queryToAppend = null;

            if (companyComboBox.SelectedValue != null)
            {
                queryToAppend = "companyId=" + companyComboBox.SelectedValue.ToString();

                if (baseUri.Query != null && baseUri.Query.Length > 1)
                    baseUri.Query = baseUri.Query.Substring(1) + "&" + queryToAppend;
                else
                    baseUri.Query = queryToAppend;
            }

            if (productComboBox.SelectedValue != null)
            {
                queryToAppend = "productId=" + productComboBox.SelectedValue.ToString();

                if (baseUri.Query != null && baseUri.Query.Length > 1)
                    baseUri.Query = baseUri.Query.Substring(1) + "&" + queryToAppend;
                else
                    baseUri.Query = queryToAppend;

            }

            HtmlPage.Window.Navigate(baseUri.Uri, "_blank");
        }
    }
}
