﻿  using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.IO;
using CFService.BusinessComponent.BusinessEntry;
using System.Xml;
using System.Xml.Serialization;
using CFService.BusinessComponent;
using System.Configuration;
using System.Data;
using ComplianceFactorService.Utility;


namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CommunicationService" in code, svc and config file together.
    public class CommunicationService : ICommunicationService
    {
        Utility.Utility objUtility = new Utility.Utility();

       
    #region FieldNote

    #region Create Fieldnote
        public string CreateFieldNote(string strJsonFieldNotes)
        {
            CreateFieldNotesResponse objResponse = new CreateFieldNotesResponse();
            try
            {
                FieldNotesBE objFieldnotesBE = new FieldNotesBE();
                objFieldnotesBE = Utility.Utility.JsonDeserialize<FieldNotesBE>(strJsonFieldNotes);

                List<FieldNotesAttachmentBE> lstAttachment = new List<FieldNotesAttachmentBE>();
                List<FieldNotesSentToBE> lstFieldSentTo = new List<FieldNotesSentToBE>();
                List<FieldNotesSentToBE> lstFieldSentTotemp = new List<FieldNotesSentToBE>();
                lstFieldSentTo = objFieldnotesBE.SentTo;
                lstAttachment = objFieldnotesBE.Attachment;
                if (lstFieldSentTo != null)
                {
                    //Get the sentto userid from the username
                    foreach (FieldNotesSentToBE objTempSentTo in lstFieldSentTo)
                    {
                        //encrypt the Username and get userid from db
                        //encrypt and check with db
                        objTempSentTo.SentTo = objUtility.GenerateHashvalue(objTempSentTo.SentTo, true);
                        lstFieldSentTotemp.Add(objTempSentTo);
                    }

                    //Convert the List object To XML string
                    //objFieldnotesBE.SentToXML = ObjectToXML(lstFieldSentTo);
                    objFieldnotesBE.SentToXML = SentToObjectToXML(lstFieldSentTotemp);
                }
                else
                    objFieldnotesBE.SentToXML = null;

                if (lstAttachment != null)
                    objFieldnotesBE.AttachmentXML = AttachmentObjectToXML(lstAttachment);
                else
                    objFieldnotesBE.AttachmentXML = null;

                Logger.WriteToErrorLog("CommunicationService:CreateFieldNote() ", "Inserting " + objFieldnotesBE.FieldNoteID);
                //Insert on Fieldnotes On DB
                string strResult = FieldNotesBLL.CreateFieldNotes(objFieldnotesBE);
                if (strResult == "0")
                {
                    strResult = objFieldnotesBE.FieldNoteID;
                    objResponse.FieldNoteID = objFieldnotesBE.FieldNoteID;
                }
              
               if (strResult == "-1")
               {
                   objResponse.StatusCode = "1";
                   objResponse.StatusMessage = "DB Error";
               }
               else
               {
                   objResponse.StatusCode = "0";
                   objResponse.StatusMessage = "Success";  
               }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:CreateFieldNote() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:CreateFieldNote() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;  
        }

        public string SentToObjectToXML(List<FieldNotesSentToBE> User)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(User.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, User);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        public string AttachmentObjectToXML(List<FieldNotesAttachmentBE> Attach)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(Attach.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, Attach);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

       

      

        //public string InsertAttachFieldNotes(string strJsonFieldNotesAttach) //Need to pass the FieldNote ID and Attachment xml to SP
        //{
        //    GeneralResponse objResponse = new GeneralResponse();
        //     try
        //     {
        //         List<FieldNotesAttachmentBE> lstAttachment = new List<FieldNotesAttachmentBE>();
        //         FieldNotesBE objFieldnotesBE = new FieldNotesBE();
        //         objFieldnotesBE = Utility.Utility.JsonDeserialize<FieldNotesBE>(strJsonFieldNotesAttach);
        //         lstAttachment = objFieldnotesBE.Attachment;

        //         if (lstAttachment != null)
        //             objFieldnotesBE.AttachmentXML = AttachmentObjectToXML(lstAttachment);

        //         string strResult = FieldNotesBLL.InsertAttachFieldNotes(objFieldnotesBE);
        //         if (strResult == "-1")
        //         {
        //             objResponse.StatusCode = "1";
        //             objResponse.StatusMessage = "DB Error";
        //         }
        //         else
        //         {
        //             objResponse.StatusCode = "0";
        //             objResponse.StatusMessage = "Success";
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         if (Logger.LogErrors == true)
        //         {
        //             if (ex.InnerException != null)
        //                 Logger.WriteToErrorLog("CommunicationService:InsertAttachFieldNotes() ", ex.Message, ex.InnerException.Message);
        //             else
        //                 Logger.WriteToErrorLog("CommunicationService:InsertAttachFieldNotes() ", ex.Message);
        //             objResponse.StatusCode = "-3";
        //             objResponse.StatusMessage = ex.Message.ToString();
        //         }
        //     }
        //     string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
        //     return strResponse;  
        //}

        public string DeleteAttachFieldNotes(string strFieldNoteAttachID)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                //Need to delete the file on FTP
                string strResult = FieldNotesBLL.DeleteAttachFieldNotes(strFieldNoteAttachID);
                if (strResult == "-1")
                {
                    objResponse.StatusCode = "1";
                    objResponse.StatusMessage = "DB Error";
                }
                else
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:DeleteAttachFieldNotes() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:DeleteAttachFieldNotes() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;  
        }


        public string GetDeleteAttachFieldNotesFromWeb(string strUserId)
        {
            GetDeleteAttachFieldNotesResponse objResponse = new GetDeleteAttachFieldNotesResponse();
            try
            {
                List<FieldNotesAttachmentBE> lstAttachment = new List<FieldNotesAttachmentBE>();
                lstAttachment = FieldNotesBLL.GetDeleteAttachFieldNotes(strUserId);

                objResponse.StatusCode = "0";
                objResponse.StatusMessage = "Success";
                objResponse.DeletedAttachment = lstAttachment;
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetDeleteAttachFieldNotes() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetDeleteAttachFieldNotes() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;  
        }
        #endregion

    #region Attachment Upload/Download
        //public bool AttachmentUpload(FieldNotesAttachmentBE clsfieldsAtt)
        //{
        //    try
        //    {
        //        //string uploadDirectory = ConfigurationManager.AppSettings["FieldNotesImagePath"];
        //        string uploadDirectory = "D:\\Upload";
        //        string strUploadFilename = Guid.NewGuid().ToString();
        //        string strFileExtension = "." + clsfieldsAtt.FNFileName.ToString().Split('.')[1].ToString();
        //        strUploadFilename = strUploadFilename + strFileExtension;

        //        // Try to create the upload directory if it does not yet exist
        //        if (!Directory.Exists(uploadDirectory))
        //            Directory.CreateDirectory(uploadDirectory);
        //        string path = Path.Combine(uploadDirectory, strUploadFilename);

        //        using (var f = System.IO.File.Create("c:\\file.pdf"))
        //        {
        //            var b = Convert.FromBase64String(clsfieldsAtt.FileContent);
        //            f.Write(b, 0, b.Length);
        //        }
        //        int intDBResult = FieldNotesBLL.CreateFieldNotesAttachment(clsfieldsAtt);
        //        if (intDBResult == 0)
        //            return true;
        //        else
        //            return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public string UploadTestImage(string fileContent, string filename)
        //{
        //    try
        //    {
        //        string uploadDirectory = ConfigurationManager.AppSettings["FieldNotesImagePath"];
        //        //string uploadDirectory = "D:\\Upload";
        //        string strUploadFilename = Guid.NewGuid().ToString();
        //        string strFileExtension = "." + filename.ToString().Split('.')[1].ToString();
        //        strUploadFilename = strUploadFilename + strFileExtension;

        //        // Try to create the upload directory if it does not yet exist
        //        if (!Directory.Exists(uploadDirectory))
        //            Directory.CreateDirectory(uploadDirectory);
        //        string path = Path.Combine(uploadDirectory, strUploadFilename);

        //        using (var f = System.IO.File.Create(path))
        //        {
        //            var b = Convert.FromBase64String(fileContent);
        //            f.Write(b, 0, b.Length);
        //        }
        //      return "true";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "false";
        //    }
        //}

        //public bool FieldNoteAttachments(clsFieldNotesAttachment clsfieldsAtt)
        //{
        //    try
        //    {
        //        //string uploadDirectory = ConfigurationManager.AppSettings["FieldNotesImagePath"];
        //        string uploadDirectory = "D:\\Upload";
        //        string strUploadFilename = Guid.NewGuid().ToString();
        //        string strFileExtension = "." + clsfieldsAtt.FileName.ToString().Split('.')[1].ToString();
        //        strUploadFilename = strUploadFilename + strFileExtension;

        //        // Try to create the upload directory if it does not yet exist
        //        if (!Directory.Exists(uploadDirectory))
        //            Directory.CreateDirectory(uploadDirectory);
        //        //string path = Path.Combine(uploadDirectory, clsfieldsAtt.FileName);
        //        string path = Path.Combine(uploadDirectory, strUploadFilename);

        //        if (File.Exists(path))
        //            File.Delete(path);

        //        // Read the incoming stream and save it to file
        //        const int bufferSize = 10000;
        //        byte[] buffer = new byte[bufferSize];

        //        using (FileStream outputStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        //        {
        //            int bytesRead = clsfieldsAtt.Attachments.Read(buffer, 0, bufferSize);
        //            while (bytesRead > 0)
        //            {
        //                outputStream.Write(buffer, 0, bytesRead);
        //                bytesRead = clsfieldsAtt.Attachments.Read(buffer, 0, bufferSize);
        //            }
        //            outputStream.Close();
        //        }

        //        FieldNotesAttachmentBE objFNAttachementBE = new FieldNotesAttachmentBE();
        //        objFNAttachementBE.FieldNoteID = clsfieldsAtt.FieldNoteID;
        //        objFNAttachementBE.FNFileName = clsfieldsAtt.FileName;
        //        objFNAttachementBE.FilePath = strUploadFilename;
        //        int intDBResult = FieldNotesBLL.CreateFieldNotesAttachment(objFNAttachementBE);

        //        //if (intDBResult == 0)
        //        //{
        //        //    return new UploadResponse { UploadSucceeded = true };
        //        //}
        //        //else
        //        //{
        //        //    return new UploadResponse { UploadSucceeded = false };
        //        //}

        //        return true;

        //    }
        //    catch (Exception ex)
        //    {
        //        //return new UploadResponse
        //        //{
        //        //    UploadSucceeded = false
        //        //};
        //        return false;
        //    }
        //}
        #endregion

    #region Update Fieldnotes
        public string UpdateFieldNote(string strJsonFieldNotes)
        {
            CreateFieldNotesResponse objResponse = new CreateFieldNotesResponse();
            try
            {
                List<FieldNotesAttachmentBE> lstAttachment = new List<FieldNotesAttachmentBE>();
                FieldNotesBE objFieldnotesBE = new FieldNotesBE();
                objFieldnotesBE = Utility.Utility.JsonDeserialize<FieldNotesBE>(strJsonFieldNotes);
                lstAttachment = objFieldnotesBE.Attachment;
                if (lstAttachment != null)
                    objFieldnotesBE.AttachmentXML = AttachmentObjectToXML(lstAttachment);
                else
                    objFieldnotesBE.AttachmentXML = null;
                //List<FieldNotesSentToBE> lstFieldSentTo = new List<FieldNotesSentToBE>();
                //lstFieldSentTo = objFieldnotesBE.SentTo;
                ////Convert the List object To XML string
                //objFieldnotesBE.SentToXML = ObjectToXML(lstFieldSentTo);

                //Insert on Fieldnotes On DB
                string strResult = FieldNotesBLL.UpdateFieldNote(objFieldnotesBE);
                if (strResult == "0")
                {
                    strResult = objFieldnotesBE.FieldNoteID;
                    objResponse.FieldNoteID = objFieldnotesBE.FieldNoteID;
                }

                if (strResult == "-1")
                {
                    objResponse.StatusCode = "1";
                    objResponse.StatusMessage = "DB Error";
                }
                else
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:ModifyFieldNote() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:ModifyFieldNote() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }
        #endregion

    #region Web Saved FN  sent to Mobile
        public string GetSavedFNWebToMobile(string strJsonFieldNotes)
        {
            GetSavedFieldNotesResponse objResponse = new GetSavedFieldNotesResponse();
            try
            {
                FieldNotesBEBase objFieldnotesBEbase = new FieldNotesBEBase();
                FieldNotesSavedWebToMobileBE objFieldnotesBE = new FieldNotesSavedWebToMobileBE();
                objFieldnotesBEbase = Utility.Utility.JsonDeserialize<FieldNotesBEBase>(strJsonFieldNotes);

                objFieldnotesBE = FieldNotesBLL.GetSavedFNWebToMobile(objFieldnotesBEbase);
                if (objFieldnotesBE != null)
                {
                    objResponse.FieldNotes = objFieldnotesBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetSavedFNWebToMobile() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetSavedFNWebToMobile() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }

            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string SavedFNMobileResponse(string strJsonFieldNotes)
        {
            FieldNotesResponseFromMobile objResponseFromMobile = new FieldNotesResponseFromMobile();
            try
            {
                objResponseFromMobile = Utility.Utility.JsonDeserialize<FieldNotesResponseFromMobile>(strJsonFieldNotes);
                if (objResponseFromMobile.StatusCode == "0")
                {
                    FieldNotesBEBase objFNBase = new FieldNotesBEBase();
                    objFNBase.FieldNoteID = objResponseFromMobile.FieldNoteID;
                    objFNBase.CreatedBy = objResponseFromMobile.CreatedBy;
                    string strResult = FieldNotesBLL.SavedFNMobileResponse(objFNBase);
                    if (strResult == "0")
                    {
                        objResponseFromMobile.StatusCode = "0";
                        objResponseFromMobile.StatusMessage = "Success";
                    }
                    else
                    {
                        objResponseFromMobile.StatusCode = "-3";
                        objResponseFromMobile.StatusMessage = "db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:SavedFNMobileResponse() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:SavedFNMobileResponse() ", ex.Message);
                    objResponseFromMobile.StatusCode = "-3";
                    objResponseFromMobile.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponseFromMobile.GetType(), objResponseFromMobile);
            return strResponse;
        }
        #endregion

    #region Web Updated FN  sent to Mobile
        public string GetUpdatedFNWebToMobile(string strJsonFieldNotes)
        {
            //GetFieldNotesResponse objResponse = new GetFieldNotesResponse();
            //try
            //{
            //    FieldNotesBEBase objFieldnotesBEbase = new FieldNotesBEBase();
            //    FieldNotesBE objFieldnotesBE = new FieldNotesBE();
            //    objFieldnotesBEbase = Utility.Utility.JsonDeserialize<FieldNotesBEBase>(strJsonFieldNotes);
            //    objFieldnotesBE = FieldNotesBLL.GetUpdatedFNWebToMobile(objFieldnotesBEbase);
            //    if (objFieldnotesBE != null)
            //    {
            //        objResponse.FieldNote = objFieldnotesBE;
            //        objResponse.StatusCode = "0";
            //        objResponse.StatusMessage = "Success";
            //    }
            //    else
            //    {
            //        objResponse.FieldNote = null;
            //        objResponse.StatusCode = "-1";
            //        objResponse.StatusMessage = "Record Not Found";
            //    }

            GetSavedFieldNotesResponse objResponse = new GetSavedFieldNotesResponse();
            try
            {
                FieldNotesBEBase objFieldnotesBEbase = new FieldNotesBEBase();
                FieldNotesSavedWebToMobileBE objFieldnotesBE = new FieldNotesSavedWebToMobileBE();
                objFieldnotesBEbase = Utility.Utility.JsonDeserialize<FieldNotesBEBase>(strJsonFieldNotes);

                objFieldnotesBE = FieldNotesBLL.GetUpdatedFNWebToMobile(objFieldnotesBEbase);
                if (objFieldnotesBE != null)
                {
                    objResponse.FieldNotes = objFieldnotesBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetUpdatedFNWebToMobile() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetUpdatedFNWebToMobile() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }

            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        public string UpdatedFNMobileResponse(string strJsonFieldNotes)
        {
            FieldNotesResponseFromMobile objResponseFromMobile = new FieldNotesResponseFromMobile();
            try
            {
                objResponseFromMobile = Utility.Utility.JsonDeserialize<FieldNotesResponseFromMobile>(strJsonFieldNotes);
                if (objResponseFromMobile.StatusCode == "0")
                {
                    FieldNotesBEBase objFNBase = new FieldNotesBEBase();
                    objFNBase.FieldNoteID = objResponseFromMobile.FieldNoteID;
                    objFNBase.CreatedBy = objResponseFromMobile.CreatedBy;
                    string strResult = FieldNotesBLL.UpdatedFNMobileResponse(objFNBase);
                    if (strResult == "0")
                    {
                        objResponseFromMobile.StatusCode = "0";
                        objResponseFromMobile.StatusMessage = "Success";
                    }
                    else
                    {
                        objResponseFromMobile.StatusCode = "-3";
                        objResponseFromMobile.StatusMessage = "db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UpdatedFNMobileResponse() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UpdatedFNMobileResponse() ", ex.Message);
                    objResponseFromMobile.StatusCode = "-3";
                    objResponseFromMobile.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponseFromMobile.GetType(), objResponseFromMobile);
            return strResponse;
        }
#endregion

    #region DownloadReceivedFN
        public string DownloadReceivedFieldNote(string strUserId)
        {
            DownloadReceivedFieldNotesResponse objResponse = new DownloadReceivedFieldNotesResponse();
            try
            {
                List<FieldNotesDownloadBE> lstFieldnotesBE = new List<FieldNotesDownloadBE>();
                FieldNotesReceivedWebToMobileBE objFieldnotesReceicvedBE = new FieldNotesReceivedWebToMobileBE();
                objFieldnotesReceicvedBE = FieldNotesBLL.GetReceivedFieldNotes(strUserId);
                if (objFieldnotesReceicvedBE != null)
                {
                    if (objFieldnotesReceicvedBE.LFieldNotes != null)
                    {
                        foreach (FieldNotesDownloadBE obj in objFieldnotesReceicvedBE.LFieldNotes)
                        {
                            string strCreatedByNameEnc = objUtility.Decrypt(obj.CreatedByName, true);
                            obj.CreatedByName = strCreatedByNameEnc;
                            lstFieldnotesBE.Add(obj);
                        }
                        objFieldnotesReceicvedBE.LFieldNotes = lstFieldnotesBE;
                    }
                    objResponse.FieldNote = objFieldnotesReceicvedBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:DownloadReceivedFieldNote() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:DownloadReceivedFieldNote() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string ReceivedFieldNoteResponse(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                FieldNotesSentToBE objFN = new FieldNotesSentToBE();
                objFN = Utility.Utility.JsonDeserialize<FieldNotesSentToBE>(strJsonString);
                if (objFN != null)
                {
                   
                    string strResult = FieldNotesBLL.GetReceivedFieldNotesResponse(objFN);
                    if (strResult == "0")
                    {
                        objResponse.StatusCode = "0";
                        objResponse.StatusMessage = "success";
                    }
                    else
                    {
                        objResponse.StatusCode = strResult;
                        objResponse.StatusMessage = "Db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:ReceivedFieldNoteResponse() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:ReceivedFieldNoteResponse() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

#endregion

    #region UserAcknowledgeFieldNote
        public string UserAcknowledgeFieldNote(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                FieldNotesSentToBE objFN = new FieldNotesSentToBE();
                objFN = Utility.Utility.JsonDeserialize<FieldNotesSentToBE>(strJsonString);
                if (objFN != null)
                {

                    string strResult = FieldNotesBLL.AcknowledgeFieldNote(objFN);
                    if (strResult == "0")
                    {
                        objResponse.StatusCode = "0";
                        objResponse.StatusMessage = "success";
                    }
                    else
                    {
                        objResponse.StatusCode = strResult;
                        objResponse.StatusMessage = "Db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UserAcknowledgeFieldNote() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UserAcknowledgeFieldNote() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        
        public string GetFieldNoteAcknowledgementResult(string strUserId)
        {
            GetFieldNoteAcknowledgementResultResponse objResponse = new GetFieldNoteAcknowledgementResultResponse();
            try
            {
                List<FNDownloadAcknowlegdeStatusBE> lstFieldnotesBE = new List<FNDownloadAcknowlegdeStatusBE>(); 
                lstFieldnotesBE = FieldNotesBLL.AcknowledgeFieldNotesSync(strUserId);
                if (lstFieldnotesBE != null)
                {
                    //FNDownloadAcknowlegdeStatusBE objFieldnotesBE = new FNDownloadAcknowlegdeStatusBE();
                    List<FNDownloadAcknowlegdeStatusBE> lsttempFieldnotesBE = new List<FNDownloadAcknowlegdeStatusBE>();
                    foreach (FNDownloadAcknowlegdeStatusBE objBE in lstFieldnotesBE)
                    {
                        string strCreatedByNameEnc = objUtility.Decrypt(objBE.SentToUserByName, true);
                        objBE.SentToUserByName = strCreatedByNameEnc;
                        lsttempFieldnotesBE.Add(objBE);
                    }
                    objResponse.AcknowledgedResult = lsttempFieldnotesBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
                else
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Records Not Found";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetFieldNoteAcknowledgementResult() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetFieldNoteAcknowledgementResult() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        #endregion

    #region Archive FieldNote
        public string UserArchiveFieldNote(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                FieldNotesSentToBE objFN = new FieldNotesSentToBE();
                objFN = Utility.Utility.JsonDeserialize<FieldNotesSentToBE>(strJsonString);
                if (objFN != null)
                {

                    string strResult = FieldNotesBLL.UserArchiveFieldNote(objFN);
                    if (strResult == "0")
                    {
                        objResponse.StatusCode = "0";
                        objResponse.StatusMessage = "success";
                    }
                    else
                    {
                        objResponse.StatusCode = strResult;
                        objResponse.StatusMessage = "Db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveFieldNote() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveFieldNote() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string UserArchiveFieldNoteFromWeb(string strUserId)
        {
            UserArchiveFieldNoteFromWebResponse objResponse = new UserArchiveFieldNoteFromWebResponse();
            try
            {
                List<FieldNoteArchive> lstFieldnotesBE = new List<FieldNoteArchive>();
                List<FieldNoteArchive> lstFieldnotesBEtemp = new List<FieldNoteArchive>();
                lstFieldnotesBE = FieldNotesBLL.GetUserArchiveFieldNoteFromWeb(strUserId);
                if (lstFieldnotesBE != null)
                {
                    foreach (FieldNoteArchive obj in lstFieldnotesBE)
                    {
                        string strCreatedByNameEnc = objUtility.Decrypt(obj.UserIdByName, true);
                        obj.UserIdByName = strCreatedByNameEnc;
                        lstFieldnotesBEtemp.Add(obj);
                    }
                }
                objResponse.ArchiveResult = lstFieldnotesBEtemp;
                objResponse.StatusCode = "0";
                objResponse.StatusMessage = "Success";
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveFieldNoteFromWeb() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveFieldNoteFromWeb() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


    #endregion


        //public ClsFieldNotesDownloadAttachment FieldNoteDownloadAttachment(DownloadRequest clsDownloadRequest) //(DownloadRequest request)
        //{
        //    ClsFieldNotesDownloadAttachment result = new ClsFieldNotesDownloadAttachment();
        //    try
        //    {
        //        //string uploadDirectory = ConfigurationManager.AppSettings["FieldNotesImagePath"];
        //        string uploadDirectory = "D:\\Upload";
        //        string filename = clsDownloadRequest.FilePathname;
        //        string filePath = System.IO.Path.Combine(uploadDirectory, filename);
        //        System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

        //        System.IO.FileStream stream = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //        result.FileName = clsDownloadRequest.ActualFilename;
        //        result.FileByteStream = stream;
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return result;

        //}

        //public bool UserArchiveFieldNote(ClsFieldNotesSentTo clsfieldnoteSentTo)
        //{
        //    try
        //    {
        //        FieldNotesSentToBE objFieldnotesSentToBE = new FieldNotesSentToBE();
        //        objFieldnotesSentToBE.FieldNoteID = clsfieldnoteSentTo.FieldNoteID;
        //        objFieldnotesSentToBE.SentTo = clsfieldnoteSentTo.SentTo;

        //        int intDBResult = FieldNotesBLL.UserArchiveFieldNote(objFieldnotesSentToBE);

        //        if (intDBResult == -1)
        //            return false;
        //        else
        //            return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        #endregion

        #region MInspection
        #region CheckList
        public void CreateCheckList()
        {
            //to do
        }
        public void EditCheckList()
        {
            //to do
        }
        public void GetAllMyCheckList()
        {
            //to do
        }
        #endregion


        public void CreateMInspection()
        {
            //to do
        }
        #endregion

        #region OJT
        #region Create OJT
        public string CreateOJT(string strJsonOJT)
        {
            CreateOJTResponse objResponse = new CreateOJTResponse();
            try
            {
                OJTBE objOJTBE = new OJTBE();
                objOJTBE = Utility.Utility.JsonDeserialize<OJTBE>(strJsonOJT);

                List<OJTAttachmentBE> lstAttachment = new List<OJTAttachmentBE>();
                List<OJTSentToBE> lstOJTSentTo = new List<OJTSentToBE>();
                List<OJTSentToBE> lstOJTSentTotemp = new List<OJTSentToBE>();
                lstOJTSentTo = objOJTBE.SentTo;
                lstAttachment = objOJTBE.Attachment;
                if (lstOJTSentTo != null)
                {
                    //Get the sentto userid from the username
                    foreach (OJTSentToBE objTempSentTo in lstOJTSentTo)
                    {
                        //encrypt the Username and get userid from db
                        //encrypt and check with db
                        objTempSentTo.SentTo = objUtility.GenerateHashvalue(objTempSentTo.SentTo, true);
                        lstOJTSentTotemp.Add(objTempSentTo);
                    }

                    //Convert the List object To XML string
                    //objFieldnotesBE.SentToXML = ObjectToXML(lstFieldSentTo);
                    objOJTBE.SentToXML = SentToObjectToXML_OJT(lstOJTSentTotemp);
                }
                else
                    objOJTBE.SentToXML = null;

                if (lstAttachment != null)
                    objOJTBE.AttachmentXML = AttachmentObjectToXML_OJT(lstAttachment);
                else
                    objOJTBE.AttachmentXML = null;


                //Insert on Fieldnotes On DB
                string strResult = OJTBLL.CreateOJT(objOJTBE);
                if (strResult == "0")
                {
                    strResult = objOJTBE.OJTID;
                    objResponse.OJTID = objOJTBE.OJTID;
                }

                if (strResult == "-1")
                {
                    objResponse.StatusCode = "1";
                    objResponse.StatusMessage = "DB Error";
                }
                else
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:CreateOJT() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:CreateOJT() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string SentToObjectToXML_OJT(List<OJTSentToBE> User)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(User.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, User);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        public string AttachmentObjectToXML_OJT(List<OJTAttachmentBE> Attach)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(Attach.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, Attach);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc.InnerXml;
            }
        }

        public string DeleteAttachOJT(string strOJTAttachID)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                //Need to delete the file on FTP
                string strResult = OJTBLL.DeleteAttachOJT(strOJTAttachID);
                if (strResult == "-1")
                {
                    objResponse.StatusCode = "1";
                    objResponse.StatusMessage = "DB Error";
                }
                else
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:DeleteAttachOJT() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:DeleteAttachOJT() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        public string GetDeleteAttachOJTFromWeb(string strUserId)
        {
            GetDeleteAttachOJTResponse objResponse = new GetDeleteAttachOJTResponse();
            try
            {
                List<OJTAttachmentBE> lstAttachment = new List<OJTAttachmentBE>();
                lstAttachment = OJTBLL.GetDeleteAttachOJT(strUserId);

                objResponse.StatusCode = "0";
                objResponse.StatusMessage = "Success";
                objResponse.DeletedAttachment = lstAttachment;
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetDeleteAttachOJTFromWeb() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetDeleteAttachOJTFromWeb() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }
        #endregion

        #region Update OJT
        public string UpdateOJT(string strJsonOJT)
        {
            CreateOJTResponse objResponse = new CreateOJTResponse();
            try
            {
                List<OJTAttachmentBE> lstAttachment = new List<OJTAttachmentBE>();
                OJTBE objOJTBE = new OJTBE();
                objOJTBE = Utility.Utility.JsonDeserialize<OJTBE>(strJsonOJT);
                lstAttachment = objOJTBE.Attachment;
                if (lstAttachment != null)
                    objOJTBE.AttachmentXML = AttachmentObjectToXML_OJT(lstAttachment);
                else
                    objOJTBE.AttachmentXML = null;
                //List<FieldNotesSentToBE> lstFieldSentTo = new List<FieldNotesSentToBE>();
                //lstFieldSentTo = objFieldnotesBE.SentTo;
                ////Convert the List object To XML string
                //objFieldnotesBE.SentToXML = ObjectToXML(lstFieldSentTo);

                //Insert on Fieldnotes On DB
                string strResult = OJTBLL.UpdateOJT(objOJTBE);
                if (strResult == "0")
                {
                    strResult = objOJTBE.OJTID;
                    objResponse.OJTID = objOJTBE.OJTID;
                }

                if (strResult == "-1")
                {
                    objResponse.StatusCode = "1";
                    objResponse.StatusMessage = "DB Error";
                }
                else
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:ModifyOJT() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:ModifyOJT() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }
        #endregion

        #region Web Saved OJT  sent to Mobile
        public string GetSavedOJTWebToMobile(string strJsonOJT)
        {
            GetSavedOJTResponse objResponse = new GetSavedOJTResponse();
            try
            {
                OJTBEBase objOJTBEbase = new OJTBEBase();
                OJTSavedWebToMobileBE objOJTBE = new OJTSavedWebToMobileBE();
                objOJTBEbase = Utility.Utility.JsonDeserialize<OJTBEBase>(strJsonOJT);

                objOJTBE = OJTBLL.GetSavedOJTWebToMobile(objOJTBEbase);
                if (objOJTBE != null)
                {
                    objResponse.OJT = objOJTBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetSavedOJTWebToMobile() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetSavedOJTWebToMobile() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }

            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string SavedOJTMobileResponse(string strJsonOJT)
        {
            OJTResponseFromMobile objResponseFromMobile = new OJTResponseFromMobile();
            try
            {
                objResponseFromMobile = Utility.Utility.JsonDeserialize<OJTResponseFromMobile>(strJsonOJT);
                if (objResponseFromMobile.StatusCode == "0")
                {
                    OJTBEBase objOJTBase = new OJTBEBase();
                    objOJTBase.OJTID = objResponseFromMobile.OJTID;
                    objOJTBase.CreatedBy = objResponseFromMobile.CreatedBy;
                    string strResult = OJTBLL.SavedOJTMobileResponse(objOJTBase);
                    if (strResult == "0")
                    {
                        objResponseFromMobile.StatusCode = "0";
                        objResponseFromMobile.StatusMessage = "Success";
                    }
                    else
                    {
                        objResponseFromMobile.StatusCode = "-3";
                        objResponseFromMobile.StatusMessage = "db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:SavedOJTMobileResponse() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:SavedOJTMobileResponse() ", ex.Message);
                    objResponseFromMobile.StatusCode = "-3";
                    objResponseFromMobile.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponseFromMobile.GetType(), objResponseFromMobile);
            return strResponse;
        }
        #endregion

        #region Web Updated OJT  sent to Mobile
        public string GetUpdatedOJTWebToMobile(string strJsonOJT)
        {
            GetSavedOJTResponse objResponse = new GetSavedOJTResponse();
            try
            {
                OJTBEBase objOJTBEbase = new OJTBEBase();
                OJTSavedWebToMobileBE objOJTBE = new OJTSavedWebToMobileBE();
                objOJTBEbase = Utility.Utility.JsonDeserialize<OJTBEBase>(strJsonOJT);

                objOJTBE = OJTBLL.GetUpdatedOJTWebToMobile(objOJTBEbase);
                if (objOJTBE != null)
                {
                    objResponse.OJT = objOJTBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetUpdatedOJTWebToMobile() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetUpdatedOJTWebToMobile() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }

            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        public string UpdatedOJTMobileResponse(string strJsonOJT)
        {
            OJTResponseFromMobile objResponseFromMobile = new OJTResponseFromMobile();
            try
            {
                objResponseFromMobile = Utility.Utility.JsonDeserialize<OJTResponseFromMobile>(strJsonOJT);
                if (objResponseFromMobile.StatusCode == "0")
                {
                    OJTBEBase objOJTBase = new OJTBEBase();
                    objOJTBase.OJTID = objResponseFromMobile.OJTID;
                    objOJTBase.CreatedBy = objResponseFromMobile.CreatedBy;
                    string strResult = OJTBLL.UpdatedOJTMobileResponse(objOJTBase);
                    if (strResult == "0")
                    {
                        objResponseFromMobile.StatusCode = "0";
                        objResponseFromMobile.StatusMessage = "Success";
                    }
                    else
                    {
                        objResponseFromMobile.StatusCode = "-3";
                        objResponseFromMobile.StatusMessage = "db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UpdatedOJTMobileResponse() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UpdatedOJTMobileResponse() ", ex.Message);
                    objResponseFromMobile.StatusCode = "-3";
                    objResponseFromMobile.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponseFromMobile.GetType(), objResponseFromMobile);
            return strResponse;
        }
        #endregion

        #region DownloadReceivedOJT
        public string DownloadReceivedOJT(string strUserId)
        {
            DownloadReceivedOJTResponse objResponse = new DownloadReceivedOJTResponse();
            try
            {
                List<OJTDownloadBE> lstOJTBE = new List<OJTDownloadBE>();
                OJTReceivedWebToMobileBE objOJTReceicvedBE = new OJTReceivedWebToMobileBE();
                objOJTReceicvedBE = OJTBLL.GetReceivedOJT(strUserId);
                if (objOJTReceicvedBE != null)
                {
                    if (objOJTReceicvedBE.LOJT != null)
                    {
                        foreach (OJTDownloadBE obj in objOJTReceicvedBE.LOJT)
                        {
                            string strCreatedByNameEnc = objUtility.Decrypt(obj.CreatedByName, true);
                            obj.CreatedByName = strCreatedByNameEnc;
                            lstOJTBE.Add(obj);
                        }
                        objOJTReceicvedBE.LOJT = lstOJTBE;
                    }
                    objResponse.OJT = objOJTReceicvedBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:DownloadReceivedOJT() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:DownloadReceivedOJT() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string ReceivedOJTResponse(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                OJTSentToBE objOJT = new OJTSentToBE();
                objOJT = Utility.Utility.JsonDeserialize<OJTSentToBE>(strJsonString);
                if (objOJT != null)
                {

                    string strResult = OJTBLL.GetReceivedOJTResponse(objOJT);
                    if (strResult == "0")
                    {
                        objResponse.StatusCode = "0";
                        objResponse.StatusMessage = "success";
                    }
                    else
                    {
                        objResponse.StatusCode = strResult;
                        objResponse.StatusMessage = "Db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:ReceivedOJTResponse() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:ReceivedOJTResponse() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        #endregion

        #region UserAcknowledgeOJT
        public string UserAcknowledgeOJT(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                OJTSentToBE objOJT = new OJTSentToBE();
                objOJT = Utility.Utility.JsonDeserialize<OJTSentToBE>(strJsonString);
                if (objOJT != null)
                {

                    string strResult = OJTBLL.AcknowledgeOJT(objOJT);
                    if (strResult == "0")
                    {
                        objResponse.StatusCode = "0";
                        objResponse.StatusMessage = "success";
                    }
                    else
                    {
                        objResponse.StatusCode = strResult;
                        objResponse.StatusMessage = "Db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UserAcknowledgeOJT() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UserAcknowledgeOJT() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }



        public string GetOJTAcknowledgementResult(string strUserId)
        {
            GetOJTAcknowledgementResultResponse objResponse = new GetOJTAcknowledgementResultResponse();
            try
            {
                List<OJTDownloadAcknowlegdeStatusBE> lstOJTBE = new List<OJTDownloadAcknowlegdeStatusBE>();
                lstOJTBE = OJTBLL.AcknowledgeOJTSync(strUserId);
                if (lstOJTBE != null)
                {
                    //FNDownloadAcknowlegdeStatusBE objFieldnotesBE = new FNDownloadAcknowlegdeStatusBE();
                    List<OJTDownloadAcknowlegdeStatusBE> lsttempOJTBE = new List<OJTDownloadAcknowlegdeStatusBE>();
                    foreach (OJTDownloadAcknowlegdeStatusBE objBE in lstOJTBE)
                    {
                        string strCreatedByNameEnc = objUtility.Decrypt(objBE.SentToUserByName, true);
                        objBE.SentToUserByName = strCreatedByNameEnc;
                        lsttempOJTBE.Add(objBE);
                    }
                    objResponse.AcknowledgedResult = lsttempOJTBE;
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Success";
                }
                else
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "Records Not Found";
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetOJTAcknowledgementResult() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetOJTAcknowledgementResult() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        #endregion

        #region Archive OJT
        public string UserArchiveOJT(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                OJTSentToBE objOJT = new OJTSentToBE();
                objOJT = Utility.Utility.JsonDeserialize<OJTSentToBE>(strJsonString);
                if (objOJT != null)
                {

                    string strResult = OJTBLL.UserArchiveOJT(objOJT);
                    if (strResult == "0")
                    {
                        objResponse.StatusCode = "0";
                        objResponse.StatusMessage = "success";
                    }
                    else
                    {
                        objResponse.StatusCode = strResult;
                        objResponse.StatusMessage = "Db error";
                    }
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveOJT() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveOJT() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string UserArchiveOJTFromWeb(string strUserId)
        {
            UserArchiveOJTFromWebResponse objResponse = new UserArchiveOJTFromWebResponse();
            try
            {
                List<OJTArchive> lstOJTBE = new List<OJTArchive>();
                List<OJTArchive> lstOJTBEtemp = new List<OJTArchive>();
                lstOJTBE = OJTBLL.GetUserArchiveOJTFromWeb(strUserId);
                if (lstOJTBE != null)
                {
                    foreach (OJTArchive obj in lstOJTBE)
                    {
                        string strCreatedByNameEnc = objUtility.Decrypt(obj.UserIdByName, true);
                        obj.UserIdByName = strCreatedByNameEnc;
                        lstOJTBEtemp.Add(obj);
                    }
                }
                objResponse.ArchiveResult = lstOJTBEtemp;
                objResponse.StatusCode = "0";
                objResponse.StatusMessage = "Success";
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveOJTFromWeb() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:UserArchiveOJTFromWeb() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        #endregion


        #region HARM

        public string GetHARMFromWeb()
        {
            GetHARMFromWebResponse objResponse = new GetHARMFromWebResponse();
            try
            {
                List<OJTHARM> lstOJTHARMBE = new List<OJTHARM>();
                lstOJTHARMBE = OJTBLL.GetHARMFromWeb();
                objResponse.lstHARM = lstOJTHARMBE;
                objResponse.StatusCode = "0";
                objResponse.StatusMessage = "Success";
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("CommunicationService:GetHARMFromWeb() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("CommunicationService:GetHARMFromWeb() ", ex.Message);
                    objResponse.StatusCode = "-3";
                    objResponse.StatusMessage = ex.Message.ToString();
                }
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        #endregion
        #endregion
    }
}
