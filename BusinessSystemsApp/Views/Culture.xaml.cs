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
    public partial class Culture : Page
    {
        public Culture()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           //Fill grid column headers from resource
            userTypeDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Culture_Grid_Id;
            userTypeDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Culture_Grid_Language;
            userTypeDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Culture_Grid_Descripition;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Culture_Button_Delete.Visibility = System.Windows.Visibility.Visible;
                Culture_Button_New.Visibility = System.Windows.Visibility.Visible;
                Culture_Button_Edit.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void localizationDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.userTypeDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void UserTypes_Button_New_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.CultureDetails cd = new ChildForms.CultureDetails();
                cd.Closed += new EventHandler(cd_Closed);
                cd.Show();
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
        void cd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.CultureDetails cd = (ChildForms.CultureDetails)sender;

                //If dialog window result is ok
                if (cd.NewCulture != null)
                {
                    //If new user type
                    if (cd.isNew)
                    {
                        localizationDomainDataSource.DataView.Add(cd.NewCulture);

                        localizationDomainDataSource.SubmitChanges();
                    }
                    //If editing existing user type
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(localizationDomainDataSource.DomainContext);

                        //Find specified entity and update it
                        Localization type = _context.Localizations.Where(t => t.Id == cd.NewCulture.Id).First();
                        type.CultureCode = cd.NewCulture.CultureCode;
                        type.Description = cd.NewCulture.Description;

                        localizationDomainDataSource.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void UserTypes_Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var culture = userTypeDataGrid.SelectedItem;

                //Opens child window and passes user type for editing
                ChildForms.CultureDetails cd = new ChildForms.CultureDetails(culture as Localization);

                cd.Closed += new EventHandler(cd_Closed);
                cd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void UserTypes_Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helpers.MessageBoxDialog msg = new Helpers.MessageBoxDialog(BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Title, BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Text);
                msg.Show();

                msg.Closed += (s2, e2) =>
                {
                    if (msg.DialogResult == true)
                    {
                        var culture = userTypeDataGrid.SelectedItem;

                        //remove selected item
                        localizationDomainDataSource.DataView.Remove(culture);

                        localizationDomainDataSource.SubmitChanges();
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
