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
    public partial class Site : Page
    {
        BusinessSystemsDomainContext _context;

        public Site()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            siteDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Site_Grid_Id;
            siteDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Site_Grid_Name;
            siteDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Site_Grid_Description;
            siteDataGrid.Columns[4].Header = BusinessSystemsApp.Resources.BS_Resources.Site_Grid_Company;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Site_Button_Delete.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void siteDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.siteDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Open child window for inserting new Site 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.SiteDetails sd = new ChildForms.SiteDetails();
                sd.Closed += new EventHandler(sd_Closed);
                sd.Show();
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
        void sd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.SiteDetails sd = (ChildForms.SiteDetails)sender;

                if (sd.NewSite != null)
                {
                    //If is new site entered
                    if (sd.isNew)
                    {
                        _context = (BusinessSystemsDomainContext)(siteDomainDataSource.DomainContext);
                        _context.Sites.Add(sd.NewSite);
                        siteDomainDataSource.SubmitChanges();

                        siteDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(siteDomainDataSource_SubmittedChanges);
                    }
                    //If editing existing site
                    else
                    {
                        _context = (BusinessSystemsDomainContext)(siteDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Site type = _context.Sites.Where(t => t.Id == sd.NewSite.Id).First();
                        type.SiteName = sd.NewSite.SiteName;
                        type.Description = sd.NewSite.Description;
                        type.CompanyId = sd.NewSite.CompanyId;


                        siteDomainDataSource.SubmitChanges();
                        siteDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(siteDomainDataSource_SubmittedChanges);

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
        /// When site data source is submitted load site list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void siteDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            try
            {
                siteDataGrid.ItemsSource = _context.Sites;
                _context.Load(_context.GetSiteQuery());

                siteDomainDataSource.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(siteDomainDataSource_SubmittedChanges);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// Opens child window for editing selected site
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var site = siteDataGrid.SelectedItem;

                ChildForms.SiteDetails sd = new ChildForms.SiteDetails(site as BusinessSystemsApp.Web.Site);

                sd.Closed += new EventHandler(sd_Closed);
                sd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        /// <summary>
        /// Delete selected site
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
                        var site = siteDataGrid.SelectedItem;

                        siteDomainDataSource.DataView.Remove(site);
                        siteDomainDataSource.SubmitChanges();
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
