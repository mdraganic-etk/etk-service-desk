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
    public partial class TypeOfRequesterDetails : ChildWindow
    {

        public RequesterType NewRequesterType { get; set; }

        public bool isNew = true;

        /// <summary>
        /// Opens new window for add new product
        /// </summary>
        public TypeOfRequesterDetails()
        {
            InitializeComponent();

            NewRequesterType = new RequesterType();

            isNew = true;
        }

        /// <summary>
        /// Opens new window fro editing selected product
        /// </summary>
        /// <param name="type">Product instance</param>
        public TypeOfRequesterDetails(RequesterType type)
        {
            InitializeComponent();

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewRequesterType = new RequesterType();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.Name;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;

            NewRequesterType.Id = type.Id;
        }

        //When button ok is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            NewRequesterType.Name = nameTextBox.Text;
            NewRequesterType.Description = descriptionTextBox.Text;

            this.DialogResult = true;
        }

        //When button cancel is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewRequesterType = null;

            this.DialogResult = false;
        }

        private void TypeOfRequesterDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.TypeOfRequesterDetails_Window_Title;
        }
    }
}

