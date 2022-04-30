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
using System.Collections.ObjectModel;
using BusinessSystemsApp.Web;
using System.ServiceModel.DomainServices.Client;



namespace BusinessSystemsApp.Views
{
    public partial class Company : Page
    {
        BusinessSystemsDomainContext _context ;
        
        ChildForms.CompanyDetails cd = null;

        Helpers.HelperClasses.Util helper = new Helpers.HelperClasses.Util();

        public Company()
        {
            InitializeComponent();

            _context = (BusinessSystemsDomainContext)(companyDomainDataSource.DomainContext);
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Get grid labels from resource depending on language
            companyDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.Company_Grid_Id;
            companyDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.Company_Grid_Name;
            companyDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.Company_Grid_Description;

            if (WebContext.Current.User.IsInRole("Super Admin"))
            {
                Company_Button_Delete.Visibility = System.Windows.Visibility.Visible;
            }
        }

        //When companies are loaded
        private void companyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            this.companyDataGrid.MaxHeight = LayoutRoot.ActualHeight;

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Opens new child window for inserting new company
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChildForms.CompanyDetails cd = new ChildForms.CompanyDetails();
                cd.Closed += new EventHandler(cd_Closed);
                cd.Show();
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
        void cd_Closed(object sender, EventArgs e)
        {
            try
            {
                cd = (ChildForms.CompanyDetails)sender;

                if (cd.CurrentCompany != null)
                {
                    if (cd.isNew)
                    {
                        _context = (BusinessSystemsDomainContext)(companyDomainDataSource.DomainContext);
                        _context.Companies.Add(cd.CurrentCompany);
                        companyDomainDataSource.SubmitChanges();

                        companyDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(companyDomainDataSource_SubmittedChanges);
                    }
                    else
                    {
                        _context = (BusinessSystemsDomainContext)(companyDomainDataSource.DomainContext);

                        BusinessSystemsApp.Web.Company type = _context.Companies.Where(t => t.Id == cd.CurrentCompany.Id).First();
                        type.CompanyName = cd.CurrentCompany.CompanyName;
                        type.Description = cd.CurrentCompany.Description;

                        companyDomainDataSource.SubmitChanges();

                        companyDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(companyDomainDataSource_SubmittedChanges);
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
        /// When company data source is submitted load new list and add company related data to database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void companyDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
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
                        companyDataGrid.ItemsSource = _context.Companies;
                        _context.Load(_context.GetCompanyQuery());

                        InsertCompanyDetails();
                    }
                    catch (Exception ex)
                    {
                        Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                        msg.Show();
                    }
                }
                companyDomainDataSource.SubmittedChanges -= new EventHandler<SubmittedChangesEventArgs>(companyDomainDataSource_SubmittedChanges);
         
        }


        /// <summary>
        /// Check inserted company customization (statuses, priorities, assigned companies, products )
        /// </summary>
        void InsertCompanyDetails()
        {
            try
            {
                int id;

                

                _context = (BusinessSystemsDomainContext)(productsInCompanyDomainDataSource.DomainContext);


                //Check product list and insert added items
                foreach (ProductsInCompany item in cd.productList)
                {
                    if (!helper.ExistsInList(cd.productListDB, item))
                    {
                        id = item.ProductId;
                        ProductsInCompany cproduct = item;
                        cproduct.Product = null;
                        cproduct.ProductId = id;
                        if (cproduct.CompanyId == 0)
                            cproduct.CompanyId = cd.CurrentCompany.Id;
                        _context.ProductsInCompanies.Add(cproduct);
                    }
                }

                if (cd.productListDB != null)
                {
                    //Check for deleted items from product list
                    foreach (ProductsInCompany item in cd.productListDB)
                    {
                        if (!helper.ExistsInList(cd.productList, item))
                        {

                            ProductsInCompany product = _context.ProductsInCompanies.Where(p => p.CompanyId == item.CompanyId && p.ProductId == item.ProductId).First();

                            _context.ProductsInCompanies.Remove(product);
                        }
                    }
                }

                productsInCompanyDomainDataSource.SubmitChanges();



                _context = (BusinessSystemsDomainContext)(companyStatusesDomainDataSource.DomainContext);
                //Check statuses list and insert added items
                foreach (CompanyStatuses item in cd.statusList)
                {
                    if (!helper.ExistsInList(cd.statusListDB, item))
                    {
                        id = item.StatusId;
                        CompanyStatuses cstatus = item;
                        cstatus.Csr_Status = null;
                        cstatus.StatusId = id;
                        if (cstatus.CompanyId == 0)
                            cstatus.CompanyId = cd.CurrentCompany.Id;
                        _context.CompanyStatuses.Add(cstatus);
                    }
                }

                if (cd.statusListDB != null)
                {
                    //Check for deleted items from product list
                    foreach (CompanyStatuses item in cd.statusListDB)
                    {
                        if (!helper.ExistsInList(cd.statusList, item))
                        {

                            CompanyStatuses status = _context.CompanyStatuses.Where(p => p.CompanyId == item.CompanyId && p.StatusId == item.StatusId).First();

                            _context.CompanyStatuses.Remove(status);
                        }
                    }
                }

                companyStatusesDomainDataSource.SubmitChanges();



                _context = (BusinessSystemsDomainContext)(companyPrioritiesDomainDataSource.DomainContext);
                //Check priorities list and insert added items  
                foreach (CompanyPriorities item in cd.priorityList)
                {
                    if (!helper.ExistsInList(cd.priorityListDB, item))
                    {
                        id = item.PriorityId;
                        CompanyPriorities cpriority = item;
                        cpriority.Priority = null;
                        cpriority.PriorityId = id;
                        if (cpriority.CompanyId == 0)
                            cpriority.CompanyId = cd.CurrentCompany.Id;
                        _context.CompanyPriorities.Add(cpriority);
                    }
                }


                if (cd.priorityListDB != null)
                {
                    //Check for deleted items from product list
                    foreach (CompanyPriorities item in cd.priorityListDB)
                    {
                        if (!helper.ExistsInList(cd.priorityList, item))
                        {

                            CompanyPriorities priority = _context.CompanyPriorities.Where(p => p.CompanyId == item.CompanyId && p.PriorityId == item.PriorityId).First();

                            _context.CompanyPriorities.Remove(priority);
                        }
                    }
                }

                companyPrioritiesDomainDataSource.SubmitChanges();


                _context = (BusinessSystemsDomainContext)(companiesAssignmentDomainDataSource.DomainContext);
                //Check list of assigned companies and insert added items
                foreach (CompaniesAssignment item in cd.companyList)
                {
                    if (!helper.ExistsInList(cd.companyListDB, item))
                    {
                        id = item.AssignedCommpanyId;
                        CompaniesAssignment ccompany = item;
                        ccompany.Company1 = null;
                        ccompany.Company = null;
                        ccompany.AssignedCommpanyId = id;
                        if (ccompany.CompanyId == 0)
                            ccompany.CompanyId = cd.CurrentCompany.Id;
                        _context.CompaniesAssignments.Add(ccompany);
                    }
                }

                if (cd.companyListDB != null)
                {
                    //Check for deleted items from product list
                    foreach (CompaniesAssignment item in cd.companyListDB)
                    {
                        if (!helper.ExistsInList(cd.companyList, item))
                        {

                            CompaniesAssignment company = _context.CompaniesAssignments.Where(p => p.CompanyId == item.CompanyId && p.AssignedCommpanyId == item.AssignedCommpanyId).First();

                            _context.CompaniesAssignments.Remove(company);
                        }
                    }
                }

                companiesAssignmentDomainDataSource.SubmitChanges();

                companiesAssignmentDomainDataSource.SubmittedChanges += new EventHandler<SubmittedChangesEventArgs>(companiesAssignmentDomainDataSource_SubmittedChanges);


                _context = (BusinessSystemsDomainContext)(userNotificationsDomainDataSource.DomainContext);
                //Check users to notify list and insert added items
                foreach (UserNotifications item in cd.userList)
                {
                    if (!helper.ExistsInList(cd.userListDB, item))
                    {
                        id = item.UserId;
                        UserNotifications cuser = item;
                        cuser.User = null;
                        cuser.UserId = id;
                        if (cuser.CompanyId == 0)
                            cuser.CompanyId = cd.CurrentCompany.Id;
                        _context.UserNotifications.Add(cuser);
                    }
                }


                if (cd.userListDB != null)
                {
                    //Check for deleted items from product list
                    foreach (UserNotifications item in cd.userListDB)
                    {
                        if (!helper.ExistsInList(cd.userList, item))
                        {

                            UserNotifications user = _context.UserNotifications.Where(p => p.CompanyId == item.CompanyId && p.UserId == item.UserId).First();

                            _context.UserNotifications.Remove(user);
                        }
                    }
                }

                userNotificationsDomainDataSource.SubmitChanges();



                _context = (BusinessSystemsDomainContext)(requesterTypeInCompanyDomainDataSource.DomainContext);


                //Check requester type and insert added items
                foreach (RequesterTypeInCompany item in cd.typeOfRequesterList)
                {
                    if (!helper.ExistsInList(cd.typeOfRequesterListDB, item))
                    {
                        id = item.RequesterTypeId;
                        RequesterTypeInCompany cproduct = item;
                        cproduct.RequesterType = null;
                        cproduct.RequesterTypeId = id;
                        if (cproduct.CompanyId == 0)
                            cproduct.CompanyId = cd.CurrentCompany.Id;
                        _context.RequesterTypeInCompanies.Add(cproduct);
                    }
                }

                if (cd.typeOfRequesterListDB != null)
                {
                    //Check for deleted items from product list
                    foreach (RequesterTypeInCompany item in cd.typeOfRequesterListDB)
                    {
                        if (!helper.ExistsInList(cd.typeOfRequesterList, item))
                        {

                            RequesterTypeInCompany product = _context.RequesterTypeInCompanies.Where(p => p.CompanyId == item.CompanyId && p.RequesterTypeId == item.RequesterTypeId).First();

                            _context.RequesterTypeInCompanies.Remove(product);
                        }
                    }
                }

                requesterTypeInCompanyDomainDataSource.SubmitChanges();


                _context = (BusinessSystemsDomainContext)(issueDomainInCompanyDomainDataSource.DomainContext);


                //Check requester type and insert added items
                foreach (IssueDomainInCompany item in cd.issueDomainList)
                {
                    if (!helper.ExistsInList(cd.issueDomainListDB, item))
                    {
                        id = item.IssueDomainId;
                        IssueDomainInCompany cproduct = item;
                        cproduct.IssueDomain = null;
                        cproduct.IssueDomainId = id;
                        if (cproduct.CompanyId == 0)
                            cproduct.CompanyId = cd.CurrentCompany.Id;
                        _context.IssueDomainInCompanies.Add(cproduct);
                    }
                }

                if (cd.issueDomainListDB != null)
                {
                    //Check for deleted items from product list
                    foreach (IssueDomainInCompany item in cd.issueDomainListDB)
                    {
                        if (!helper.ExistsInList(cd.issueDomainList, item))
                        {

                            IssueDomainInCompany product = _context.IssueDomainInCompanies.Where(p => p.CompanyId == item.CompanyId && p.IssueDomainId == item.IssueDomainId).First();

                            _context.IssueDomainInCompanies.Remove(product);
                        }
                    }
                }

                issueDomainInCompanyDomainDataSource.SubmitChanges();

            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            } 
        }

        void companiesAssignmentDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        void productsInCompanyDomainDataSource_SubmittedChanges(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Opens child window for editing selected existing company
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var comp = companyDataGrid.SelectedItem;

                //Load products in companies to to data source for later comparision and delete
                productsInCompanyDomainDataSource.QueryParameters.Clear();
                
                Parameter par = new Parameter();
                par.ParameterName = "_companyId";
                par.Value = (comp as Web.Company).Id;

                productsInCompanyDomainDataSource.QueryParameters.Add(par);

                productsInCompanyDomainDataSource.Load();
                ///////////

                //Load company statuses to to data source for later comparision and delete
                companyStatusesDomainDataSource.QueryParameters.Clear();

                Parameter par1 = new Parameter();
                par1.ParameterName = "_companyId";
                par1.Value = (comp as Web.Company).Id;

                companyStatusesDomainDataSource.QueryParameters.Add(par1);

                companyStatusesDomainDataSource.Load();
                ///////////

                //Load company priorities to data source for later comparision and delete
                companyPrioritiesDomainDataSource.QueryParameters.Clear();

                Parameter par2 = new Parameter();
                par2.ParameterName = "_companyId";
                par2.Value = (comp as Web.Company).Id;

                companyPrioritiesDomainDataSource.QueryParameters.Add(par2);

                companyPrioritiesDomainDataSource.Load();
                ///////////

                //Load user notifications for company to data source for later comparision and delete
                userNotificationsDomainDataSource.QueryParameters.Clear();

                Parameter par3 = new Parameter();
                par3.ParameterName = "_companyId";
                par3.Value = (comp as Web.Company).Id;

                userNotificationsDomainDataSource.QueryParameters.Add(par3);

                userNotificationsDomainDataSource.Load();
                ///////////

                //Load companies for company to data source for later comparision and delete
                companiesAssignmentDomainDataSource.QueryParameters.Clear();

                Parameter par4 = new Parameter();
                par4.ParameterName = "_companyId";
                par4.Value = (comp as Web.Company).Id;

                companiesAssignmentDomainDataSource.QueryParameters.Add(par4);

                companiesAssignmentDomainDataSource.Load();
                ///////////

                ChildForms.CompanyDetails cd = new ChildForms.CompanyDetails(comp as BusinessSystemsApp.Web.Company);

                cd.Closed += new EventHandler(cd_Closed);
                cd.Show();
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        /// <summary>
        /// Delete selected company from list
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
                        var comp = companyDataGrid.SelectedItem;


                        companyDomainDataSource.DataView.Remove(comp);
                        companyDomainDataSource.SubmitChanges();
                    }
                };
               
                
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


       

        private void companyStatusesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void companyPrioritiesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void productsInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void companiesAssignmentDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void userNotificationsDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        private void companyDomainDataSource_SubmittedChanges_1(object sender, SubmittedChangesEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.Error.ToString(), e.Error.Message);
                msg.Show();
                e.MarkErrorAsHandled();

                this.NavigationService.Refresh();
            }
        }

        private void issueDomainInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void requesterTypeInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
    
    }
}
