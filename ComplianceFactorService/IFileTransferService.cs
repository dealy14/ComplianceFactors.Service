using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;

namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IFileTransferService" in both code and config file together.
    [ServiceContract]
    public interface IFileTransferService
    {
        [OperationContract]
        string DoWork();

        [OperationContract]
        UploadResponseTest FieldNoteAttachmentsUpload(clsFieldNotesAttachment clsfields);


        //Use for live
        [OperationContract]
        byte[] FieldNoteAttachmentsDownload(string strAttachementFileID);

        [OperationContract]
        string Upload(string strJsonString);

        //[OperationContract]
        //string Download(string strJsonString);

        //OJT
        [OperationContract]
        byte[] OJTAttachmentsDownload(string strAttachementFileID);

        [OperationContract]
        string OJTUpload(string strJsonString);

        [OperationContract]
        string OJTCertificateUpload(string strJsonString);

        [OperationContract]
        byte[] OJTCertificateDownload(string strOJTID);

    }

    [MessageContract]
    public class clsFieldNotesAttachment
    {
        [MessageHeader(MustUnderstand = true)]
        public string FNAttachmentID { get; set; }
        [MessageHeader(MustUnderstand = true)]
        public string FileName { get; set; }
        [MessageBodyMember(Order = 1)]
        public Stream Attachments { get; set; }
        //public byte[] Attachments { get; set; }

    }


    [MessageContract]
    public class UploadResponseTest
    {
        [MessageBodyMember(Order = 1)]
        public bool UploadSucceeded { get; set; }
    }


    public class clsFileDetails
    {
        public string FieldNotesAttID { get; set; }
        public string FileBase64string { get; set; }
        public string FileExtension { get; set; }
       // public byte[] Filebyte { get; set; }
    }

    public class clsOJTFileDetails
    {
        public string OJTAttID { get; set; }
        public string FileBase64string { get; set; }
        public string FileExtension { get; set; }
        // public byte[] Filebyte { get; set; }
    }

    public class clsOJTCertificateFileDetails
    {
        public string OJTID { get; set; }
        public string FileBase64string { get; set; }
        public string FileExtension { get; set; }
        // public byte[] Filebyte { get; set; }
    }
}
