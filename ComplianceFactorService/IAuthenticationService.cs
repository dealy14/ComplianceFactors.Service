using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CFService.BusinessComponent.BusinessEntry;

namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAuthenticationService" in both code and config file together.
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        string HelloTest();
        [OperationContract]
        //bool UserAuthentication(clsUserAuthenticate clsUserAuthenticate);
        string UserAuthentication(string UserAuthenticationRequest);
        //[OperationContract]
        //string GetParticularUserDetails(string strUserID);
        //[OperationContract]
        //string GetAllUsers();
        //Get all CO
        [OperationContract]
        string GetAllComplianceOfficer();

        
    }

    [DataContract]
    public class UserAuthenticationResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        //public List<UserBE> User { get; set; }
        public UserBE User { get; set; }
    }


    [DataContract]
    public class GetAllUsersResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public List<AllUsers> User { get; set; }
    }



    //[DataContract]
    //public class clsUser
    //{
    //    [DataMember]
    //    public string Userid { get; set; }
    //    [DataMember]
    //    public string Username { get; set; }
    //    [DataMember]
    //    public string Password_ash { get; set; }
    //    [DataMember]
    //    public string Password_salt { get; set; }
    //    [DataMember]
    //    public string Firstname { get; set; }
    //    [DataMember]
    //    public string Middlename { get; set; }
    //    [DataMember]
    //    public string Lastname { get; set; }
    //    [DataMember]
    //    public string EmailId { get; set; }
    //    [DataMember]
    //    public bool Active_status_flag { get; set; }
    //    [DataMember]
    //    public string Active_Type { get; set; }
    //    [DataMember]
    //    public string locale_descriptoin { get; set; }
    //    [DataMember]
    //    public string timezone_location { get; set; }
    //}

   
     
}
