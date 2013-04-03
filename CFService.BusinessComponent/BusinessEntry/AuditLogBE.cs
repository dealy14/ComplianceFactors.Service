using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFService.BusinessComponent.BusinessEntry
{
    public class AuditLogBE
    {
        public string Guid { get; set; }
        public string Auditid { get; set; }
        public string Userid { get; set; }
        public string user_type { get; set; }
        public string user_detail { get; set; }
        public string action_desc { get; set; }
        public DateTime u_datetime { get; set; }
        public string ipaddress { get; set; }
        public string device { get; set; }
    }
}
