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
    public partial class IssueDomain : Page
    {
        public IssueDomain()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            issueDomainDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.IssueDomain_Grid_Id;
            issueDomainDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.IssueDomain_Grid_Name;
            issueDomainDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.IssueDomain_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
            btn_delete.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void issueDomainDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.issueDomainDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void btn_new_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.IssueDomainDetails pd = new ChildForms.IssueDomainDetails();
                pd.Closed += new EventHandler(pd_Closed);
                pd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var prod = issueDomainDataGrid.SelectedItem;

                ChildForms.IssueDomainDetails pd = new ChildForms.IssueDomainDetails(prod as BusinessSystemsApp.Web.IssueDomain);

                pd.Closed += new EventHandler(pd_Closed);
                pd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }

        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helpers.MessageBoxDialog msg = new Helpers.MessageBoxDialog(BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Title, BusinessSystemsApp.Resources.BS_Resources.Dialog_ConfirmDelete_Text);
                msg.Show();

                msg.Closed += (s2, e2) =>
                {
                    if (msg.DialogResult == true)
                    {
                        var prod = issueDomainDataGrid.SelectedItem;

                        issueDomainDomainDataSource.DataView.Remove(prod);

                        issueDomainDomainDataSource.SubmitChanges();
                    }
                };

            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        // <summary>
        /// Occures when child window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.IssueDomainDetails pd = (ChildForms.IssueDomainDetails)sender;

                if (pd.NewIssueDomain != null)
                {
                    //If is new product
                    if (pd.isNew)
                    {
                        issueDomainDomainDataSource.DataView.Add(pd.NewIssueDomain);

                        issueDomainDomainDataSource.SubmitChanges();

                        issueDomainDataGrid.UpdateLayout();
                    }
                    //If is editing of existing user
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(issueDomainDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.IssueDomain type = _context.IssueDomains.Where(t => t.Id == pd.NewIssueDomain.Id).First();
                        type.Name = pd.NewIssueDomain.Name;
                        type.Description = pd.NewIssueDomain.Description;

                        issueDomainDomainDataSource.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

    }
}
