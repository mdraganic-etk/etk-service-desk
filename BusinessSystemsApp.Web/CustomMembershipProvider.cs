using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.ServiceModel.DomainServices.Server.ApplicationServices;
using System.DirectoryServices;
using System.Threading;
using System.Diagnostics;
using System.Configuration;

namespace BusinessSystemsApp.Web
{
    public class CustomMembershipProvider : MembershipProvider
    {

        public override bool ValidateUser(string username, string password)
        {
            if (ConfigurationManager.AppSettings["AuthenticationType"].ToString() == "Database")
            {
                PasswordHash ph = new PasswordHash();

                String str = ph.HashPassword(password);

                using (BusinessSystemsConnectionString context = new BusinessSystemsConnectionString())
                {
                    var user = context.User.Where(u => u.UserName == username &&
                        u.Password.Trim() == password).FirstOrDefault();
                    return user != null;
                }
            }
            else if (ConfigurationManager.AppSettings["AuthenticationType"].ToString() == "LDAP")
            {
                //Find the person in the directory to determine their distinct name

                try
                {
                    String _path = null;

                    if (ConfigurationManager.AppSettings["ETKLDAP_Path"].ToString() != "")
                        _path = ConfigurationManager.AppSettings["ETKLDAP_Path"].ToString();


                    DirectoryEntry root = new DirectoryEntry(_path, username, password);

                    Object nativeObject = root.NativeObject;

                    return true;

                    //DirectorySearcher searcher = new DirectorySearcher(root)
                    //    {
                    //        Filter = ("(SAMAccountName=" + username + ")")
                    //    };

                    //searcher.PropertiesToLoad.Add("givenName");
                    //searcher.PropertiesToLoad.Add("sn");
                    //searcher.PropertiesToLoad.Add("cn");

                    //SearchResult findResult = searcher.FindOne();

                    //if (findResult == null)
                    //{
                    //    return false;
                    //}

                    //_path = findResult.Path;

                    //_filterAtribute = (String)findResult.Properties["cn"][0];

                    //return true;
                }
                catch (DirectoryServicesCOMException cex)
                {
                    String _path1 = null;

                    if (ConfigurationManager.AppSettings["ExternLDAP_Path"].ToString() != "")
                        _path1 = ConfigurationManager.AppSettings["ExternLDAP_Path"].ToString();

                    DirectoryEntry root1 = new DirectoryEntry(_path1, username, password);

                    Object nativeObject = root1.NativeObject;

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
                return false;
        }


        public override string ApplicationName
        {
            get { return "BusinessSystems"; }
            set { }
        }


        // Other overrides not implemented
        #region Other overrides not implemented
        public override MembershipUser CreateUser(string username, string password, string email,
    string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey,
    out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }


        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}