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
    public partial class CommunicationChannel : Page
    {
        public CommunicationChannel()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Get grid labels from resource depending on language

            communicationChannelDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.CommunicationChannel_Grid_Column_Id;
            communicationChannelDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.CommunicationChannel_Grid_Column_Name;
            communicationChannelDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.CommunicationChannel_Grid_Column_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                CommunicationChannel_Button_Delete.Visibility = System.Windows.Visibility.Visible;
                CommunicationChannel_Button_Edit.Visibility = System.Windows.Visibility.Visible;
                CommunicationChannel_Button_New.Visibility = System.Windows.Visibility.Visible;
            }
            
        }

        private void communicationChannelDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.communicationChannelDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Opens child window for insert new communication channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.CommunicationChannelDetails ccd = new ChildForms.CommunicationChannelDetails();
                ccd.Closed += new EventHandler(ccd_Closed);
                ccd.Show();
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
        void ccd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.CommunicationChannelDetails ccd = (ChildForms.CommunicationChannelDetails)sender;

                if (ccd.NewComm != null)
                {
                    //If is new communication channel
                    if (ccd.isNew)
                    {
                        communicationChannelDomainDataSource.DataView.Add(ccd.NewComm);

                        communicationChannelDomainDataSource.SubmitChanges();
                    }
                    //If editing existing user
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(communicationChannelDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.CommunicationChannel type = _context.CommunicationChannels.Where(t => t.Id == ccd.NewComm.Id).First();
                        type.CommunicationChannelName = ccd.NewComm.CommunicationChannelName;
                        type.Description = ccd.NewComm.Description;

                        communicationChannelDomainDataSource.SubmitChanges();
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
        /// Opens child window for editing selected communication channel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var comm = communicationChannelDataGrid.SelectedItem;


                ChildForms.CommunicationChannelDetails ccd = new ChildForms.CommunicationChannelDetails(comm as BusinessSystemsApp.Web.CommunicationChannel);

                ccd.Closed += new EventHandler(ccd_Closed);
                ccd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        /// <summary>
        /// Delete selected communication channel
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
                        var comm = communicationChannelDataGrid.SelectedItem;

                        communicationChannelDomainDataSource.DataView.Remove(comm);

                        communicationChannelDomainDataSource.SubmitChanges();
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
