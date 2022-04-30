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

namespace BusinessSystemsApp.Views
{
    public partial class Users : Page
    {
        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        public Users()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            userDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.User_Grid_Id;
            userDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.User_Grid_FirstName;
            userDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.User_Grid_LastName;
            userDataGrid.Columns[3].Header = BusinessSystemsApp.Resources.BS_Resources.User_Grid_Username;
            userDataGrid.Columns[7].Header = BusinessSystemsApp.Resources.BS_Resources.User_Grid_UserType;
            userDataGrid.Columns[8].Header = BusinessSystemsApp.Resources.BS_Resources.User_Grid_Company;

            pbUsers.IsIndeterminate = true;
            pbUsers.Visibility = System.Windows.Visibility.Visible;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                User_Button_Delete.Visibility = System.Windows.Visibility.Visible;
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
                    this.userDataGrid.MaxHeight = LayoutRoot.ActualHeight;

                    dataPager1.Source = userDataGrid.ItemsSource;

                    pbUsers.IsIndeterminate = false;
                    pbUsers.Visibility = System.Windows.Visibility.Collapsed;

                    //Helpers.HelperClasses.Util util = new Helpers.HelperClasses.Util();

                    //foreach (User user in userDomainDataSource.Data)
                    //{
                    //    if(user.Password != null && (user.Id == 16 || user.Id == 21 || user.Id == 36)) 
                    //        user.Password = util.HashPassword(user.Password);
                    //}

                    //userDomainDataSource.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                    msg.Show();
                }
            }

            
        }

        /// <summary>
        /// Opens child window for insert new user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.UserDetails ud = new ChildForms.UserDetails();
                ud.Closed += new EventHandler(ud_Closed);
                ud.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }

        }

        /// <summary>
        /// Handle user When child window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ud_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.UserDetails ud = (ChildForms.UserDetails)sender;

                //If child window result is OK
                if (ud.NewUser != null && ud.DialogResult == true)
                {
                    //If new user is entered
                    if (ud.isNew)
                    {
                        _context = (BusinessSystemsDomainContext)(userDomainDataSource.DomainContext);

                        _context.Users.Add(ud.NewUser);
                        userDomainDataSource.SubmitChanges();

                        userDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(userDomainDataSource_SubmittedChanges);
                    }
                    //If it is user edit
                    else
                    {
                        _context = (BusinessSystemsDomainContext)(userDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.User type = _context.Users.Where(t => t.Id == ud.NewUser.Id).First();
                        type.FirstName = ud.NewUser.FirstName;
                        type.LastName = ud.NewUser.LastName;
                        type.UserName = ud.NewUser.UserName;
                        type.Password = ud.NewUser.Password;
                        type.CompanyId = ud.NewUser.CompanyId;
                        type.TypeId = ud.NewUser.TypeId;
                        type.CultureId = ud.NewUser.CultureId;

                        userDomainDataSource.SubmitChanges();
                        userDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(userDomainDataSource_SubmittedChanges);

                    }
                }
            }
             
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// Occures when userDomainDataSource is submitted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void userDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
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
                    userDomainDataSource.Load();

                    //userDataGrid.ItemsSource = _context.Users;
                    //_context.Load(_context.GetUserQuery());

                    userDomainDataSource.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(userDomainDataSource_SubmittedChanges);
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                    msg.Show();
                }
            }
        }

    
        /// <summary>
        /// When company data is loded set selected value
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                Web.Company comp = new Web.Company();
                comp.Id = 0;
                comp.CompanyName = "- None -";

                bool exists = false;

                foreach (Object company in companyDomainDataSource.DataView)
                {
                    if ((company as Web.Company).Id == 0)
                    {
                        exists = true;
                    }
                }

                if(!exists)
                    companyDomainDataSource.DataView.Add(comp);

                companyComboBox.SelectedValue = 0;
            }
        }

        /// <summary>
        /// Open child window for editing selected user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = userDataGrid.SelectedItem;

                ChildForms.UserDetails ud = new ChildForms.UserDetails(user as BusinessSystemsApp.Web.User);

                ud.Closed += new EventHandler(ud_Closed);
                ud.Show();
            }
             

            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// Delete selected user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helpers.MessageBoxDialog msg = new Helpers.MessageBoxDialog(BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Title, BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Text);
                msg.Show();

                msg.Closed += (s2, e2) =>
                {
                    if (msg.DialogResult == true)
                    {
                        var user = userDataGrid.SelectedItem;

                        userDomainDataSource.DataView.Remove(user);
                        userDomainDataSource.SubmitChanges();
                    }
                };
         
            }
             
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        
        private void companyComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Delete)
            //{
            //    companyComboBox.SelectedItem = null;
            //}
        }

        private void Users_Expander_Header_Collapsed(object sender, RoutedEventArgs e)
        {
            if (Users_Expander_Header != null)
            {
                Users_Expander_Header.Height = 23; 
            }
        }

        private void Users_Expander_Header_Expanded(object sender, RoutedEventArgs e)
        {
            if (Users_Expander_Header != null)
            {
                Users_Expander_Header.Height = Double.NaN;
            }

        }

       

        private void CSRReview_Filter_Button_Filter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                userDomainDataSource.QueryParameters.Clear();

                userDomainDataSource.QueryName = "GetUserWithParamsQuery";
                
                Parameter parId = new Parameter();
                parId.ParameterName = "_id";
                parId.Value = UserId_TextBox.Text;

                Parameter parFirstName = new Parameter();
                parFirstName.ParameterName = "_firstName";
                parFirstName.Value = firstNameTextBox.Text;

                Parameter parLastName = new Parameter();
                parLastName.ParameterName = "_lastName";
                parLastName.Value = lastNameTextBox.Text;

                Parameter parCompId = new Parameter();
                parCompId.ParameterName = "_companyId";
                parCompId.Value = companyComboBox.SelectedValue;

              
                userDomainDataSource.QueryParameters.Add(parId);
                userDomainDataSource.QueryParameters.Add(parFirstName);
                userDomainDataSource.QueryParameters.Add(parLastName);
                userDomainDataSource.QueryParameters.Add(parCompId);

                if (userDomainDataSource.CanLoad)
                    userDomainDataSource.Load();

                Users_Expander_Header.IsExpanded = false;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

    }
}
