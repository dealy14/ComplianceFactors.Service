using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using CFService.BusinessComponent;
using CFService.BusinessComponent.BusinessEntry;
using ComplianceFactorService.Utility;

namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AuthenticationService" in code, svc and config file together.
    public class AuthenticationService : IAuthenticationService
    {
        Utility.Utility objUtility = new Utility.Utility();
        public string HelloTest()
        {
            return "Hello Test Return";
        }

        public string HelloTest1()
        {
            return "Hello Test Return";
        }

        #region UserAuthentication
        public string UserAuthentication(string UserAuthenticationRequest)
        {
            string strResponse = string.Empty;
            UserAuthenticationResponse objResponse = new UserAuthenticationResponse();
            //List<UserBE> lstUserBE = new List<UserBE>();
            try
            {
                UserAuthenticateBE objUserBE = new UserAuthenticateBE();
                objUserBE = Utility.Utility.JsonDeserialize<UserAuthenticateBE>(UserAuthenticationRequest);

                //encrypt and check with db
                Utility.Utility objUtility = new Utility.Utility();
                objUserBE.Username = objUtility.GenerateHashvalue(objUserBE.Username, true);
                objUserBE.Password = objUtility.GenerateHashvalue(objUserBE.Password, true);

                DataTable dtUser = UserBLL.UserAuthentication(objUserBE);
                if (dtUser.Rows.Count > 0)
                {
                    //foreach (DataRow dr in dtUser.Rows)
                    //{
                        UserBE objUser = new UserBE();
                        objUser.Userid = dtUser.Rows[0]["u_user_id_pk"].ToString();
                        objUser.Username = dtUser.Rows[0]["u_username_enc"].ToString();
                        objUser.Password_ash = dtUser.Rows[0]["u_password_enc_ash"].ToString();
                        //objUser.Password_salt = dtUser.Rows[0]["u_password_enc_salt"].ToString();
                        objUser.Firstname = dtUser.Rows[0]["u_first_name"].ToString();
                        objUser.Middlename = dtUser.Rows[0]["u_middle_name"].ToString();
                        objUser.Lastname = dtUser.Rows[0]["u_last_name"].ToString();
                        objUser.EmailId = dtUser.Rows[0]["u_email_address"].ToString();
                        objUser.Active_status_flag = Convert.ToBoolean(dtUser.Rows[0]["u_active_status_flag"]);
                        //objUser.Active_Type = dtUser.Rows[0]["u_active_type_fk"].ToString();
                        //objUser.locale_descriptoin = dtUser.Rows[0]["u_locale_id_fk"].ToString();
                        //objUser.timezone_location = dtUser.Rows[0]["u_timezone_fk"].ToString();
                        //lstUserBE.Add(objUser);

                        if (objUser.Active_status_flag)
                        {
                            objResponse.StatusCode = "0";
                            objResponse.StatusMessage = "Success";
                            //objResponse.User = lstUserBE;
                            objResponse.User = objUser;
                        }
                        else
                        {
                            objResponse.StatusCode = "-2";
                            objResponse.StatusMessage = "Inactive";
                            objResponse.User = null;
                        }
                    //}
                }
                else
                {
                    objResponse.StatusCode = "-1";
                    objResponse.StatusMessage = "Record Not Found";
                }
                
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                    {
                        Logger.WriteToErrorLog("AuthenticationService:UserAuthentication() ", ex.Message, ex.InnerException.Message);
                    }
                    else
                    {
                        Logger.WriteToErrorLog("AuthenticationService:UserAuthentication() ", ex.Message);
                    }
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = "General Error";
                    objResponse.User = null;
                }
            }

            strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse; 
        }
        #endregion

        //#region GetParticularUserDetails
        //public string GetParticularUserDetails(string strUserID)
        //{
        //    clsUser objUser = new clsUser();
        //    ClsUserResponse objClsUserResponse = new ClsUserResponse();
        //    List<clsUser> lstUSer = new List<clsUser>();
        //    try
        //    {
        //        DataTable dtUser = UserBLL.GetUserDetails(strUserID);
        //        if (dtUser.Rows.Count > 0)
        //        {
        //            objUser.Userid = dtUser.Rows[0]["u_user_id_pk"].ToString();
        //            objUser.Username = dtUser.Rows[0]["u_username_enc"].ToString();
        //            objUser.Password_ash = dtUser.Rows[0]["u_password_enc_ash"].ToString();
        //            objUser.Password_salt = dtUser.Rows[0]["u_password_enc_salt"].ToString();
        //            objUser.Firstname = dtUser.Rows[0]["u_first_name"].ToString();
        //            objUser.Middlename = dtUser.Rows[0]["u_middle_name"].ToString();
        //            objUser.Lastname = dtUser.Rows[0]["u_last_name"].ToString();
        //            objUser.EmailId = dtUser.Rows[0]["u_email_address"].ToString();
        //            objUser.Active_status_flag = Convert.ToBoolean(dtUser.Rows[0]["u_active_status_flag"]);
        //            objUser.Active_Type = dtUser.Rows[0]["u_active_type_fk"].ToString();
        //            objUser.locale_descriptoin = dtUser.Rows[0]["u_locale_id_fk"].ToString();
        //            objUser.timezone_location = dtUser.Rows[0]["u_timezone_fk"].ToString();
        //            lstUSer.Add(objUser);

        //            objClsUserResponse.StatusCode = "0";
        //            objClsUserResponse.StatusMessage = "Success";
        //            objClsUserResponse.AllUsers = lstUSer;
        //        }
        //        else
        //        {
        //            objClsUserResponse.StatusCode = "1";
        //            objClsUserResponse.StatusMessage = "Not Found";
        //        }
        //    }
        //    catch (ArgumentException argEx)
        //    {
        //        objClsUserResponse.StatusCode = "-1";
        //        objClsUserResponse.StatusMessage = argEx.Message;
        //    }
        //    catch (ObjectDisposedException objDesposedEx)
        //    {
        //        objClsUserResponse.StatusCode = "-2";
        //        objClsUserResponse.StatusMessage = objDesposedEx.Message;
        //    }
        //    catch (InvalidOperationException invalidOprEx)
        //    {
        //        objClsUserResponse.StatusCode = "-3";
        //        objClsUserResponse.StatusMessage = invalidOprEx.Message;
        //    }
        //    catch (NullReferenceException nullEx)
        //    {
        //        objClsUserResponse.StatusCode = "-4";
        //        objClsUserResponse.StatusMessage = nullEx.Message;
        //    }
        //    catch (IndexOutOfRangeException indexOutEx)
        //    {
        //        objClsUserResponse.StatusCode = "-5";
        //        objClsUserResponse.StatusMessage = indexOutEx.Message;
        //    }
        //    catch (FaultException faultEx)
        //    {
        //        objClsUserResponse.StatusCode = "-6";
        //        objClsUserResponse.StatusMessage = faultEx.Message;
        //    }
        //    catch (Exception ex)
        //    {
        //        objClsUserResponse.StatusCode = "-7";
        //        objClsUserResponse.StatusMessage = ex.Message;
        //    }

        //    string strResponse = Utility.Utility.JsonSerialize(objClsUserResponse.GetType(), objClsUserResponse);
        //    return strResponse;  
        //}
        //#endregion

        #region GetAllComplianceOfficer

        public string GetAllComplianceOfficer()
        {
            GetAllUsersResponse objResponse = new GetAllUsersResponse();
            try
            {
                List<AllUsers> lstUsers = null;
                lstUsers = UserBLL.GetUsers();

                if (lstUsers != null)
                {
                    List<AllUsers> lstUsersTemp = new List<AllUsers>();
                    foreach (AllUsers obj in lstUsers)
                    {
                        string strCreatedByNameEnc = objUtility.Decrypt(obj.UserNameEnc, true);
                        obj.UserName = strCreatedByNameEnc;
                        lstUsersTemp.Add(obj);
                    }
                    objResponse.User = lstUsersTemp;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
                else
                {
                    objResponse.StatusCode = "-1";
                    objResponse.StatusMessage = "Record Not Found";
                }

            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("AuthenticationService:GetAllComplianceOfficer() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("AuthenticationService:GetAllComplianceOfficer() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }

            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        //public string GetAllUsers()
        //{
        //    ClsUserResponse objClsUserResponse = new ClsUserResponse();
        //    try
        //    {
        //        List<clsUser> lstUsers = new List<clsUser>();
        //        DataTable dtUser = UserBLL.GetAllUserDetails();
        //        if (dtUser.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dtUser.Rows)
        //            {
        //                clsUser objUser = new clsUser();
        //                objUser.Userid = dr["u_user_id_pk"].ToString();
        //                objUser.Username = dtUser.Rows[0]["u_username_enc"].ToString();
        //                objUser.Password_ash = dr["u_password_enc_ash"].ToString();
        //                objUser.Password_salt = dr["u_password_enc_salt"].ToString();
        //                objUser.Firstname = dr["u_first_name"].ToString();
        //                objUser.Middlename = dr["u_middle_name"].ToString();
        //                objUser.Lastname = dr["u_last_name"].ToString();
        //                objUser.EmailId = dr["u_email_address"].ToString();
        //                objUser.Active_status_flag = Convert.ToBoolean(dr["u_active_status_flag"]);
        //                objUser.Active_Type = dr["u_active_type_fk"].ToString();
        //                objUser.locale_descriptoin = dr["u_locale_id_fk"].ToString();
        //                objUser.timezone_location = dr["u_timezone_fk"].ToString();
        //                lstUsers.Add(objUser);
        //            }

        //            objClsUserResponse.AllUsers = lstUsers;
        //            objClsUserResponse.StatusCode = "0";
        //            objClsUserResponse.StatusMessage = "Success";
        //        }
        //        else
        //        {
        //            objClsUserResponse.StatusCode = "1";
        //            objClsUserResponse.StatusMessage = "Not Found";
        //            objClsUserResponse.AllUsers = null;
        //        }
        //    }
        //    catch (ArgumentException argEx)
        //    {
        //        objClsUserResponse.StatusCode = "-1";
        //        objClsUserResponse.StatusMessage = argEx.Message;
        //        objClsUserResponse.AllUsers = null;
        //    }
        //    catch (ObjectDisposedException objDesposedEx)
        //    {
        //        objClsUserResponse.StatusCode = "-2";
        //        objClsUserResponse.StatusMessage = objDesposedEx.Message;
        //        objClsUserResponse.AllUsers = null;
        //    }
        //    catch (InvalidOperationException invalidOprEx)
        //    {
        //        objClsUserResponse.StatusCode = "-3";
        //        objClsUserResponse.StatusMessage = invalidOprEx.Message;
        //        objClsUserResponse.AllUsers = null;
        //    }
        //    catch (NullReferenceException nullEx)
        //    {
        //        objClsUserResponse.StatusCode = "-4";
        //        objClsUserResponse.StatusMessage = nullEx.Message;
        //        objClsUserResponse.AllUsers = null;
        //    }
        //    catch (IndexOutOfRangeException indexOutEx)
        //    {
        //        objClsUserResponse.StatusCode = "-5";
        //        objClsUserResponse.StatusMessage = indexOutEx.Message;
        //        objClsUserResponse.AllUsers = null;
        //    }
        //    catch (FaultException faultEx)
        //    {
        //        objClsUserResponse.StatusCode = "-6";
        //        objClsUserResponse.StatusMessage = faultEx.Message;
        //        objClsUserResponse.AllUsers = null;
        //    }

        //    catch (Exception ex)
        //    {
        //        objClsUserResponse.StatusCode = "-7";
        //        objClsUserResponse.StatusMessage = ex.Message;
        //        objClsUserResponse.AllUsers = null;
        //    }
        //    string strResponse = Utility.Utility.JsonSerialize(objClsUserResponse.GetType(), objClsUserResponse);
        //    return strResponse;
        //}
        #endregion
    }
}
