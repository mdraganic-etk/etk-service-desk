using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Net.Mail;
using System.ServiceModel.DomainServices.EntityFramework;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Collections.Generic;

namespace BusinessSystemsApp.Web
{
    [RequiresAuthentication]
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class MailService
    {
        
        [OperationContract]
        public bool SendMail(List<string> emailTo, string emailFrom, string msgSubject, string msgBody)
        {
            bool success = false;

            try
            {
                MailMessage msg = new MailMessage();

                msg.From = new MailAddress(emailFrom);

                foreach (string to in emailTo)
                {
                    msg.To.Add(new MailAddress(to));
                }

                msg.Subject = msgSubject;

                msg.Body = msgBody;
                msg.IsBodyHtml = false;

                SmtpClient smtp = new SmtpClient();
                
                //smtp.EnableSsl = true;
                
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
