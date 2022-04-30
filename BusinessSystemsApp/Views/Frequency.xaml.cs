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
    public partial class Frequency : Page
    {
        public Frequency()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill grid column headers from resource depending on language
            frequencyDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Frequency_Grid_Id;
            frequencyDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Frequency_Grid_Name;
            frequencyDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Frequency_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Frequency_Button_Delete.Visibility = System.Windows.Visibility.Visible;
                Frequency_Button_Edit.Visibility = System.Windows.Visibility.Visible;
                Frequency_Button_New.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void frequencyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.frequencyDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        /// <summary>
        /// Opens child window for insert new frequency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(CSR_Property.Frequency);
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

                if (csrDetails.NewFrequency != null)
                {
                    //If is new frequency
                    if (csrDetails.isNew)
                    {
                        frequencyDomainDataSource.DataView.Add(csrDetails.NewFrequency);

                        frequencyDomainDataSource.SubmitChanges();
                    }
                    //If is editing of existing frequency    
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(frequencyDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Frequency type = _context.Frequencies.Where(t => t.Id == csrDetails.NewFrequency.Id).First();
                        type.FrequencyName = csrDetails.NewFrequency.FrequencyName;
                        type.Description = csrDetails.NewFrequency.Description;

                        frequencyDomainDataSource.SubmitChanges();
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
        /// Opens child window fro editing selected frequency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var freq = frequencyDataGrid.SelectedItem;


                if (freq != null)
                {
                    ChildForms.CSR_PropertyDetails csrDetails = new ChildForms.CSR_PropertyDetails(freq as BusinessSystemsApp.Web.Frequency);

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
        /// Delete selected frequency
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
                        var freq = frequencyDataGrid.SelectedItem;

                        frequencyDomainDataSource.DataView.Remove(freq);

                        frequencyDomainDataSource.SubmitChanges();
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
