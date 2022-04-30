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
    public partial class Priority : Page
    {
        public Priority()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            priorityDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Priority_Grid_Id;
            priorityDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Priority_Grid_Name;
            priorityDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Priority_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Priority_Button_Delete.Visibility = System.Windows.Visibility.Visible;
                Priority_Button_Edit.Visibility = System.Windows.Visibility.Visible;
                Priority_Button_New.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void priorityDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            this.priorityDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        /// <summary>
        /// Opens child window for inserting new priority
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(CSR_Property.Priority);
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

                if (csrDetails.NewPriority != null)
                {
                    //If is new priority
                    if (csrDetails.isNew)
                    {
                        priorityDomainDataSource.DataView.Add(csrDetails.NewPriority);

                        priorityDomainDataSource.SubmitChanges();
                    }
                    //If editing existing priority
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(priorityDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Priority type = _context.Priorities.Where(t => t.Id == csrDetails.NewPriority.Id).First();
                        type.PriorityName = csrDetails.NewPriority.PriorityName;
                        type.Description = csrDetails.NewPriority.Description;

                        priorityDomainDataSource.SubmitChanges();
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
        /// Opens child window for editing selected priority
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var prio = priorityDataGrid.SelectedItem;

                if (prio != null)
                {
                    ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(prio as BusinessSystemsApp.Web.Priority);

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
        /// Delete selected priority
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Helpers.MessageBoxDialog msg = new Helpers.MessageBoxDialog("Comfirm delete", "Are you sure you want delete selected priority?");
                msg.Closed += new EventHandler(msg_Closed);
                msg.Show();
               
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        void msg_Closed(object sender, EventArgs e)
        {
            try
            {
                if ((sender as Helpers.MessageBoxDialog).DialogResult == true)
                {

                    var prio = priorityDataGrid.SelectedItem;

                    priorityDomainDataSource.DataView.Remove(prio);

                    priorityDomainDataSource.SubmitChanges();
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
