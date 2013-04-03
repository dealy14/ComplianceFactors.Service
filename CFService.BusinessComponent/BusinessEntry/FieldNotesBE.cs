using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CFService.BusinessComponent.BusinessEntry
{

    public class FieldNotesBEBase
    {
        public string FieldNoteID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
    }

    public class FieldNotesBE : FieldNotesBEBase
    {
        //public string FieldNoteID { get; set; }
        //public string CreatedBy { get; set; }
        public string Title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string FielsNoteNO { get; set; }
        public List<FieldNotesSentToBE> SentTo { get; set; }
        public List<FieldNotesAttachmentBE> Attachment { get; set; }
        public string SentToXML { get; set; }
        public string AttachmentXML { get; set; }
        public bool Status { get; set; }
       // public bool IsAcknowledge { get; set; }
    }

    public class FieldNotesSentToBE
    {
        public string FieldNoteID { get; set; }
        [XmlAttribute]
        public string SentTo { get; set; }
        [XmlAttribute]
        public bool IsAcknowledge { get; set; }
        public bool AcknowledgeStaus { get; set; }
        public bool IsMobileSync { get; set; }
    }

    public class FieldNotesAttachmentBE
    {
        [XmlAttribute]
        public string FNAttachmentID { get; set; }
        [XmlAttribute]
        public string FieldNoteID { get; set; }
        public string FilePath { get; set; }
        [XmlAttribute]
        public string FileType { get; set; }
        [XmlAttribute]
        public string FNFileName { get; set; }
        public string FileContent { get; set; }
    }


    public class FieldNotesSavedWebToMobileBE
    {
       public List<FieldNotesBE> LFieldNotes{get;set;}
       public List<FieldNotesAttachmentBE> LFieldNoteAttachments { get; set; }
    }

    public class FieldNotesReceivedWebToMobileBE
    {
        public List<FieldNotesDownloadBE> LFieldNotes { get; set; }
        public List<FieldNotesAttachmentBE> LFieldNoteAttachments { get; set; }
    }

    public class FieldNotesDownloadBE
    {
        public string FieldNoteID { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string Title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string FielsNoteNO { get; set; }
        public bool IsAcknowledge { get; set; }
    }


    public class FNDownloadAcknowlegdeStatusBE
    {
        public string FieldNoteID { get; set; }
        public string SentToUser { get; set; }
        public string SentToUserByName { get; set; }
    }


    //Get Archive FN from Web 
    public class FieldNoteArchive
    {
        public string FieldNoteID { get; set; }
        public string UserId { get; set; }
        public string UserIdByName { get; set; }
        public string FieldNoteType { get; set; }
    }

}
