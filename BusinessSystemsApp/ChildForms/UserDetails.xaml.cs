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
using BusinessSystemsApp.Web;
using System.ServiceModel.DomainServices.Client;
using BusinessSystemsApp.Helpers;

namespace BusinessSystemsApp.ChildForms
{
    public partial class UserDetails : ChildWindow
    {
        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        public Contact con;

        public User NewUser { get; set; }

        public bool isNew = true;

        /// <summary>
        /// Initialize new window for add new user
        /// </summary>
        public UserDetails()
        {
            InitializeComponent();

            NewUser = new User();

            isNew = true;

        }

        /// <summary>
        /// Initialize new window for editing selected user
        /// </summary>
        /// <param name="type">User instance for editing</param>
        public UserDetails(User type)
        {


            InitializeComponent();

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewUser = new User();


            NewUser.Id = type.Id;
            NewUser.FirstName = type.FirstName;
            NewUser.LastName = type.LastName;
            NewUser.Password = type.Password;

            NewUser.TypeId = type.TypeId;
            NewUser.ContactId = type.ContactId;
            NewUser.CompanyId = type.CompanyId;
            NewUser.CultureId = type.CultureId;


            idTextBox.Text = type.Id.ToString();
            
            if(type.FirstName != null)
                firstNameTextBox.Text = type.FirstName;

            if(type.LastName != null)
                lastNameTextBox.Text = type.LastName;
            
            if(type.UserName != null)
                usernameTextBox.Text = type.UserName;

        }

        private bool ValidateInput()
        {
            bool ok = true;

            passwordTextBox.ClearValidationError();
            companyComboBox.ClearValidationError();
            userTypeComboBox.ClearValidationError();

            string textWarning = BusinessSystemsApp.Resources.BS_Resources.UserDetails_PasswordValid;

            string textWarning2 = BusinessSystemsApp.Resources.BS_Resources.CsrReport_RequiredInput;

            if (passwordTextBox.Text.Length > 0 && passwordTextBox.Text.Length < 6)
            {
                passwordTextBox.SetValidation(textWarning);
                passwordTextBox.RaiseValidationError();
                
                ok = false;
            }

            if ((int)companyComboBox.SelectedValue == 0)
            {
                companyComboBox.SetValidation(textWarning2);
                companyComboBox.RaiseValidationError();

                ok = false;
            }

            if ((int)userTypeComboBox.SelectedValue == 0)
            {
                userTypeComboBox.SetValidation(textWarning2);
                userTypeComboBox.RaiseValidationError();

                ok = false;
            }

            return ok;
        }

        //When button OK is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            bool duplicate = false;

            if(ValidateInput())
            {
                _context = (BusinessSystemsDomainContext)(contactDomainDataSource.DomainContext);

              

                    if (isNew)
                    {

                        foreach (Object user in userDomainDataSource.DataView)
                        {
                            if ((user as Web.User).UserName.Equals(usernameTextBox.Text))
                            {
                                duplicate = true;
                                break;
                            }
                        }
                
                        if (!duplicate)
                        {
                            con = new Contact();
                            con.Fix1 = fix1TextBox.Text;
                            con.Fix2 = fix2TextBox.Text;
                            con.Mobile1 = mobile1TextBox.Text;
                            con.Mobile2 = mobile2TextBox.Text;
                            con.Email = emailTextBox.Text;

                            _context.Contacts.Add(con);

                            contactDomainDataSource.SubmitChanges();
                            contactDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(contactDomainDataSource_SubmittedChanges);
                        }
                        else
                        {
                            System.Windows.MessageBox.Show(BusinessSystemsApp.Resources.BS_Resources.UserDetails_DuplicateUser_Text, BusinessSystemsApp.Resources.BS_Resources.UserDetails_DuplicateUser_Text, MessageBoxButton.OK);
                        }
                    }

                    else
                    {
                        Contact con = _context.Contacts.Where(c => c.Id == NewUser.ContactId).First();
                        con.Fix1 = fix1TextBox.Text;
                        con.Fix2 = fix2TextBox.Text;
                        con.Mobile1 = mobile1TextBox.Text;
                        con.Mobile2 = mobile2TextBox.Text;
                        con.Email = emailTextBox.Text;

                        contactDomainDataSource.SubmitChanges();

                        contactDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(contactDomainDataSource_SubmittedChanges);
                    }
            }
        }

