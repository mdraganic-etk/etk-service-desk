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
using System.ServiceModel.DomainServices.Client;
using BusinessSystemsApp.Web;
using BusinessSystemsApp.Helpers;

namespace BusinessSystemsApp.ChildForms
{
    public partial class KBNotesDetails : ChildWindow
    {
        public KB_Notes NewNote { get; set; }

        public bool isNew = true;

        public List<Attachment> attachmentList = new List<Attachment>();

        Web.User user;

        /// <summary>
        /// Open window for adding new note fro selected CSR
        /// </summary>
        /// <param name="_csrId">Id of CSR</param>
        public KBNotesDetails(Int32 _csrId)
        {
            InitializeComponent();

            KbNotesDetails_Label_Attachments.Visibility = System.Windows.Visibility.Collapsed;
            AttachmentsListBox.Visibility = System.Windows.Visibility.Collapsed;
            AddAachmentsButton.Visibility = System.Windows.Visibility.Collapsed;


            NewNote = new KB_Notes();

            CsrIdTextBox.Text = _csrId.ToString();

            isNew = true;

            DateTextBox.Text = DateTime.Now.ToString("dd.MMMM.yyyy hh:mm");

            ///////Get user First and Last name
            userDomainDataSource.QueryName = "GetUserWithUsername";
            userDomainDataSource.QueryParameters.Clear();

            String username = WebContext.Current.Authentication.User.Identity.Name;

            var par = new Parameter();
            par.ParameterName = "_userName";
            par.Value = username;

            userDomainDataSource.QueryParameters.Add(par);

            userDomainDataSource.Load();

            userDomainDataSource.LoadedData += new EventHandler<LoadedDataEventArgs>(userDomainDataSource_LoadedData);

            ////
        }


        /// <summary>
        /// OPen window for editing selected note
        /// </summary>
        /// <param name="type">KB_Note instance</param>
        public KBNotesDetails(KB_Notes type)
        {
            InitializeComponent();

            CsrReport_Expander.Visibility = System.Windows.Visibility.Collapsed;

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewNote = new KB_Notes();


            productDomainDataSource.LayoutUpdated += new EventHandler(productDomainDataSource_LayoutUpdated);

            NewNote.Id = type.Id;
            NewNote.Heading = type.Heading;
            NewNote.CsrId = type.CsrId;
            NewNote.Date = type.Date;
            NewNote.Note = type.Note;
            NewNote.Description = type.Description;
            NewNote.ProductId = type.ProductId;
            NewNote.Description = type.Description;


            CsrIdTextBox.Text = type.CsrId.ToString();
            idTextBox.Text = type.Id.ToString();
            HeadingTextBox.Text = type.Heading;
            NoteTextBox.Text = type.Note;
            NoteSolutionTextBox.Text = type.Description;
            DateTextBox.Text = type.Date.ToString("dd.MMMM.yyyy hh:mm");

            //Get user first and last name
            userDomainDataSource.QueryName = "GetUserWithUsername";
            userDomainDataSource.QueryParameters.Clear();

            String username = WebContext.Current.Authentication.User.Identity.Name;

            var par = new Parameter();
            par.ParameterName = "_userName";
            par.Value = username;

            userDomainDataSource.QueryParameters.Add(par);

            userDomainDataSource.Load();

            userDomainDataSource.LoadedData += new EventHandler<LoadedDataEventArgs>(userDomainDataSource_LoadedData);

            ////

            //Load selected note attachments
            attachmenttAssignDomainDataSource.QueryParameters[0].Value = type.Id;

            attachmenttAssignDomainDataSource.Load();

            ////

        }

        //When layout is updated
        void productDomainDataSource_LayoutUpdated(object sender, EventArgs e)
        {
            productComboBox.SelectedValue = NewNote.ProductId;

            productDomainDataSource.LayoutUpdated -= new EventHandler(productDomainDataSource_LayoutUpdated);
        }


