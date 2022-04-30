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
    public partial class Severity : Page
    {
        public Severity()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            severityDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Severity_Grid_Id;
            severityDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Severity_Grid_Name;
            severityDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Severity_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Severity_Button_Delete.Visibility = System.Windows.Visibility.Visible;
                Severity_Button_Edit.Visibility = System.Windows.Visibility.Visible;
                Severity_Button_New.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void severityDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.severityDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Opens child window for inserting new Severity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(CSR_Property.Severity);
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

                if (csrDetails.NewSeverity != null)
                {
                    //If is new severity
                    if (csrDetails.isNew)
                    {
                        severityDomainDataSource.DataView.Add(csrDetails.NewSeverity);

                        severityDomainDataSource.SubmitChanges();
                    }
                    //If editing existing severity
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(severityDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Severity type = _context.Severities.Where(t => t.Id == csrDetails.NewSeverity.Id).First();
                        type.SeverityName = csrDetails.NewSeverity.SeverityName;
                        type.Description = csrDetails.NewSeverity.Description;

                        severityDomainDataSource.SubmitChanges();
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
        /// Opens child window for editing selected severity
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var sev = severityDataGrid.SelectedItem;


                if (sev != null)
                {
                    ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(sev as BusinessSystemsApp.Web.Severity);

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
        /// Delete selected severity
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
                        var sev = severityDataGrid.SelectedItem;

                        severityDomainDataSource.DataView.Remove(sev);

                        severityDomainDataSource.SubmitChanges();
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
