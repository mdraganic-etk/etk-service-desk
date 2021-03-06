using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel.DomainServices.Hosting;
using System.ServiceModel.DomainServices.Server;
using System.ServiceModel.DomainServices.Server.ApplicationServices;

namespace BusinessSystemsApp.Web
{
    [EnableClientAccess]
    public class AuthenticationDomainService : AuthenticationBase<AuthUser>
    {
        // To enable Forms/Windows Authentication for the Web Application, 
        // edit the appropriate section of web.config file.
    }

    public class AuthUser : UserBase
    {
        // NOTE: Profile properties can be added here 
        // To enable profiles, edit the appropriate section of web.config file.

        // public string MyProfileProperty { get; set; }
    }
}