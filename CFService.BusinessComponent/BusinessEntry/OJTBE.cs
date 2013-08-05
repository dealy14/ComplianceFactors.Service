using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CFService.BusinessComponent.BusinessEntry
{

    public class OJTBEBase
    {
        public string OJTID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }


    public class OJTBE : OJTBEBase
    {
        public string OJTNumber { get; set; }
        public string Title { get; set; }
        public string description { get; set; }
        public string location { get; set; }

        public string Trainer { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
        public bool IsSaftyBrief { get; set; }
        public string frequency { get; set; }
        public bool IsHarmRelated { get; set; }
        public string HarmId { get; set; }
        //public string HarmTitle { get; set; }
        //public string HarmNumber { get; set; }
        public bool isCertifiedRelated { get; set; }
        public string CertifiedFilePath { get; set; }
        public string CertifiedFileName { get; set; }
        public string CertifiedFileExt { get; set; }
        public string Other { get; set; }
        public string CreatedDate { get; set; }
        //public bool isComplete { get; set; }
        public List<OJTSentToBE> SentTo { get; set; }
        public List<OJTAttachmentBE> Attachment { get; set; }
        public string SentToXML { get; set; }
        public string AttachmentXML { get; set; }
        public bool Status { get; set; }
        public bool IsAcknowledge { get; set; }
    }

    public class OJTSentToBE
    {
        public string OJTID { get; set; }
        [XmlAttribute]
        public string SentTo { get; set; }
        [XmlAttribute]
        public bool IsAcknowledge { get; set; }
        public bool AcknowledgeStaus { get; set; }
        public bool IsMobileSync { get; set; }
        public string AcknowledgeDate { get; set; }
    }

    public class OJTAttachmentBE
    {
        [XmlAttribute]
        public string AttachmentID { get; set; }
        [XmlAttribute]
        public string OJTID { get; set; }
        public string FilePath { get; set; }
        [XmlAttribute]
        public string FileType { get; set; }
        [XmlAttribute]
        public string OJTFileName { get; set; }
        public string FileContent { get; set; }
    }

    public class OJTSavedWebToMobileBE
    {
        public List<OJTBE> LOJT { get; set; }
        public List<OJTAttachmentBE> LOJTAttachments { get; set; }
    }

    public class OJTReceivedWebToMobileBE
    {
        public List<OJTDownloadBE> LOJT { get; set; }
        public List<OJTAttachmentBE> LOJTAttachments { get; set; }
    }

    public class OJTDownloadBE
    {
        public string OJTID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string OJTNumber { get; set; }
        public string Title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string Trainer { get; set; }
        public string Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Duration { get; set; }
        public string Type { get; set; }
        public bool IsSaftyBrief { get; set; }
        public string frequency { get; set; }
        public bool IsHarmRelated { get; set; }
        public string HarmId { get; set; }
        //public string HarmTitle { get; set; }
        //public string HarmNumber { get; set; }
        public bool isCertifiedRelated { get; set; }
        public string CertifiedFilePath { get; set; }
        public string CertifiedFileName { get; set; }
        public string CertifiedFileExt { get; set; }
        public string Other { get; set; }
        public string CreatedDate { get; set; }
        //public bool isComplete { get; set; }
        public List<OJTSentToBE> SentTo { get; set; }
        public List<OJTAttachmentBE> Attachment { get; set; }
        public string SentToXML { get; set; }
        public string AttachmentXML { get; set; }
        public bool Status { get; set; }
        public bool IsAcknowledge { get; set; }
    }


    public class OJTDownloadAcknowlegdeStatusBE
    {
        public string OJTID { get; set; }
        public string SentToUser { get; set; }
        public string SentToUserByName { get; set; }
        public string AcknowledgeDate { get; set; }
    }


    //Get Archive FN from Web 
    public class OJTArchive
    {
        public string OJTID { get; set; }
        public string UserId { get; set; }
        public string UserIdByName { get; set; }
        public string OJTType { get; set; }
    }

    public class OJTHARM
    {
        public string HARMID { get; set; }
        public string HARMNO { get; set; }
        //public string UserId { get; set; }
    }
}
