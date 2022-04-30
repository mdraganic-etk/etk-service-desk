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
using System.ServiceModel.DomainServices.Client.ApplicationServices;
using System.Threading;

namespace BusinessSystemsApp.ChildForms
{
    public partial class Login : ChildWindow
    {
        BusinessSystemsDomainContext _context = new BusinessSystemsDomainContext();

        private delegate void ShowErrorDelegate(String text);

        LoadOperation lo = null;

        public Login()
        {
            InitializeComponent();

            UserNameTextBox.Focus();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            LogUser();
        }

        //Trys to log on user with entered username and password
        private void LogUser()
        {
            try
            {
                Login_Button_Login.IsEnabled = false;

                WarningGrid.Visibility = System.Windows.Visibility.Collapsed;

                this.Height = 263;

                Helpers.HelperClasses.Util util = new Helpers.HelperClasses.Util();

                string hashedPassword = util.HashPassword(passwordBox.Password);


                LoginOperation loginOp = WebContext.Current.Authentication.Login(new LoginParameters(UserNameTextBox.Text, hashedPassword));
          

                busyIndicator1.Visibility = System.Windows.Visibility.Visible;
                busyIndicator1.IsBusy = true;

                //When login operation is completed
                loginOp.Completed += (s2, e2) =>
                {
                    if (loginOp.HasError)
                    {
                        loginOp.MarkErrorAsHandled();

                        Helpers.MessageBox msg = new Helpers.MessageBox("Error", "Greška", loginOp.Error.ToString());
                        msg.Show();

                        this.Dispatcher.BeginInvoke(HideBusy);

                        this.Dispatcher.BeginInvoke(new ShowErrorDelegate(ShowError), "Username or password is incorrect.  Please try again. If you are unauthorized user please exit immediately!");

                        Login_Button_Login.IsEnabled = true;

                        return;
                    }
                    else if (!loginOp.LoginSuccess)
                    {
                        this.Dispatcher.BeginInvoke(HideBusy);

                        this.Dispatcher.BeginInvoke(new ShowErrorDelegate(ShowError), "Username or password is incorrect.  Please try again. If you are unauthorized user please exit immediately!");

                        Login_Button_Login.IsEnabled = true;

                        return;
                    }
                    else
                    {
                        this.Dispatcher.BeginInvoke(HideBusy);

                        lo = _context.Load(_context.GetUserWithUsernameQuery(UserNameTextBox.Text));
                        lo.Completed += new EventHandler(lo_Completed);

                    }
                };
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
         
        }


       
        void lo_Completed(object sender, EventArgs e)
        {
            try
            {
                if ((lo.Entities.ElementAt(0) as User).UserType.Id == 6) 
                {
                    
                    this.Dispatcher.BeginInvoke(HideBusy);

                    this.Dispatcher.BeginInvoke(new ShowErrorDelegate(ShowError),"Your are inactive user and you are not allowed to access this apliaction. Please contact administrator!");

                    Login_Button_Login.IsEnabled = true;

                    passwordBox.Password = String.Empty;

                    return;
                }
                else if ((lo.Entities.ElementAt(0) as User).UserType.Id == 1)
                {
                    this.Dispatcher.BeginInvoke(HideBusy);

                    this.Dispatcher.BeginInvoke(new ShowErrorDelegate(ShowError),"You are not authorized to access this application because you are defined as \"Caller\"! Please contact administrator!");

                    Login_Button_Login.IsEnabled = true;

                    passwordBox.Password = String.Empty;

                    return;
                }
                else
                {
                    UserLogging userLogging = new UserLogging();
                    userLogging.LogInTime = DateTime.Now;
                    userLogging.UserId = (lo.Entities.ElementAt(0) as User).Id;

                    _context.UserLoggings.Add(userLogging);
                    _context.SubmitChanges(OnSubmitChanges, null);
                }
            }
            catch (Exception ex)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", ex.Message.ToString(), ex.ToString());
                msg.Show();
            }
        }

        private void OnSubmitChanges(SubmitOperation so)
        {
            this.DialogResult = true;
        }

        //Hides loading bar
        private void HideBusy()
        {
            busyIndicator1.Visibility = System.Windows.Visibility.Collapsed;
            busyIndicator1.IsBusy = false;
        }

        //Shows error text
        private void ShowError(String text)
        {
            this.Login_TextBlock_Failed.Text = text;
            WarningGrid.Visibility = System.Windows.Visibility.Visible;
            this.Height = 388;
        }

        //Perform login operation on Enter key
        private void UserNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LogUser();
        }

        //Perform login operation on Enter Key
        private void passwordBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                LogUser();
        }

        private void Login_Label_Forgot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
             
        }

        private void ChildWindow_Closed(object sender, EventArgs e)
        {

        }
    }
}

