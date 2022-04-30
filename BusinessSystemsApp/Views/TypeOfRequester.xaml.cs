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
    public partial class TypeOfRequester : Page
    {
        public TypeOfRequester()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            requesterTypeDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.RequesterType_Grid_Id;
            requesterTypeDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.RequesterType_Grid_Name;
            requesterTypeDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.RequesterType_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                TypeOfRequester_Button_Delete.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void requesterTypeDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.requesterTypeDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void TypeOfRequester_Button_New_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.TypeOfRequesterDetails pd = new ChildForms.TypeOfRequesterDetails();
                pd.Closed += new EventHandler(pd_Closed);
                pd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void TypeOfRequester_Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var prod = requesterTypeDataGrid.SelectedItem;


                ChildForms.TypeOfRequesterDetails pd = new ChildForms.TypeOfRequesterDetails(prod as BusinessSystemsApp.Web.RequesterType);

                pd.Closed += new EventHandler(pd_Closed);
                pd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void TypeOfRequester_Button_Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helpers.MessageBoxDialog msg = new Helpers.MessageBoxDialog(BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Title, BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Text);
                msg.Show();

                msg.Closed += (s2, e2) =>
                {
                    if (msg.DialogResult == true)
                    {
                        var prod = requesterTypeDataGrid.SelectedItem;

                        requesterTypeDomainDataSource.DataView.Remove(prod);

                        requesterTypeDomainDataSource.SubmitChanges();
                    }
                };

            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        /// <summary>
        /// Occures when child window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.TypeOfRequesterDetails pd = (ChildForms.TypeOfRequesterDetails)sender;

                if (pd.NewRequesterType != null)
                {
                    //If is new product
                    if (pd.isNew)
                    {
                        requesterTypeDomainDataSource.DataView.Add(pd.NewRequesterType);

                        requesterTypeDomainDataSource.SubmitChanges();

                        requesterTypeDataGrid.UpdateLayout();
                    }
                    //If is editing of existing user
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(requesterTypeDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.RequesterType type = _context.RequesterTypes.Where(t => t.Id == pd.NewRequesterType.Id).First();
                        type.Name = pd.NewRequesterType.Name;
                        type.Description = pd.NewRequesterType.Description;

                        requesterTypeDomainDataSource.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void requesterTypeDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            
        }

        private void requesterTypeDomainDataSource_SubmittingChanges(object sender, SubmittingChangesEventArgs e)
        {
           
        }

    }
}
