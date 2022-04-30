using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace BusinessSystemsApp.Helpers
{
    public class ComposeAndSendEmail
    {
        //Type of mails which can be send
        public enum MailType
        {
            NewCSR,
            CSRChanges,
            NewNote
        };

        private ObservableCollection<String> AddressList;
        private MailType type;

        private String MailBody = null;
        private String MailSubject = null;

        /// <summary>
        /// Creates e-mail message with provided prameters
        /// </summary>
        /// <param name="addressList">List of user addresses to send e.mail notification</param>
        /// <param name="csrId">CSR Id</param>
        /// <param name="heading">Heading of CSR</param>
        /// <param name="description">Description of CSR</param>
        /// <param name="priority">Priority name of CSR</param>
        /// <param name="company">CSR Company</param>
        /// <param name="date">Date of CSR</param>
        /// <param name="user">User of CSR</param>
        /// <param name="caller">Caller for CSR</param>
        /// <param name="mailType">Type of emal ENUM</param>
        /// <param name="changes">Checj+k if mail is for changes of CSR</param>
        public ComposeAndSendEmail(ObservableCollection<String> addressList, String csrId, String heading, String description, String priority, String company, String date, String user, String caller, String product, String attachment, String userPhone, String userMail, MailType mailType, String changes)
        {
            AddressList = addressList;
            type = mailType;

            if (mailType == MailType.NewCSR)
            {
                MailSubject = "CSR no " + csrId + " reported by: " + company + "(" + user + ") - Priority: " + priority;

                MailBody += "Dear " + user + Environment.NewLine + "Thank you for contacting us. Your request: " + heading + " with refence number: " + csrId + " is taken care of.";
                MailBody += Environment.NewLine + Environment.NewLine + "You can track your request at : " + MainPage.ApplicationUrl;
                
                MailBody += Environment.NewLine + Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------";
                //MailBody += Environment.NewLine + "CSR filled by: " + user;
                //MailBody += Environment.NewLine + "User e-mail: " + userMail;
                //MailBody += Environment.NewLine + "User phone: " + userPhone;
                //MailBody += Environment.NewLine + "Company: " + company;
                 MailBody += Environment.NewLine + "Reported at: " + date;
                //if(caller != null)
                //    MailBody += Environment.NewLine + "Caller: " + caller;
                MailBody += Environment.NewLine + "Product: " + product;
                MailBody += Environment.NewLine + "Priority: " + priority;
                MailBody += Environment.NewLine + "Problem heading: " + heading;
                //MailBody += Environment.NewLine + "Problem description: " + description;
                //MailBody += Environment.NewLine + "Attachment: " + attachment;
            }
            else if (mailType == MailType.CSRChanges)
            {
                MailSubject = "CSR no " + csrId + " changed by: " + user;

                MailBody += "Dear " + user + Environment.NewLine + "Your request: " + heading + " with refence number: " + csrId + " was changed.";

                MailBody += Environment.NewLine + Environment.NewLine + "Changes: " + changes;

            }
            else if (mailType == MailType.NewNote)
            {
                MailSubject = "New note in CSR no: " + csrId + " entered by: " + user; 

                MailBody += "Dear " + user + Environment.NewLine + "Thank you for contacting us. Your note: " + heading + " for CSR number: " + csrId + " has been saved.";
                MailBody += Environment.NewLine + Environment.NewLine + "You can track your request at : " + MainPage.ApplicationUrl;

                MailBody += Environment.NewLine + Environment.NewLine + "------------------------------------------------------------------------------------------------------------------------";
                MailBody += Environment.NewLine + "Note filled by: " + user;
                MailBody += Environment.NewLine + "Company: " + company;
                MailBody += Environment.NewLine + "Note heading: " + heading;
                //MailBody += Environment.NewLine + "Note description: " + description;
                //MailBody += Environment.NewLine + "Attachment: " + attachment;
            }
        }

        /// <summary>
        /// Send created e-mail
        /// </summary>
        public void Send()
        {
            System.ServiceModel.BasicHttpBinding bindingMail = new System.ServiceModel.BasicHttpBinding();

            Uri address = null;

            if (App.Current.Host.Source.AbsoluteUri.StartsWith("https"))
            {
                bindingMail.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

                address = new Uri(Application.Current.Host.Source, "../MailService.svc");
            }
            else
            {
                address = new Uri(Application.Current.Host.Source, "../MailService.svc");
            }

            System.ServiceModel.EndpointAddress addressMail = new System.ServiceModel.EndpointAddress(address.AbsoluteUri);
            
            MailServiceReference.MailServiceClient client = new MailServiceReference.MailServiceClient(bindingMail, addressMail);

            //Send e-mails to all users which have to be notified
            client.SendMailAsync(AddressList, "csrTool@ericsson.hr", MailSubject, MailBody);

            client.SendMailCompleted += new EventHandler<MailServiceReference.SendMailCompletedEventArgs>(client_SendMailCompleted);
        }


        void client_SendMailCompleted(object sender, MailServiceReference.SendMailCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Helpers.MessageBox msg = new Helpers.MessageBox("Error", e.Error.Message, e.Error.InnerException.Message);
                msg.Show();
            }
        }
    }
}
