using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BusinessSystemsApp.Web
{
    public class PasswordHash
    {
        public String HashPassword(string _password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(_password, "SHA1");
        }
    }
}