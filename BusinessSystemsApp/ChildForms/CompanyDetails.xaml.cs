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
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace BusinessSystemsApp.ChildForms
{
    public partial class CompanyDetails : ChildWindow
    {
        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        public Company CurrentCompany {get; set;}

        //Lists with new data
        public ObservableCollection<ProductsInCompany> productList = new ObservableCollection<ProductsInCompany>();
        public ObservableCollection<CompanyStatuses> statusList = new ObservableCollection<CompanyStatuses>();
        public ObservableCollection<CompanyPriorities> priorityList = new ObservableCollection<CompanyPriorities>();
        public ObservableCollection<CompaniesAssignment> companyList = new ObservableCollection<CompaniesAssignment>();
        public ObservableCollection<UserNotifications> userList = new ObservableCollection<UserNotifications>();
        public ObservableCollection<RequesterTypeInCompany> typeOfRequesterList = new ObservableCollection<RequesterTypeInCompany>();
        public ObservableCollection<IssueDomainInCompany> issueDomainList = new ObservableCollection<IssueDomainInCompany>();

        //Lists with data loaded from database
        public ObservableCollection<ProductsInCompany> productListDB = null;
        public ObservableCollection<CompanyStatuses> statusListDB = null;
        public ObservableCollection<CompanyPriorities> priorityListDB = null;
        public ObservableCollection<CompaniesAssignment> companyListDB = null;
        public ObservableCollection<UserNotifications> userListDB = null;
        public ObservableCollection<RequesterTypeInCompany> typeOfRequesterListDB = null;
        public ObservableCollection<IssueDomainInCompany> issueDomainListDB = null;

        //Define load operations
        LoadOperation<ProductsInCompany> loProducts;
        LoadOperation<CompanyStatuses> loStatuses;
        LoadOperation<CompanyPriorities> loPriorities;
        LoadOperation<CompaniesAssignment> loCompanies;
        LoadOperation<UserNotifications> loUsers;
        LoadOperation<RequesterTypeInCompany> loTypeOfRequester;
        LoadOperation<IssueDomainInCompany> loIssueDomain;

        public bool isNew = true;

        Helpers.HelperClasses.Util helper = new Helpers.HelperClasses.Util();

        /// <summary>
        /// New Company form initialization
        /// </summary>
        public CompanyDetails()
        {
            InitializeComponent();

            CurrentCompany = new Company();

            isNew = true;

            this.productsInCompanyDataGrid.ItemsSource = productList;
            this.companiesAssignmentDataGrid.ItemsSource = companyList;
            this.companyStatusesDataGrid.ItemsSource = statusList;
            this.companyPrioritiesDataGrid.ItemsSource = priorityList;
            this.userNotificationsDataGrid.ItemsSource = userList;
            this.requesterTypeInCompanyDataGrid.ItemsSource = typeOfRequesterList;
            this.issueDomainInCompanyDataGrid.ItemsSource = issueDomainList;
        }

        /// <summary>
        /// Edit company form initialization
        /// </summary>
        /// <param name="comp"></param>
        public CompanyDetails(Company type)
        {
            
            InitializeComponent();
          
            loProducts = _context.Load(_context.GetProductsInCompanyQuery(type.Id));
            loProducts.Completed += new EventHandler(loProducts_Completed);
            
           
            loStatuses = _context.Load(_context.GetCompanyStatusesQuery(type.Id));
            loStatuses.Completed += new EventHandler(loStatuses_Completed);
            

            loPriorities = _context.Load(_context.GetCompanyPrioritiesQuery(type.Id));
            loPriorities.Completed += new EventHandler(loPriorities_Completed);
           

            loCompanies = _context.Load(_context.GetCompaniesAssignmentQuery(type.Id));
            loCompanies.Completed += new EventHandler(loCompanies_Completed);

            
            loUsers = _context.Load(_context.GetUserNotificationsQuery(type.Id));
            loUsers.Completed += new EventHandler(loUsers_Completed);

            loTypeOfRequester = _context.Load(_context.GetRequesterTypeInCompanyQuery(type.Id));
            loTypeOfRequester.Completed += new EventHandler(loTypeOfRequester_Completed);

            loIssueDomain = _context.Load(_context.GetIssueDomainInCompanyQuery(type.Id));
            loIssueDomain.Completed += new EventHandler(loIssueDomain_Completed);
          
            idTextBox.IsReadOnly = true;

            isNew = false;

            CurrentCompany = new Company();

            CurrentCompany.Id = type.Id;
            CurrentCompany.CompanyName = type.CompanyName;
            CurrentCompany.Description = type.Description;
            

            idTextBox.Text = type.Id.ToString();
            nameTextBox.Text = type.CompanyName;
            if (type.Description != null)
                descriptionTextBox.Text = type.Description;

            
            
          

        }

        //When loading user list is completed
        void loUsers_Completed(object sender, EventArgs e)
        {
            
            try
            {
                userList = new ObservableCollection<UserNotifications>(loUsers.Entities);

                userListDB = new ObservableCollection<UserNotifications>(loUsers.Entities);

                this.userNotificationsDataGrid.ItemsSource = userList;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When loading companies list is completed
        void loCompanies_Completed(object sender, EventArgs e)
        {
            try
            {
                companyList = new ObservableCollection<CompaniesAssignment>(loCompanies.Entities);

                companyListDB = new ObservableCollection<CompaniesAssignment>(loCompanies.Entities);

                this.companiesAssignmentDataGrid.ItemsSource = companyList;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When loading priorities list is completed
        void loPriorities_Completed(object sender, EventArgs e)
        {
            try
            {
                priorityList = new ObservableCollection<CompanyPriorities>(loPriorities.Entities);

                priorityListDB = new ObservableCollection<CompanyPriorities>(loPriorities.Entities);

                this.companyPrioritiesDataGrid.ItemsSource = priorityList;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }
        
        //When loading statuses list is completed
        void loStatuses_Completed(object sender, EventArgs e)
        {
            try
            {
                statusList = new ObservableCollection<CompanyStatuses>(loStatuses.Entities);

                statusListDB = new ObservableCollection<CompanyStatuses>(loStatuses.Entities);

                this.companyStatusesDataGrid.ItemsSource = statusList;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When loading products list is completed 
        void loProducts_Completed(object sender, EventArgs e)
        {
            try
            {
                productList = new ObservableCollection<ProductsInCompany>(loProducts.Entities);

                productListDB = new ObservableCollection<ProductsInCompany>(loProducts.Entities);

                this.productsInCompanyDataGrid.ItemsSource = productList;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When loading type of requesterslist is completed
        void loTypeOfRequester_Completed(object sender, EventArgs e)
        {
            try
            {
                typeOfRequesterList = new ObservableCollection<RequesterTypeInCompany>(loTypeOfRequester.Entities);

                typeOfRequesterListDB = new ObservableCollection<RequesterTypeInCompany>(loTypeOfRequester.Entities);

                this.requesterTypeInCompanyDataGrid.ItemsSource = typeOfRequesterList;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When loading type of issueDomainList is completed
        void loIssueDomain_Completed(object sender, EventArgs e)
        {
            try
            {
                issueDomainList = new ObservableCollection<IssueDomainInCompany>(loIssueDomain.Entities);

                issueDomainListDB = new ObservableCollection<IssueDomainInCompany>(loIssueDomain.Entities);

                this.issueDomainInCompanyDataGrid.ItemsSource = issueDomainList;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        //When OK button is selected
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentCompany.CompanyName = nameTextBox.Text;
                CurrentCompany.Description = descriptionTextBox.Text;

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When canacel button is selected
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CurrentCompany = null;

            this.DialogResult = false;
        }

        //When company statuses are loaded
        private void companyStatusesDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //When all csr statuses are loaded
        private void csr_StatusDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //When all priorities are loaded
        private void priorityDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //When all products are loaded
        private void productDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //When companies are loaded
        private void companyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        //When CSR statuses are loaded
        private void csr_StatusDomainDataSource1_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //When company products are loaded
        private void productsInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }

        //When assigned companies are loaded
        private void companiesAssignmentDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        //When Product add button is selected add Product to list
        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProductsInCompany cproduct = new ProductsInCompany();
                Product prod = new Product();

                cproduct.CompanyId = CurrentCompany.Id;
                cproduct.ProductId = Convert.ToInt32(productComboBox.SelectedValue);

                prod.Id = cproduct.ProductId;
                prod.ProductName = (productComboBox.SelectedItem as Product).ProductName.ToString();

                cproduct.Product = prod;

                if (!helper.ExistsInList(productList, cproduct))
                {
                    productList.Add(cproduct);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        //When Priority remove button is selected remove Priority from list
        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                productList.Remove(productsInCompanyDataGrid.SelectedItem as ProductsInCompany);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        //When Status add button is selected add Status to list
        private void btnAddStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompanyStatuses cstatus = new CompanyStatuses();
                Csr_Status status = new Csr_Status();

                cstatus.CompanyId = CurrentCompany.Id;
                cstatus.StatusId = Convert.ToInt32(csr_StatusComboBox.SelectedValue);

                status.Id = cstatus.StatusId;
                status.StatusName = (csr_StatusComboBox.SelectedItem as Csr_Status).StatusName.ToString();

                cstatus.Csr_Status = status;

                if (!helper.ExistsInList(statusList, cstatus))
                {
                    statusList.Add(cstatus);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        //When Priority add button is selected add Priority to list
        private void btnAddPriority_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompanyPriorities cpriority = new CompanyPriorities();
                Priority prio = new Priority();

                cpriority.CompanyId = CurrentCompany.Id;
                cpriority.PriorityId = Convert.ToInt32(priorityComboBox.SelectedValue);

                prio.Id = cpriority.PriorityId;
                prio.PriorityName = (priorityComboBox.SelectedItem as Priority).PriorityName.ToString();

                cpriority.Priority = prio;

                if (!helper.ExistsInList(priorityList, cpriority))
                {
                    priorityList.Add(cpriority);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
           
        }

        ////When Company add button is selected add company to list
        private void btnAddCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CompaniesAssignment ccompany = new CompaniesAssignment();
                Company comp = new Company();

                ccompany.CompanyId = CurrentCompany.Id;
                ccompany.AssignedCommpanyId = Convert.ToInt32(companyComboBox.SelectedValue);

                comp.Id = ccompany.AssignedCommpanyId;
                comp.CompanyName = (companyComboBox.SelectedItem as Company).CompanyName.ToString();

                ccompany.Company1 = comp;

                if (!helper.ExistsInList(companyList, ccompany))
                {
                    companyList.Add(ccompany);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }

        }


        //When Status remove button is selected remove status from list
        private void btnRemoveStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                statusList.Remove(companyStatusesDataGrid.SelectedItem as CompanyStatuses);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        //When priority remove button is selected remove priority from list
        private void btnRemovePriority_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                priorityList.Remove(companyPrioritiesDataGrid.SelectedItem as CompanyPriorities);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }


        //When Company remove button is selected remove company from list
        private void btnRemoveCompany_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                companyList.Remove(companiesAssignmentDataGrid.SelectedItem as CompaniesAssignment);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When all users are loaded
        private void userDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        //When notification users are loaded
        private void userNotificationsDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {
            if (e.HasError)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.ToString(), e.Error.ToString());
                msg.Show();
                e.MarkErrorAsHandled();
            }
        }


        //When Add button for user is selected add user to list
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                UserNotifications cuser = new UserNotifications();
                User user = new User();

                cuser.CompanyId = CurrentCompany.Id;
                cuser.UserId = Convert.ToInt32(userComboBox.SelectedValue);

                user.Id = cuser.UserId;
                user.FirstName = (userComboBox.SelectedItem as User).FirstName.ToString();
                user.LastName = (userComboBox.SelectedItem as User).LastName.ToString();

                cuser.User = user;

                if (!helper.ExistsInList(userList, cuser))
                {
                    userList.Add(cuser);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        //When remove button for user is selected remove user from list
        private void btnRemoveUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                userList.Remove(userNotificationsDataGrid.SelectedItem as UserNotifications);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void CompanyDetails_Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Window_Title;

            //Fill grids column headers with texts in resource file depending on language
            productsInCompanyDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Products_Grid_Id;
            productsInCompanyDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Products_Grid_Name;

            companyStatusesDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Statuses_Grid_Id;
            companyStatusesDataGrid.Columns[3].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Statuses_Grid_Name;

            companyPrioritiesDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Priorities_Grid_Id;
            companyPrioritiesDataGrid.Columns[3].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Priorities_Grid_Name;

            companiesAssignmentDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Companies_Grid_Id;
            companiesAssignmentDataGrid.Columns[1].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Companies_Grid_Name;

            userNotificationsDataGrid.Columns[0].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Users_Grid_Id;
            userNotificationsDataGrid.Columns[2].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Users_Grid_FirstName;
            userNotificationsDataGrid.Columns[3].Header = BusinessSystemsApp.Resources.BS_Resources.CompanyDetails_Users_Grid_LastName;
        }

        private void requesterTypeDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void issueDomainDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }

        private void requesterTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CompanyDetails_TypeOfRequester_Button_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RequesterTypeInCompany cTypeOfRequester = new RequesterTypeInCompany();
                RequesterType reqType = new RequesterType();

                cTypeOfRequester.CompanyId = CurrentCompany.Id;
                cTypeOfRequester.RequesterTypeId = Convert.ToInt32(requesterTypeComboBox.SelectedValue);

                reqType.Id = cTypeOfRequester.RequesterTypeId;
                reqType.Name = (requesterTypeComboBox.SelectedItem as RequesterType).Name.ToString();

                cTypeOfRequester.RequesterType = reqType;

                if (!helper.ExistsInList(typeOfRequesterList, cTypeOfRequester))
                {
                    typeOfRequesterList.Add(cTypeOfRequester);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void CompanyDetails_TypeOfRequester_Button_Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                typeOfRequesterList.Remove(requesterTypeInCompanyDataGrid.SelectedItem as RequesterTypeInCompany);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void CompanyDetails_IssueDomain_Button_Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IssueDomainInCompany cIssueDomain = new IssueDomainInCompany();
                IssueDomain issueDomain = new IssueDomain();

                cIssueDomain.CompanyId = CurrentCompany.Id;
                cIssueDomain.IssueDomainId = Convert.ToInt32(issueDomainComboBox.SelectedValue);

                issueDomain.Id = cIssueDomain.IssueDomainId;
                issueDomain.Name = (issueDomainComboBox.SelectedItem as IssueDomain).Name.ToString();

                cIssueDomain.IssueDomain = issueDomain;

                if (!helper.ExistsInList(issueDomainList, cIssueDomain))
                {
                    issueDomainList.Add(cIssueDomain);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
            }
        }

        private void CompanyDetails_IssueDomain_Button_Remove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                issueDomainList.Remove(issueDomainInCompanyDataGrid.SelectedItem as IssueDomainInCompany);
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message, ex.ToString());
                msg.Show();
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

        private void issueDomainInCompanyDomainDataSource_LoadedData(object sender, LoadedDataEventArgs e)
        {

            if (e.HasError)
            {
                System.Windows.MessageBox.Show(e.Error.ToString(), "Load Error", System.Windows.MessageBoxButton.OK);
                e.MarkErrorAsHandled();
            }
        }
    }
}

