using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFService.BusinessComponent.BusinessEntry
{
    public class UserBE
    {
        public string Userid { get; set; }
        public string Username { get; set; }
        public string Password_ash { get; set; }
        //public string Password_salt { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string EmailId { get; set; }
        public bool Active_status_flag { get; set; }
        //public string Active_Type { get; set; }
        //public string u_locale_descriptoin { get; set; }
        //public string u_timezone_location { get; set; }
    }

    public class UserAuthenticateBE
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ipaddress { get; set; }
        public string device { get; set; }
    }


    public class AllUsers
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserNameEnc{ get; set; }
    }
}
