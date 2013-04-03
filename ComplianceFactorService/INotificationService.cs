using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "INotificationService" in both code and config file together.
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract]
        string HelloTest(string strUserID);
        [OperationContract]
        string GetNewNotificationCount(string UserID);
    }

    [DataContract]
    public class clsNotificationResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public string FieldNotesCount { get; set; }
        [DataMember]
        public string MInspectionCount { get; set; }
       [DataMember]
        public string OJTCount { get; set; }

    }
}