        //When button OK is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRequiredInput())
            {
                NewNote.Heading = HeadingTextBox.Text;
                NewNote.Note = NoteTextBox.Text;
                NewNote.Date = Convert.ToDateTime(DateTextBox.Text);
                NewNote.CsrId = Convert.ToInt32(CsrIdTextBox.Text);
                NewNote.ProductId = Convert.ToInt32(productComboBox.SelectedValue);
                NewNote.Description = NoteSolutionTextBox.Text;

                //Upload attachments
                if (fileUploaderControl1.Files.Count > 0)
                {

                    fileUploaderControl1.UploadFilesMethod();
                    fileUploaderControl1.Files.AllFilesFinished += new EventHandler(Files_AllFilesFinished);
                }
                else
                {
                    this.DialogResult = true;
                }
            }
        }

        void Files_AllFilesFinished(object sender, EventArgs e)
        {

            foreach (Vci.Silverlight.FileUploader.UserFile file in fileUploaderControl1.Files)
            {

                Attachment attach = new Attachment();
                attach.AttachmentName = file.FileName;
                attach.Url = file.Guid;

                attachmentList.Add(attach);

            }

            this.DialogResult = true;
        }


        //When cancel button is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewNote = null;

            this.DialogResult = false;
        }

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
                productComboBox.SelectedValue = NewNote.ProductId;
            }
        }

       


        //When user data is loaded
        private void userDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                        user = userDomainDataSource.DataView[0] as Web.User;

                        AuthorTextBox.Text = user.FirstName + " " + user.LastName;

                        if (isNew)
                        {
                            NewNote.AutorId = user.Id;
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

        private void AddAachmentsButton_Click(object sender, RoutedEventArgs e)
        {
            KbNotesDetails_Label_Attachments.Visibility = System.Windows.Visibility.Collapsed;
            AttachmentsListBox.Visibility = System.Windows.Visibility.Collapsed;
            AddAachmentsButton.Visibility = System.Windows.Visibility.Collapsed;

            CsrReport_Expander.Visibility = System.Windows.Visibility.Visible;
            CancelEditAttachmentsButton.Visibility = System.Windows.Visibility.Visible;
        }

        private void button1_Click_1(object sender, RoutedEventArgs e)
        {
            KbNotesDetails_Label_Attachments.Visibility = System.Windows.Visibility.Visible;
            AttachmentsListBox.Visibility = System.Windows.Visibility.Visible;
            AddAachmentsButton.Visibility = System.Windows.Visibility.Visible;

            CsrReport_Expander.Visibility = System.Windows.Visibility.Collapsed;
            CancelEditAttachmentsButton.Visibility = System.Windows.Visibility.Collapsed;
        }


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
                        lbi.Name = attach.Attachment.Url;
                        lbi.Content = attach.Attachment.AttachmentName;

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

        private void KBNotesDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.KBNotesDetails_Window_Title;
        }

        //Check required input controls
        private bool CheckRequiredInput()
        {
            bool isFormValid = true;

            HeadingTextBox.ClearValidationError();
            NoteTextBox.ClearValidationError();
            NoteSolutionTextBox.ClearValidationError();
            productComboBox.ClearValidationError();

            string textWarning = BusinessSystemsApp.Resources.BS_Resources.CsrReport_RequiredInput;

            if (HeadingTextBox.Text == String.Empty)
            {
                HeadingTextBox.SetValidation(textWarning);
                HeadingTextBox.RaiseValidationError();
                isFormValid = false;
            }

            if (NoteTextBox.Text == String.Empty)
            {
                NoteTextBox.SetValidation(textWarning);
                NoteTextBox.RaiseValidationError();
                isFormValid = false;
            }

            if (NoteSolutionTextBox.Text == String.Empty)
            {
                NoteSolutionTextBox.SetValidation(textWarning);
                NoteSolutionTextBox.RaiseValidationError();
                isFormValid = false;
            }

            if (productComboBox.SelectedItem == null)
            {
                productComboBox.SetValidation(textWarning);
                productComboBox.RaiseValidationError();
                isFormValid = false;
            }

            return isFormValid;
        }
    }
}

