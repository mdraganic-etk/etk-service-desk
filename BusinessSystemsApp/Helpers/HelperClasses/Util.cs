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
using BusinessSystemsApp.Web;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Text;

namespace BusinessSystemsApp.Helpers.HelperClasses
{
    public class Util
    {
        
        
        /// <summary>
        /// Check if item exists in list of products
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_newItem"></param>
        /// <returns></returns>
      
        public bool ExistsInList(ObservableCollection<ProductsInCompany> _list, ProductsInCompany _newItem)
        {
            if (_list == null)
                return false;
            else
            {
                foreach (ProductsInCompany item in _list)
                {
                    if (item.CompanyId == _newItem.CompanyId && item.ProductId == _newItem.ProductId)
                        return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Check if item exists in list of priorities
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_newItem"></param>
        /// <returns></returns>
        public bool ExistsInList(ObservableCollection<CompanyPriorities> _list, CompanyPriorities _newItem)
        {
            if (_list == null)
                return false;
            else
            {
                foreach (CompanyPriorities item in _list)
                {
                    if (item.CompanyId == _newItem.CompanyId && item.PriorityId == _newItem.PriorityId)
                        return true;
                }

                return false;
            }
        }


        /// <summary>
        /// Check if item exists in list of statuses
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_newItem"></param>
        /// <returns></returns>
        public bool ExistsInList(ObservableCollection<CompanyStatuses> _list, CompanyStatuses _newItem)
        {
            if (_list == null)
                return false;
            else
            {
                foreach (CompanyStatuses item in _list)
                {
                    if (item.CompanyId == _newItem.CompanyId && item.StatusId == _newItem.StatusId)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Check if item exists in list of companies
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_newItem"></param>
        /// <returns></returns>
        public bool ExistsInList(ObservableCollection<CompaniesAssignment> _list, CompaniesAssignment _newItem)
        {
            if (_list == null)
                return false;
            else
            {
                foreach (CompaniesAssignment item in _list)
                {
                    if (item.CompanyId == _newItem.CompanyId && item.AssignedCommpanyId == _newItem.AssignedCommpanyId)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Check if item exists in list of users
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_newItem"></param>
        /// <returns></returns>
        public bool ExistsInList(ObservableCollection<UserNotifications> _list, UserNotifications _newItem)
        {
            if (_list == null)
                return false;
            else
            {
                foreach (UserNotifications item in _list)
                {
                    if (item.CompanyId == _newItem.CompanyId && item.UserId == _newItem.UserId)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Check if item exists in list type of requesters
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_newItem"></param>
        /// <returns></returns>
        public bool ExistsInList(ObservableCollection<RequesterTypeInCompany> _list, RequesterTypeInCompany _newItem)
        {
            if (_list == null)
                return false;
            else
            {
                foreach (RequesterTypeInCompany item in _list)
                {
                    if (item.CompanyId == _newItem.CompanyId && item.RequesterTypeId == _newItem.RequesterTypeId)
                        return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Check if item exists in list type of requesters
        /// </summary>
        /// <param name="_list"></param>
        /// <param name="_newItem"></param>
        /// <returns></returns>
        public bool ExistsInList(ObservableCollection<IssueDomainInCompany> _list, IssueDomainInCompany _newItem)
        {
            if (_list == null)
                return false;
            else
            {
                foreach (IssueDomainInCompany item in _list)
                {
                    if (item.CompanyId == _newItem.CompanyId && item.IssueDomainId == _newItem.IssueDomainId)
                        return true;
                }

                return false;
            }
        }

        public bool ValidEmail(string inputEmail)
        {
            bool isEmailValid = true;
            string emailExpression = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            Regex re = new Regex(emailExpression);
            if (!re.IsMatch(inputEmail))
            {
                isEmailValid = false;
            }
            return isEmailValid;
        }

        public String HashPassword(String val)
        {

            System.Text.Encoder enc = System.Text.Encoding.Unicode.GetEncoder();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(val);
            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1Managed();
            byte[] res = sha.ComputeHash(data);
            return System.BitConverter.ToString(res).Replace("-", "").ToUpper();            
        }
    }
}
