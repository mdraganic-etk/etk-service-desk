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
    public partial class Status : Page
    {
        public Status()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            csr_StatusDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Status_Grid_Id;
            csr_StatusDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Status_Grid_Name;
            csr_StatusDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Status_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Status_Button_Delete.Visibility = System.Windows.Visibility.Visible;
                Status_Button_Edit.Visibility = System.Windows.Visibility.Visible;
                Status_Button_New.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void csr_StatusDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.csr_StatusDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        /// <summary>
        /// Open child window for inserting new CSR ststus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(CSR_Property.Status);
                csrDetails.Closed += new EventHandler(csrDetails_Closed);
                csrDetails.Show();
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
        void csrDetails_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.CSR_PropertyDetails csrDetails = (ChildForms.CSR_PropertyDetails)sender;

                if (csrDetails.NewStatus != null)
                {
                    //Is is new ststus
                    if (csrDetails.isNew)
                    {
                        csr_StatusDomainDataSource.DataView.Add(csrDetails.NewStatus);

                        csr_StatusDomainDataSource.SubmitChanges();
                    }
                    //If editing existing status
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(csr_StatusDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Csr_Status type = _context.Csr_Status.Where(t => t.Id == csrDetails.NewStatus.Id).First();
                        type.StatusName = csrDetails.NewStatus.StatusName;
                        type.Description = csrDetails.NewStatus.Description;

                        csr_StatusDomainDataSource.SubmitChanges();
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
        /// Open child window for edit selected CSR status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var status = csr_StatusDataGrid.SelectedItem;

                if (status != null)
                {
                    ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(status as BusinessSystemsApp.Web.Csr_Status);

                    csrDetails.Closed += new EventHandler(csrDetails_Closed);

                    csrDetails.Show();
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// Delete selected CSR ststus
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
                        var status = csr_StatusDataGrid.SelectedItem;

                        csr_StatusDomainDataSource.DataView.Remove(status);

                        csr_StatusDomainDataSource.SubmitChanges();
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