        //When contact changes are submitted
        void contactDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
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
                    Helpers.HelperClasses.Util util = new Helpers.HelperClasses.Util();

                    NewUser.FirstName = firstNameTextBox.Text;
                    NewUser.LastName = lastNameTextBox.Text;
                    NewUser.UserName = usernameTextBox.Text;
                    
                    if(passwordTextBox.Text.Trim() != String.Empty)
                        NewUser.Password = util.HashPassword(passwordTextBox.Text);

                    if (isNew)
                        NewUser.ContactId = con.Id;

                    NewUser.CompanyId = Convert.ToInt32(companyComboBox.SelectedValue);
                    NewUser.TypeId = Convert.ToInt32(userTypeComboBox.SelectedValue);

                    if (localizationComboBox.SelectedValue != null && (int)localizationComboBox.SelectedValue != 0)
                        NewUser.CultureId = Convert.ToInt32(localizationComboBox.SelectedValue);

                    this.DialogResult = true;
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        //When Cancel button is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewUser = null;
            this.DialogResult = false;
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
                companyDomainDataSource.Load();

                userTypeDomainDataSource.Load();

                localizationDomainDataSource.Load();
            }
        }

        //When companies are loaded
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
                    Company comp = new Company();
                    comp.Id = 0;
                    comp.CompanyName = "";

                    companyDomainDataSource.DataView.Add(comp);

                    companyComboBox.SelectedValue = 0;

                    if (NewUser != null)
                    {
                        if (NewUser.CompanyId != 0)
                        {
                            companyComboBox.SelectedValue = NewUser.CompanyId;
                        }
                        else
                            companyComboBox.SelectedValue = 0;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        private void userTypeDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    if (!WebContext.Current.User.IsInRole("Super Admin"))
                    {
                        foreach (Object type in userTypeDomainDataSource.DataView)
                        {
                            if ((type as UserType).TypeName == "Super Admin")
                            {
                                userTypeDomainDataSource.DataView.Remove(type);
                                break;
                            }
                        }
                    }
                    
                    UserType ut = new UserType();
                    ut.Id = 0;
                    ut.TypeName = "";

                    userTypeDomainDataSource.DataView.Add(ut);

                    userTypeComboBox.SelectedValue = 0;

                    if (NewUser != null)
                    {
                        if (NewUser.TypeId != 0)
                        {
                            userTypeComboBox.SelectedValue = NewUser.TypeId;
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


        //When contact data is loaded
        private void contactDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            try
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
                        _context = (BusinessSystemsDomainContext)(contactDomainDataSource.DomainContext);

                        if (_context.Contacts.Where(c => c.Id == this.NewUser.ContactId).Count() != 0)
                        {
                            BusinessSystemsApp.Web.Contact con = _context.Contacts.Where(c => c.Id == this.NewUser.ContactId).First();

                           
                            contactIdTextBox.Text = con.Id.ToString();
                            if (con.Fix1 != null)
                                fix1TextBox.Text = con.Fix1;
                            if (con.Fix2 != null)
                                fix2TextBox.Text = con.Fix2;
                            if (con.Mobile1 != null)
                                mobile1TextBox.Text = con.Mobile1;
                            if (con.Mobile2 != null)
                                mobile2TextBox.Text = con.Mobile2;
                            if (con.Email != null)
                                emailTextBox.Text = con.Email;
                        }
                    }
                    catch (Exception ex)
                    {
                        Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                        msg.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        private void localizationDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
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
                    Localization loc = new Localization();
                    loc.Id = 0;
                    loc.CultureCode = "";
                  

                    localizationDomainDataSource.DataView.Add(loc);

                    if (NewUser.CultureId != null)
                        localizationComboBox.SelectedValue = NewUser.CultureId;
                    else
                        localizationComboBox.SelectedValue = 0;
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }

            }
        }

        private void UserDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.UserDetails_Window_Title;
        }

        private void companyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }
       
    }
       
}

