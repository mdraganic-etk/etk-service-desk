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
using BusinessSystemsApp.Web;
using System.ServiceModel.DomainServices.Client;

namespace BusinessSystemsApp.ChildForms
{
    public partial class SiteDetails : ChildWindow
    {
        public Site NewSite { get; set; }

        public bool isNew = true;

        /// <summary>
        /// Initialize new window for add new site
        /// </summary>
        public SiteDetails()
        {
            InitializeComponent();

            NewSite = new Site();

            isNew = true;
        }

        /// <summary>
        /// Initialize new window for editing selected site
        /// </summary>
        /// <param name="type">Site instance for edit</param>
        public SiteDetails(Site type)
        {
            InitializeComponent();

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewSite = new Site();

            
            companyDomainDataSource.LayoutUpdated += new EventHandler(companyDomainDataSource_LayoutUpdated);
            

            NewSite.Id = type.Id;
            NewSite.SiteName = type.SiteName;
            NewSite.Description = type.Description;
            NewSite.CompanyId = type.CompanyId;

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.SiteName;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;
        }

        //When Layout is updated
        void companyDomainDataSource_LayoutUpdated(object sender, EventArgs e)
        {
            companyComboBox.SelectedValue = NewSite.CompanyId;

            companyDomainDataSource.LayoutUpdated -= new EventHandler(companyDomainDataSource_LayoutUpdated);
        }

      
        //when button OK is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {  
            NewSite.SiteName = nameTextBox.Text;
            NewSite.Description = descriptionTextBox.Text;
            NewSite.CompanyId = Convert.ToInt32(companyComboBox.SelectedValue);
            
            this.DialogResult = true;
        }


        //When Cancel button is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewSite = null;

            this.DialogResult = false;
        }

        //When companies data is loaded
        private void companyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
            else
            {
                try
                {

                    companyComboBox.SelectedValue = NewSite.CompanyId;
                }
                catch (Exception ex)
                {
                    Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                    msg.Show();
                }
            }
        }

        private void SiteDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.SiteDetails_Window_Title;
        }
    }
}

