using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;

using CFService.BusinessComponent.BusinessEntry;
using ComplicanceFactor.DataAccessLayer;
namespace CFService.BusinessComponent
{
    public class UserBLL
    {
        public static DataTable UserAuthentication(UserAuthenticateBE clsUserAuthenticate)
        {
            Hashtable htLogin = new Hashtable();
            
            htLogin.Add("@Username", clsUserAuthenticate.Username);
            htLogin.Add("@Password", clsUserAuthenticate.Password);
            htLogin.Add("@IPAddress", clsUserAuthenticate.ipaddress);
            htLogin.Add("@Device", clsUserAuthenticate.device);
            try
            {
                DataTable dtUsers = DataProxy.FetchDataTable("u_wcf_get_user_login", htLogin);
                return dtUsers;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static List<AllUsers> GetUsers()
        {

            AllUsers objReturn = null;
            List<AllUsers> lstReturn = null;

            DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_all_compliance_officer");
            if (dtResult.Rows.Count > 0)
            {
                lstReturn = new List<AllUsers>();
                foreach (DataRow dr in dtResult.Rows)
                {
                    objReturn = new AllUsers();
                    objReturn.UserID = dr["u_user_id_pk"].ToString();
                    objReturn.UserNameEnc = dr["u_username_enc"].ToString();
                    //objReturn.SentToUserByName = dr["u_username_enc"].ToString();
                    lstReturn.Add(objReturn);
                }
            }
            return lstReturn;
        }

        //public static DataTable GetAllUserDetails()
        //{
        //    try
        //    {
        //        return DataProxy.FetchDataTable("u_wcf_get_all_user");
        //    }

        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Get Notification for Fieldnotes, MInspections, OJT
        /// </summary>
        /// <returns>Count of each in Datatable</returns>
        public static DataTable GetAllNotifications(string strUserID)
        {
            try
            {
                Hashtable htGetNotification = new Hashtable();
                htGetNotification.Add("@UserId", strUserID);
                return DataProxy.FetchDataTable("sv_wcf_Get_Notifications", htGetNotification);
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
