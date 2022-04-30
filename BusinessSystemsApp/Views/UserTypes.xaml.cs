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
    public partial class UserTypes : Page
    {
        public UserTypes()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            userTypeDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.UserType_Grid_Id;
            userTypeDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.UserType_Grid_Name;
            userTypeDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.UserType_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                UserTypes_Button_Delete.Visibility = System.Windows.Visibility.Visible;
                UserTypes_Button_Edit.Visibility = System.Windows.Visibility.Visible;
                UserTypes_Button_New.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void userTypeDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.userTypeDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        /// <summary>
        /// Method opens child window for adding new user type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.UserTypeDetails utd = new ChildForms.UserTypeDetails();
                utd.Closed += new EventHandler(utd_Closed);
                utd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// This method i called when child window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void utd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.UserTypeDetails utd = (ChildForms.UserTypeDetails)sender;

                //If dialog window result is ok
                if (utd.NewUser != null)
                {
                    //If new user type
                    if (utd.isNew)
                    {
                        userTypeDomainDataSource.DataView.Add(utd.NewUser);

                        userTypeDomainDataSource.SubmitChanges();
                    }
                    //If editing existing user type
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(userTypeDomainDataSource.DomainContext);

                        //Find specified entity and update it
                        UserType type = _context.UserTypes.Where(t => t.Id == utd.NewUser.Id).First();
                        type.TypeName = utd.NewUser.TypeName;
                        type.Description = utd.NewUser.Description;

                        userTypeDomainDataSource.SubmitChanges();
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
        /// Method opens child window for editing selected user type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var userType = userTypeDataGrid.SelectedItem;

                //Opens child window and passes user type for editing
                ChildForms.UserTypeDetails utd = new ChildForms.UserTypeDetails(userType as UserType);

                utd.Closed += new EventHandler(utd_Closed);
                utd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// Deletes user type item from collection
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
                        var userType = userTypeDataGrid.SelectedItem;

                        //remove selected item
                        userTypeDomainDataSource.DataView.Remove(userType);

                        userTypeDomainDataSource.SubmitChanges();
                    }
                };
              
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

    }
}
