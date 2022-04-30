using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.DomainServices.EntityFramework;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace BusinessSystemsApp.Web
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Configuration" in code, svc and config file together.
    
    [ServiceContract(Namespace="")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    
    public class Configuration
    {
        [OperationContract]
        public string  GetAppSettingsValue(string name)
        {
            return ConfigurationManager.AppSettings[name];
        }
    }
}
