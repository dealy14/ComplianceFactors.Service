using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data;
using CFService.BusinessComponent;
using ComplianceFactorService.Utility;
using System.Web;
using System.Web.Hosting;

namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NotificationService" in code, svc and config file together.
    public class NotificationService : INotificationService
    {

        public string HelloTest(string strUserID)
        {
            return "Hello Test Notification Return "+ strUserID;
        }

        public string GetNewNotificationCount(string strUserID)
        {

            clsNotificationResponse objResponse = new clsNotificationResponse();
            try
            {
                DataTable dtNotify = UserBLL.GetAllNotifications(strUserID);
                if (dtNotify.Rows.Count > 0)
                {
                    foreach (DataRow drNotify in dtNotify.Rows)
                    {
                        objResponse.FieldNotesCount = drNotify["FieldnoteCount"].ToString();
                        objResponse.MInspectionCount = drNotify["InspectionCount"].ToString();
                        objResponse.OJTCount = drNotify["OjtCount"].ToString();
                        objResponse.StatusCode = "0";
                        objResponse.StatusMessage = "Success";
                    }
                }
            }
            catch(Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("NotificationService:GetNewNotificationCount() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("NotificationService:GetNewNotificationCount() ", ex.Message);

                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = "General Error";
                }
               
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse; 
        }
    }
}
