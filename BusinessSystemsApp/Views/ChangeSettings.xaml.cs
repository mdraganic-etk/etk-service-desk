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
using BusinessSystemsApp.Helpers;

namespace BusinessSystemsApp.Views
{
    public partial class ChangeSettings : Page
    {
        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        Web.User user;

        public ChangeSettings()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                //Get current user data from database
                userDomainDataSource.QueryName = "GetUserWithUsername";
                userDomainDataSource.QueryParameters.Clear();

                String username = WebContext.Current.Authentication.User.Identity.Name;

                var par = new Parameter();
                par.ParameterName = "_userName";
                par.Value = username;

                userDomainDataSource.QueryParameters.Add(par);

                userDomainDataSource.Load();

                userDomainDataSource.LoadedData += new EventHandler<LoadedDataEventArgs>(userDomainDataSource_LoadedData);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When data for user is loaded
        private void userDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            //If has error the show message
            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }

            //Else load form with user data    
            else
            {
                try
                {
                    if (userDomainDataSource.DataView.Count > 0)
                    {
                        user = userDomainDataSource.DataView[0] as Web.User;

                        usernameTextBox.Text = user.UserName;
                        firstNameTextBox.Text = user.FirstName;
                        lastNameTextBox.Text = user.LastName;

                        if (user.Contact.Fix1 != null)
                            fix1TextBox.Text = user.Contact.Fix1;
                        else
                            fix1TextBox.Text = String.Empty;
                        
                        if (user.Contact.Fix2 != null)
                            fix2TextBox.Text = user.Contact.Fix2;
                        else
                            fix2TextBox.Text = String.Empty;

                        if (user.Contact.Mobile1 != null)
                            mobile1TextBox.Text = user.Contact.Mobile1;
                        else
                            mobile1TextBox.Text = String.Empty;

                        if (user.Contact.Mobile2 != null)
                            mobile2TextBox.Text = user.Contact.Mobile2;
                        else
                            mobile2TextBox.Text = String.Empty;
                        
                        if (user.Contact.Email != null)
                            emailTextBox.Text = user.Contact.Email;
                        else
                            emailTextBox.Text = String.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                    msg.Show();
                }
            }
        }

        //When contact data is loaded
        private void contactDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //When Save button is selected
        private void btnSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                passwordAgainTextBox.ClearValidationError();
                passwordTextBox.ClearValidationError();

                ChangeSettings_Label_Saved.Content = String.Empty;

                string textWarning = BusinessSystemsApp.Resources.BS_Resources.ChangeSettings_PasswordsEqual;

                if (passwordTextBox.Text.Length > 0)
                {
                    if (passwordTextBox.Text != passwordAgainTextBox.Text || passwordTextBox.Text.Length < 6)
                    {
                        passwordAgainTextBox.SetValidation(textWarning);
                        passwordAgainTextBox.RaiseValidationError();

                        passwordTextBox.SetValidation(textWarning);
                        passwordTextBox.RaiseValidationError();
                    }
                    else
                    {
                        Helpers.HelperClasses.Util util = new Helpers.HelperClasses.Util();

                        user.Password = util.HashPassword(passwordTextBox.Text);

                        userDomainDataSource.SubmitChanges();



                        _context = (BusinessSystemsDomainContext)(contactDomainDataSource.DomainContext);

                        //Save user settings
                        Web.Contact con = _context.Contacts.Where(c => c.Id == user.ContactId).First();
                        con.Fix1 = fix1TextBox.Text;
                        con.Fix2 = fix2TextBox.Text;
                        con.Mobile1 = mobile1TextBox.Text;
                        con.Mobile2 = mobile2TextBox.Text;
                        con.Email = emailTextBox.Text;

                        contactDomainDataSource.SubmitChanges();

                        contactDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(contactDomainDataSource_SubmittedChanges);
                    }
                }
                else
                {
                    _context = (BusinessSystemsDomainContext)(contactDomainDataSource.DomainContext);

                    //Save user settings
                    Web.Contact con = _context.Contacts.Where(c => c.Id == user.ContactId).First();
                    con.Fix1 = fix1TextBox.Text;
                    con.Fix2 = fix2TextBox.Text;
                    con.Mobile1 = mobile1TextBox.Text;
                    con.Mobile2 = mobile2TextBox.Text;
                    con.Email = emailTextBox.Text;

                    contactDomainDataSource.SubmitChanges();

                    contactDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(contactDomainDataSource_SubmittedChanges);
                }
            }
            catch(Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
       
        }

        //When changes are submitted
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
                ChangeSettings_Label_Saved.Content = BusinessSystemsApp.Resources.BS_Resources.ChangeSettings_Label_Saved;
            }
        }

    }
}
