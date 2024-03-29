﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using CFService.BusinessComponent.BusinessEntry;

namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICommunicationService" in both code and config file together.
    [ServiceContract]
    public interface ICommunicationService
    {
        #region FielsNotes
        [OperationContract]
        string CreateFieldNote(string strJsonFieldNotes);

        [OperationContract]  //Receive Delete the field notes attachment from mobile
        string DeleteAttachFieldNotes(string strJsonFieldNotesAttach);

        [OperationContract] //Sent Delete the field notes attachment from Web
        string GetDeleteAttachFieldNotesFromWeb(string strUserId);

        //Update Existing(Saved) FieldNotes
        [OperationContract]
        string UpdateFieldNote(string strJsonFieldNotes);

        [OperationContract]
        string GetUpdatedFNWebToMobile(string strJsonFieldNotes);
        [OperationContract]
        string UpdatedFNMobileResponse(string strJsonFieldNotes);
        
        [OperationContract]
        string GetSavedFNWebToMobile(string strJsonFieldNotes);
        [OperationContract]
        string SavedFNMobileResponse(string strJsonFieldNotes);

        [OperationContract]
        string DownloadReceivedFieldNote(string strUserId);
        [OperationContract]
        string ReceivedFieldNoteResponse(string strJsonString);

        [OperationContract]
        string UserAcknowledgeFieldNote(string strJsonString);
        [OperationContract]
        string GetFieldNoteAcknowledgementResult(string strJsonString); //Sync the AcknowledgeFieldnote to mobile for report

        //[OperationContract]
        //string UploadTestImage(string FileContent, string filename);


        [OperationContract]
        string UserArchiveFieldNote(string strJsonString);

        [OperationContract]
        string UserArchiveFieldNoteFromWeb(string strUserId);

        //[OperationContract]
        //ClsFieldNotesDownloadAttachment FieldNoteDownloadAttachment(DownloadRequest clsDownloadRequest);

       

        #endregion

        #region MInspection
        [OperationContract]
        void CreateCheckList();
        [OperationContract]
        void EditCheckList();
        [OperationContract]
        void GetAllMyCheckList();
        [OperationContract]
        void CreateMInspection();
        #endregion

        #region OJT
        [OperationContract]
        string CreateOJT(string strJsonOJT);

        [OperationContract]  //Receive Delete the OJT attachment from mobile
        string DeleteAttachOJT(string strJsonOJTAttach);

        [OperationContract] //Sent Delete the OJT attachment from Web
        string GetDeleteAttachOJTFromWeb(string strUserId);

        //Update Existing(Saved) FieldNotes
        [OperationContract]
        string UpdateOJT(string strJsonOJT);

        [OperationContract]
        string GetUpdatedOJTWebToMobile(string strJsonOJT);

        [OperationContract]
        string UpdatedOJTMobileResponse(string strJsonOJT);

        [OperationContract]
        string GetSavedOJTWebToMobile(string strJsonOJT);
        [OperationContract]
        string SavedOJTMobileResponse(string strJsonOJT);

        [OperationContract]
        string DownloadReceivedOJT(string strUserId);
        [OperationContract]
        string ReceivedOJTResponse(string strJsonString);

        [OperationContract]
        string UserAcknowledgeOJT(string strJsonString);

        [OperationContract]
        string GetOJTAcknowledgementResult(string strJsonString); //Sync the AcknowledgeOJT to mobile for report

        [OperationContract]
        string UserArchiveOJT(string strJsonString);

        [OperationContract]
        string UserArchiveOJTFromWeb(string strUserId);

        [OperationContract]
        string GetHARMFromWeb();
       
        #endregion
    }

    #region FieldNotes
    [DataContract]
    public class GeneralResponse
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class CreateFieldNotesResponse
    {
        [DataMember]
        public string FieldNoteID { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }



    [DataContract]
    public class GetFieldNotesResponse 
    {
        [DataMember]
        public FieldNotesBE FieldNote { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class GetSavedFieldNotesResponse
    {
        [DataMember]
        public FieldNotesSavedWebToMobileBE FieldNotes { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class DownloadReceivedFieldNotesResponse
    {
        [DataMember]
        public FieldNotesReceivedWebToMobileBE FieldNote { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class FieldNotesResponseFromMobile
    {
        [DataMember]
        public string FieldNoteID { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class GetFieldNoteAcknowledgementResultResponse
    {
        [DataMember]
        public List<FNDownloadAcknowlegdeStatusBE> AcknowledgedResult { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class UserArchiveFieldNoteFromWebResponse
    {
        [DataMember]
        public List<FieldNoteArchive> ArchiveResult { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class GetDeleteAttachFieldNotesResponse
    {
        [DataMember]
        public List<FieldNotesAttachmentBE> DeletedAttachment { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    //[DataContract]
    //public class FieldNotesResponse
    //{
    //    //[DataMember]
    //    //public List<clsFieldNotes> lstFieldnotes { get; set; }

    //    //[DataMember]
    //    //public List<FieldNotesAttachment> lstAttachments { get; set; }
    //    [DataMember]
    //    public FieldNotesBE FieldNote { get; set; }
    //    [DataMember]
    //    public string StatusCode { get; set; }
    //    [DataMember]
    //    public string StatusMessage { get; set; }
    //}

    
    public class FieldNotesAttachment
    {
        [DataMember]
        public string FieldNoteID { get; set; }
        [DataMember]
        public string FileName { get; set; }
       [DataMember]
        public string FilePath { get; set; }
    }

    //[MessageContract]
    //public class clsFieldNotesAttachment
    //{
    //    [MessageHeader(MustUnderstand = true)]
    //    public string FieldNoteID { get; set; }
    //    [MessageHeader(MustUnderstand = true)]
    //    public string FileName { get; set; }
    //    [MessageBodyMember(Order = 1)]
    //    public Stream Attachments { get; set; }
    //}


    //[MessageContract]
    //public class UploadResponse
    //{
    //    [MessageBodyMember(Order = 1)]
    //    public bool UploadSucceeded { get; set; }
    //}


    [MessageContract]
    public class DownloadRequest
    {
        [MessageBodyMember(Order = 1)]
        public string FilePathname { get; set; }
        public string ActualFilename { get; set; }
    }


    [MessageContract]
    public class ClsFieldNotesDownloadAttachment
    {
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        [MessageBodyMember(Order = 1)]
        public System.IO.Stream FileByteStream;

        public void Dispose()
        {
            if (FileByteStream != null)
            {
                FileByteStream.Close();
                FileByteStream = null;
            }
        }
    }

    #endregion

    #region M-Inspections
    public class clsMInspection
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }
    public class clsCheckList
    {
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }
    #endregion

    #region OJT

    public class GetHARMFromWebResponse
    {
        [DataMember]
        public List<OJTHARM> lstHARM { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class CreateOJTResponse
    {
        [DataMember]
        public string OJTID { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }



    [DataContract]
    public class GetOJTResponse
    {
        [DataMember]
        public OJTBE OJT { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class GetSavedOJTResponse
    {
        [DataMember]
        public OJTSavedWebToMobileBE OJT { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class DownloadReceivedOJTResponse
    {
        [DataMember]
        public OJTReceivedWebToMobileBE OJT { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class OJTResponseFromMobile
    {
        [DataMember]
        public string OJTID { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class GetOJTAcknowledgementResultResponse
    {
        [DataMember]
        public List<OJTDownloadAcknowlegdeStatusBE> AcknowledgedResult { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class UserArchiveOJTFromWebResponse
    {
        [DataMember]
        public List<OJTArchive> ArchiveResult { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    [DataContract]
    public class GetDeleteAttachOJTResponse
    {
        [DataMember]
        public List<OJTAttachmentBE> DeletedAttachment { get; set; }
        [DataMember]
        public string StatusCode { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }

    public class OJTAttachment
    {
        [DataMember]
        public string OJTID { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FilePath { get; set; }
    }


    //[MessageContract]
    //public class DownloadRequest
    //{
    //    [MessageBodyMember(Order = 1)]
    //    public string FilePathname { get; set; }
    //    public string ActualFilename { get; set; }
    //}


    //[MessageContract]
    //public class ClsFieldNotesDownloadAttachment
    //{
    //    [MessageHeader(MustUnderstand = true)]
    //    public string FileName;

    //    [MessageBodyMember(Order = 1)]
    //    public System.IO.Stream FileByteStream;

    //    public void Dispose()
    //    {
    //        if (FileByteStream != null)
    //        {
    //            FileByteStream.Close();
    //            FileByteStream = null;
    //        }
    //    }
    //}
    #endregion
    
}
