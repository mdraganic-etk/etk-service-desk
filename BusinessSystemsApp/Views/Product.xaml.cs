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
    public partial class Product : Page
    {
        public Product()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Fill datagrid columns headers from resource depending on language
            productDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Product_Grid_Id;
            productDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Product_Grid_Name;
            productDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Product_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Product_Button_Delete.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void productDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            this.productDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        /// <summary>
        /// Opens child window for inserting new product
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.ProductDetails pd = new ChildForms.ProductDetails();
                pd.Closed += new EventHandler(pd_Closed);
                pd.Show();
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
        void pd_Closed(object sender, EventArgs e)
        {
            try
            {
                ChildForms.ProductDetails pd = (ChildForms.ProductDetails)sender;

                if (pd.NewProd != null)
                {
                    //If is new product
                    if (pd.isNew)
                    {
                        productDomainDataSource.DataView.Add(pd.NewProd);

                        productDomainDataSource.SubmitChanges();
                    }
                    //If is editing of existing user
                    else
                    {
                        BusinessSystemsDomainContext _context = (BusinessSystemsDomainContext)(productDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Product type = _context.Products.Where(t => t.Id == pd.NewProd.Id).First();
                        type.ProductName = pd.NewProd.ProductName;
                        type.Description = pd.NewProd.Description;

                        productDomainDataSource.SubmitChanges();
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
        /// Opens child window for editing selected product 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var prod = productDataGrid.SelectedItem;


                ChildForms.ProductDetails pd = new ChildForms.ProductDetails(prod as BusinessSystemsApp.Web.Product);

                pd.Closed += new EventHandler(pd_Closed);
                pd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        /// <summary>
        /// Delete selected product
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
                        var prod = productDataGrid.SelectedItem;

                        productDomainDataSource.DataView.Remove(prod);

                        productDomainDataSource.SubmitChanges();
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
