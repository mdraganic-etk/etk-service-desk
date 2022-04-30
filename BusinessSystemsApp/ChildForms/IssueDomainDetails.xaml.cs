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
    public partial class IssueDomainDetails : ChildWindow
    {

        public IssueDomain NewIssueDomain { get; set; }

        public bool isNew = true;

        /// <summary>
        /// Opens new window for add new product
        /// </summary>
        public IssueDomainDetails()
        {
            InitializeComponent();

            NewIssueDomain = new IssueDomain();

            isNew = true;
        }
        
        /// <summary>
        /// Opens new window fro editing selected product
        /// </summary>
        /// <param name="type">Product instance</param>
        public IssueDomainDetails(IssueDomain type)
        {
            InitializeComponent();

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewIssueDomain = new IssueDomain();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.Name;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;

            NewIssueDomain.Id = type.Id;
        }

        //When button ok is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            NewIssueDomain.Name = nameTextBox.Text;
            NewIssueDomain.Description = descriptionTextBox.Text;

            this.DialogResult = true;
        }

        //When button cancel is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewIssueDomain = null;

            this.DialogResult = false;
        }

        private void IssueDomainDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.IssueDomainDetails_Window_Title;
        }
    }
}

