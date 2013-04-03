using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFService.BusinessComponent.BusinessEntry
{
    public class OJTBE
    {
        public string OJTID { get; set; }
        public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string description { get; set; }
        public string location { get; set; }

        public string Trainer { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
        public bool IsSaftyBrief { get; set; }
        public string frequency { get; set; }
        public bool IsHarmRelated { get; set; }
        public string HarmId { get; set; }
        public string HarmTitle { get; set; }
        public string HarmNumber { get; set; }
        public bool isCertifiedRelated { get; set; }
        public string Other { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isComplete { get; set; }
        public bool isActive { get; set; }

        public string SentTo { get; set; }
        public bool IsAcknowledge { get; set; }
    }

    public class OJTSentToBE
    {
        public string OJTID { get; set; }
        public string SentTo { get; set; }
        public bool IsAcknowledge { get; set; }
        public bool AcknowledgeStaus { get; set; }
        public bool IsMobileSync { get; set; }
    }

    public class OJTAttachmentBE
    {
        public string OJTID { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}
