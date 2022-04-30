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
    public partial class ProductDetails : ChildWindow
    {

        public Product NewProd { get; set; }

        public bool isNew = true;

        /// <summary>
        /// Opens new window for add new product
        /// </summary>
        public ProductDetails()
        {
            InitializeComponent();

            NewProd = new Product();

            isNew = true;
        }

        /// <summary>
        /// Opens new window fro editing selected product
        /// </summary>
        /// <param name="type">Product instance</param>
        public ProductDetails(Product type)
        {
            InitializeComponent();

            idTextBox.IsReadOnly = true;

            isNew = false;

            NewProd = new Product();

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.ProductName;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;

            NewProd.Id = type.Id;
        }

        //When button ok is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            NewProd.ProductName = nameTextBox.Text;
            NewProd.Description = descriptionTextBox.Text;

            this.DialogResult = true;
        }

        //When button cancel is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NewProd = null;

            this.DialogResult = false;
        }

        private void ProductDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Get window title from resource file depending on language
            this.Title = BusinessSystemsApp.Resources.BS_Resources.ProductDetails_Window_Title;
        }
    }
}

