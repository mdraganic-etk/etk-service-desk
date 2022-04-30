using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.IO;
using System.Net.Mail;
using System.Configuration;


namespace BusinessSystemsApp.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Logger
    {
        [OperationContract]
        public bool LoggException(string _browserInfo, string _exception, string _user)
        {
            bool success = false;

            StreamWriter stream;
            stream = File.AppendText(ConfigurationManager.AppSettings["LogPath"]);
            stream.WriteLine(DateTime.Now + " (" + _browserInfo + ") - " + _user + " - Exception: " + _exception);
            stream.Close();

            try
            {
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(ConfigurationManager.AppSettings["Exception_Notification_Client"]);
               

                msg.To.Add(new MailAddress(ConfigurationManager.AppSettings["Exception_Notification_Person"]));
               
                msg.Subject = "Exception at user: " + _user;

                msg.Body = DateTime.Now + " - " + _exception + Environment.NewLine + Environment.NewLine + "Browser: " + _browserInfo;
                msg.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                
                smtp.Send(msg);
                success = true;
            }
            catch
            {
                success = false;
            }

            return success;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
