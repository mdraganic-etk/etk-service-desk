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
    public partial class UserTypeDetails : ChildWindow
    {
       
        public UserType NewUser { get; set; }

        public bool isNew = true;


        /// <summary>
        /// Initialize new window for add new user type
        /// </summary>
        public UserTypeDetails()
        {
            InitializeComponent();

            NewUser = new UserType();

            isNew = true;
        }

        /// <summary>
        /// Initialize new window for editing selected user type
        /// </summary>
        /// <param name="type"></param>
        public UserTypeDetails(UserType type)
        {
            InitializeComponent();

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewUser = new UserType();

            idTextBox.Text = type.Id.ToString();
            typeNameTextBox.Text = type.TypeName;
            if(type.Description != null)
                descriptionTextBox.Text = type.Description;
        }

        //When OK button is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
          
            NewUser.Id = Convert.ToInt32(idTextBox.Text);
            NewUser.TypeName = typeNameTextBox.Text;
            NewUser.Description = descriptionTextBox.Text;

            this.DialogResult = true;
        }

        //When Cancel button selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewUser = null;
            this.DialogResult = false;
        }

        //When user type is loaded
        private void userTypeDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void UserTypeDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.UserTypeDetails_Window_Title;
        }
    }
}

