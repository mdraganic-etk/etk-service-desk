using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace BusinessSystemsApp.Web
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DateTimeService
    {
        [OperationContract]
        public DateTime GetDateTime()
        {
            // Add your operation implementation here
            return DateTime.Now;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}
