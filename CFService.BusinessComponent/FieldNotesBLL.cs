using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using CFService.BusinessComponent.BusinessEntry;
using ComplicanceFactor.DataAccessLayer;
namespace CFService.BusinessComponent
{
    public class FieldNotesBLL
    {
        public static string CreateFieldNotes(FieldNotesBE clsFieldNotes)
        {
            Hashtable htCreateFN = new Hashtable();
            htCreateFN.Add("@sv_fieldnote_id_pk", clsFieldNotes.FieldNoteID);
            htCreateFN.Add("@sv_fieldnote_created_by_fk", clsFieldNotes.CreatedBy);
            htCreateFN.Add("@sv_fieldnote_title", clsFieldNotes.Title);
            htCreateFN.Add("@sv_fieldnote_description", clsFieldNotes.description);
            htCreateFN.Add("@sv_fieldnote_location", clsFieldNotes.location);
            htCreateFN.Add("@sv_fieldnote_sent_to_user", clsFieldNotes.SentToXML);
            htCreateFN.Add("@sv_fieldnote_id", clsFieldNotes.FielsNoteNO);
            htCreateFN.Add("@sv_fieldnote_attachment", clsFieldNotes.AttachmentXML);
            htCreateFN.Add("@sv_fieldnote_is_acknowledge", clsFieldNotes.IsAcknowledge);
            htCreateFN.Add("@sv_fieldnote_creation_date_time",Convert.ToDateTime(clsFieldNotes.CreatedDate));
            
            try
            {
                DataTable dtResult= DataProxy.FetchDataTable("sv_wcf_insert_fieldnote", htCreateFN);

               return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }


        //Upload and Download attachments
        public static int FieldnoteAttachmentsUpload(string attID,string path)
        {
            Hashtable htFNAttachment = new Hashtable();
            htFNAttachment.Add("@sv_fieldnotes_attachments_id_pk", attID);
            htFNAttachment.Add("@sv_file_path", path);
            try
            {
                int Result = DataProxy.FetchSPOutput("sv_wcf_fieldnote_attachments_upload", htFNAttachment);
                return Result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static string FieldnoteAttachmentsDownload(string attID)
        {
            Hashtable htFNAttachment = new Hashtable();
            htFNAttachment.Add("@sv_fieldnotes_attachments_id_pk", attID);
            try
            {
                string strResult = string.Empty;
                DataTable Result = DataProxy.FetchDataTable("sv_wcf_fieldnote_attachments_Download", htFNAttachment);
                if (Result != null)
                {
                    if (Result.Rows.Count > 0)
                        strResult= Result.Rows[0][0].ToString();
                    
                }
                return strResult;
            }

            catch (Exception)
            {
                throw;
            }
        }

        //public static string InsertAttachFieldNotes(FieldNotesBE clsFieldNotes)
        //{
        //    Hashtable htInsertAttFNFN = new Hashtable();
        //    htInsertAttFNFN.Add("@sv_fieldnote_id_pk", clsFieldNotes.FieldNoteID);
        //    htInsertAttFNFN.Add("@sv_fieldnote_attachment", clsFieldNotes.AttachmentXML);
        //    try
        //    {
        //        DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_insert_fieldnote_Attachment", htInsertAttFNFN);
        //        return dtResult.Rows[0][0].ToString();
        //    }

        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public static string DeleteAttachFieldNotes(string  FieldNoteAttachID)
        {
            Hashtable htDeleteAttFNFN = new Hashtable();
            htDeleteAttFNFN.Add("@sv_fieldnotes_attachments_id_pk", FieldNoteAttachID);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_delete_fieldnote_Attachment", htDeleteAttFNFN);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static List<FieldNotesAttachmentBE> GetDeleteAttachFieldNotes(string strUserId)
        {
            Hashtable htDeleteAttFNFN = new Hashtable();
            htDeleteAttFNFN.Add("@sv_fieldnote_user_id", strUserId);
            try
            {
                FieldNotesAttachmentBE objAttachment = null;
                List<FieldNotesAttachmentBE> lstAttachments = new List<FieldNotesAttachmentBE>();
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_user_delete_fieldnote_attach", htDeleteAttFNFN);
               // return dtResult.Rows[0][0].ToString();
                if (dtResult.Rows.Count>0)
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        objAttachment = new FieldNotesAttachmentBE();
                        objAttachment.FNAttachmentID = dr["sv_fieldnotes_attachments_id_pk"].ToString();
                        objAttachment.FieldNoteID = dr["sv_fieldnotes_id_fk"].ToString();
                        objAttachment.FileType = dr["sv_file_type"].ToString();
                        objAttachment.FNFileName = dr["sv_file_path"].ToString();
                        lstAttachments.Add(objAttachment);
                    }
                }
                return lstAttachments;

            }

            catch (Exception)
            {
                throw;
            }
        }
        
        public static string UpdateFieldNote(FieldNotesBE clsFieldNotes)
        {
            Hashtable htModifyFN = new Hashtable();
            htModifyFN.Add("@sv_fieldnote_id_pk", clsFieldNotes.FieldNoteID);
            htModifyFN.Add("@sv_fieldnote_created_by_fk", clsFieldNotes.CreatedBy);
            htModifyFN.Add("@sv_fieldnote_title", clsFieldNotes.Title);
            htModifyFN.Add("@sv_fieldnote_description", clsFieldNotes.description);
            htModifyFN.Add("@sv_fieldnote_location", clsFieldNotes.location);
            htModifyFN.Add("@sv_fieldnote_sent_to_user", clsFieldNotes.SentToXML);
            htModifyFN.Add("@sv_fieldnote_id", clsFieldNotes.FielsNoteNO);
            htModifyFN.Add("@sv_fieldnote_attachment", clsFieldNotes.AttachmentXML);
            htModifyFN.Add("@sv_fieldnote_is_acknowledge", clsFieldNotes.IsAcknowledge);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_Modify_fieldnote", htModifyFN);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static FieldNotesSavedWebToMobileBE GetUpdatedFNWebToMobile(FieldNotesBEBase clsFieldNotes)
        {
            //try
            //{
            //    FieldNotesBE objReturn = null;
            //    Hashtable htGetFN = new Hashtable();
            //    htGetFN.Add("@sv_fieldnote_user_id", clsFieldNotes.CreatedBy);
            //    DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_user_modify_fieldnote", htGetFN);
            //    if (dtResult.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dtResult.Rows)
            //        {
            //            objReturn = new FieldNotesBE();
            //            objReturn.FieldNoteID = dr["sv_fieldnote_id_pk"].ToString();
            //            objReturn.CreatedBy = dr["sv_fieldnote_created_by_fk"].ToString();
            //            objReturn.Title = dr["sv_fieldnote_title"].ToString();
            //            objReturn.description = dr["sv_fieldnote_description"].ToString();
            //            objReturn.location = dr["sv_fieldnote_location"].ToString();
            //            objReturn.FielsNoteNO = dr["sv_fieldnote_id"].ToString();
            //        }
            //    }
            //    return objReturn;
            //}
            //catch (Exception)
            //{
            //    throw;
            //}

            try
            {
                FieldNotesSavedWebToMobileBE ObjReturn = new FieldNotesSavedWebToMobileBE();
                FieldNotesBE objFieldNotes = null;
                FieldNotesAttachmentBE objAttachment = null;

                List<FieldNotesBE> lstFieldNotes = null;
                List<FieldNotesAttachmentBE> lstAttachments = null;
                Hashtable htGetFN = new Hashtable();
                htGetFN.Add("@sv_fieldnote_user_id", clsFieldNotes.CreatedBy);
                DataSet dsResult = DataProxy.FetchDataSet("sv_wcf_get_user_modify_fieldnote", htGetFN);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lstFieldNotes = new List<FieldNotesBE>();
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            objFieldNotes = new FieldNotesBE();
                            objFieldNotes.FieldNoteID = dr["sv_fieldnote_id_pk"].ToString();
                            objFieldNotes.CreatedBy = dr["sv_fieldnote_created_by_fk"].ToString();
                            objFieldNotes.Title = dr["sv_fieldnote_title"].ToString();
                            objFieldNotes.description = dr["sv_fieldnote_description"].ToString();
                            objFieldNotes.location = dr["sv_fieldnote_location"].ToString();
                            objFieldNotes.FielsNoteNO = dr["sv_fieldnote_id"].ToString();
                            objFieldNotes.IsAcknowledge = Convert.ToBoolean(dr["sv_fieldnote_is_acknowledge"].ToString());
                            objFieldNotes.CreatedDate = dr["sv_fieldnote_creation_date"].ToString();
                            lstFieldNotes.Add(objFieldNotes);
                        }
                    }
                        //Update from web Attach
                        if (dsResult.Tables.Count > 1)
                        {
                            if (dsResult.Tables[1].Rows.Count > 0)
                            {
                                lstAttachments = new List<FieldNotesAttachmentBE>();
                                foreach (DataRow dr in dsResult.Tables[1].Rows)
                                {
                                    objAttachment = new FieldNotesAttachmentBE();
                                    objAttachment.FNAttachmentID = dr["sv_fieldnotes_attachments_id_pk"].ToString();
                                    objAttachment.FieldNoteID = dr["sv_fieldnotes_id_fk"].ToString();
                                    objAttachment.FileType = dr["sv_file_type"].ToString();
                                    objAttachment.FNFileName = dr["sv_file_name"].ToString();
                                    lstAttachments.Add(objAttachment);
                                }
                            }
                        }
                    
                    ObjReturn.LFieldNotes = lstFieldNotes;
                    ObjReturn.LFieldNoteAttachments = lstAttachments;
                }
                return ObjReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string UpdatedFNMobileResponse(FieldNotesBEBase clsFieldNotes)
        {
            Hashtable htChangeStatusUpdateFN = new Hashtable();
            htChangeStatusUpdateFN.Add("@sv_fieldnote_id", clsFieldNotes.FieldNoteID);
            htChangeStatusUpdateFN.Add("@sv_fieldnote_created_by_fk", clsFieldNotes.CreatedBy);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_fieldnote_modify_sync_mobile", htChangeStatusUpdateFN);

                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }


        public static FieldNotesSavedWebToMobileBE GetSavedFNWebToMobile(FieldNotesBEBase clsFieldNotes)
        {
            try
            {
                FieldNotesSavedWebToMobileBE ObjReturn = new FieldNotesSavedWebToMobileBE();
                FieldNotesBE objFieldNotes = null;
                FieldNotesAttachmentBE objAttachment = null;
                
                List<FieldNotesBE> lstFieldNotes = null;
                List<FieldNotesAttachmentBE> lstAttachments = null;
                Hashtable htGetFN = new Hashtable();
                htGetFN.Add("@sv_fieldnote_user_id", clsFieldNotes.CreatedBy);
                DataSet dsResult = DataProxy.FetchDataSet("sv_wcf_get_user_saved_fieldnote", htGetFN);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lstFieldNotes = new List<FieldNotesBE>();
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            objFieldNotes = new FieldNotesBE();
                            objFieldNotes.FieldNoteID = dr["sv_fieldnote_id_pk"].ToString();
                            objFieldNotes.CreatedBy = dr["sv_fieldnote_created_by_fk"].ToString();
                            objFieldNotes.Title = dr["sv_fieldnote_title"].ToString();
                            objFieldNotes.description = dr["sv_fieldnote_description"].ToString();
                            objFieldNotes.location = dr["sv_fieldnote_location"].ToString();
                            objFieldNotes.FielsNoteNO = dr["sv_fieldnote_id"].ToString();
                            objFieldNotes.IsAcknowledge =Convert.ToBoolean(dr["sv_fieldnote_is_acknowledge"].ToString());
                            objFieldNotes.CreatedDate = dr["sv_fieldnote_creation_date"].ToString();
                            lstFieldNotes.Add(objFieldNotes);
                        }

                        if (dsResult.Tables.Count > 1)
                        {
                            lstAttachments = new List<FieldNotesAttachmentBE>();
                            foreach (DataRow dr in dsResult.Tables[1].Rows)
                            {
                                objAttachment = new FieldNotesAttachmentBE();
                                objAttachment.FNAttachmentID = dr["sv_fieldnotes_attachments_id_pk"].ToString();
                                objAttachment.FieldNoteID = dr["sv_fieldnotes_id_fk"].ToString();
                                objAttachment.FileType = dr["sv_file_type"].ToString();
                                objAttachment.FNFileName = dr["sv_file_name"].ToString();
                                lstAttachments.Add(objAttachment);
                            }
                        }
                    }
                    ObjReturn.LFieldNotes = lstFieldNotes;
                    ObjReturn.LFieldNoteAttachments = lstAttachments;
                }
                return ObjReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string SavedFNMobileResponse(FieldNotesBEBase clsFieldNotes)
        {
            Hashtable htChangeStatusSavedFN = new Hashtable();
            htChangeStatusSavedFN.Add("@sv_fieldnote_id", clsFieldNotes.FieldNoteID);
            htChangeStatusSavedFN.Add("@sv_fieldnote_created_by_fk", clsFieldNotes.CreatedBy);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_fieldnote_saved_sync_mobile", htChangeStatusSavedFN);

                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        //public static int RollBackFieldNotesCreation(string strFieldnoteId)
        //{
        //    Hashtable htFNRollBack = new Hashtable();
        //    htFNRollBack.Add("@sv_fieldnotes_id", strFieldnoteId);
        //    try
        //    {
        //        int Result = DataProxy.FetchSPOutput("sv_wcf_rollback_fieldnote", htFNRollBack);
        //        return Result;
        //    }

        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public static FieldNotesReceivedWebToMobileBE GetReceivedFieldNotes(string strUserID)
        {
            try
            {
                FieldNotesReceivedWebToMobileBE objReturn = new FieldNotesReceivedWebToMobileBE();
                FieldNotesDownloadBE objFieldNote = null;
                FieldNotesAttachmentBE objAttachment = null;
                List<FieldNotesDownloadBE> lstFieldNotes = null;
                List<FieldNotesAttachmentBE> lstAttachments = null;
                Hashtable htGetUserFieldNote = new Hashtable();
                htGetUserFieldNote.Add("@sv_fieldnote_user_id", strUserID);
                DataSet dsResult = DataProxy.FetchDataSet("sv_wcf_get_received_fieldnote", htGetUserFieldNote);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lstFieldNotes = new List<FieldNotesDownloadBE>();
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            objFieldNote = new FieldNotesDownloadBE();
                            objFieldNote.FieldNoteID = dr["sv_fieldnote_id_pk"].ToString();
                            objFieldNote.CreatedBy = dr["sv_fieldnote_created_by_fk"].ToString();
                            objFieldNote.CreatedByName = dr["u_username_enc"].ToString();
                            objFieldNote.Title = dr["sv_fieldnote_title"].ToString();
                            objFieldNote.description = dr["sv_fieldnote_description"].ToString();
                            objFieldNote.location = dr["sv_fieldnote_location"].ToString();
                            objFieldNote.FielsNoteNO = dr["sv_fieldnote_id"].ToString();
                            objFieldNote.IsAcknowledge = Convert.ToBoolean(dr["sv_fieldnote_is_need_acknowledged"]);
                            objFieldNote.CreatedDate = dr["sv_fieldnote_creation_date"].ToString();
                            lstFieldNotes.Add(objFieldNote);
                        }
                    }
                    if (dsResult.Tables.Count > 1)
                    {
                        if (dsResult.Tables[1].Rows.Count > 0)
                        {
                            lstAttachments = new List<FieldNotesAttachmentBE>();
                            foreach (DataRow dr in dsResult.Tables[1].Rows)
                            {
                                objAttachment = new FieldNotesAttachmentBE();
                                objAttachment.FNAttachmentID = dr["sv_fieldnotes_attachments_id_pk"].ToString();
                                objAttachment.FieldNoteID = dr["sv_fieldnotes_id_fk"].ToString();
                                objAttachment.FileType = dr["sv_file_type"].ToString();
                                objAttachment.FNFileName = dr["sv_file_name"].ToString();
                                lstAttachments.Add(objAttachment);
                            }
                        }
                    }
                    
                    objReturn.LFieldNotes = lstFieldNotes;
                    objReturn.LFieldNoteAttachments = lstAttachments;
                }
                return objReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static string GetReceivedFieldNotesResponse(FieldNotesSentToBE clsFieldNotes)
        {
            try
            {
                Hashtable htChangeStatusReceivedFN = new Hashtable();
                htChangeStatusReceivedFN.Add("@sv_fieldnote_id", clsFieldNotes.FieldNoteID);
                htChangeStatusReceivedFN.Add("@sv_fieldnote_user_id", clsFieldNotes.SentTo);
                try
                {
                    DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_received_fieldnote_sync_mobile", htChangeStatusReceivedFN);

                    return dtResult.Rows[0][0].ToString();
                }

                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string AcknowledgeFieldNote(FieldNotesSentToBE clsFieldNotesSentTo)
        {
            Hashtable htAcknowledgeFieldNote = new Hashtable();
            htAcknowledgeFieldNote.Add("@sv_fieldnote_sent_to_user", clsFieldNotesSentTo.SentTo);
            htAcknowledgeFieldNote.Add("@sv_fieldnote_id", clsFieldNotesSentTo.FieldNoteID);
            htAcknowledgeFieldNote.Add("@sv_fieldnote_acknowledge_date", clsFieldNotesSentTo.AcknowledgeDate);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_user_acknowledge_fieldnote", htAcknowledgeFieldNote);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static List<FNDownloadAcknowlegdeStatusBE> AcknowledgeFieldNotesSync(string strUserID)
        {
            try
            {
                FNDownloadAcknowlegdeStatusBE objReturn = null;
                List<FNDownloadAcknowlegdeStatusBE> lstReturn = null;
                Hashtable htFieldNote = new Hashtable();
                htFieldNote.Add("@sv_fieldnote_created_by_fk", strUserID);
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_user_acknowledge_fieldnote_sync", htFieldNote);
                if (dtResult.Rows.Count > 0)
                {
                    lstReturn = new List<FNDownloadAcknowlegdeStatusBE>();
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        objReturn = new FNDownloadAcknowlegdeStatusBE();
                        objReturn.FieldNoteID = dr["sv_fieldnote_id_pk"].ToString();
                        objReturn.SentToUser = dr["sv_fieldnote_sent_to_user_fk"].ToString();
                        objReturn.SentToUserByName = dr["u_username_enc"].ToString();
                        objReturn.AcknowledgeDate = Convert.ToString(dr["sv_fieldnote_acknowledge_date"]);
                       // objReturn.IsAcknowledge = Convert.ToBoolean(dr["sv_fieldnote_is_need_acknowledged"]);
                        lstReturn.Add(objReturn);
                    }
                }
                return lstReturn;


            }

            catch (Exception)
            {
                throw;
            }
        }


        public static string UserArchiveFieldNote(FieldNotesSentToBE clsFieldNotesSentTo)
        {
            Hashtable htArchiveFieldNote = new Hashtable();
            htArchiveFieldNote.Add("@sv_fieldnote_user_id", clsFieldNotesSentTo.SentTo);
            htArchiveFieldNote.Add("@sv_fieldnote_id", clsFieldNotesSentTo.FieldNoteID);
            try
            {
                //int Result = DataProxy.FetchSPOutput("sv_wcf_archive_fieldnote", htArchiveFieldNote);
                //return Result;

                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_archive_fieldnote", htArchiveFieldNote);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static List<FieldNoteArchive> GetUserArchiveFieldNoteFromWeb(string strUserId)
        {
            Hashtable htArchiveFieldNote = new Hashtable();
            htArchiveFieldNote.Add("@sv_fieldnote_user_id", strUserId);
            try
            {
                List<FieldNoteArchive> lstReturn = null;
                FieldNoteArchive objReturn = null;
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_web_archive_fieldnote", htArchiveFieldNote);
                if (dtResult.Rows.Count > 0)
                {
                    lstReturn = new List<FieldNoteArchive>();
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        objReturn = new FieldNoteArchive();
                        objReturn.FieldNoteID = dr["Fieldnote_Id"].ToString();
                        objReturn.UserId = dr["UserID"].ToString();
                        objReturn.UserIdByName = dr["UserName"].ToString();
                        objReturn.FieldNoteType = dr["FieldnoteType"].ToString();
                        lstReturn.Add(objReturn);
                    }
                }
                return lstReturn;
            }

            catch (Exception)
            {
                throw;
            }
        }

    }
}
