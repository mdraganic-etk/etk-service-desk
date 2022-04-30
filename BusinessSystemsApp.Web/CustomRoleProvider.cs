using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace BusinessSystemsApp.Web
{
    public class CustomRoleProvider:RoleProvider
    {
        public override string[] GetRolesForUser(string username)
        {
            using (BusinessSystemsConnectionString context = new BusinessSystemsConnectionString())
            {
                var user = context.User.Include("UserType").Where(u => u.UserName == username).FirstOrDefault();
                
                if(user != null)
                    return new string[] {(user as User).UserType.TypeName};
                else
                    return new string[]{};
            }
        }

        public override string ApplicationName
        {
            get { return "BusinessSystems"; }
            set { }
        }

        // Other overrides not implemented
        #region Not Implemented Overrides
        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}