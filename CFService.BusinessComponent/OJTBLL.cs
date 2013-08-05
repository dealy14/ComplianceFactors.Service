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
    public class OJTBLL
    {


        public static string CreateOJT(OJTBE clsOJT)
        {
            Hashtable htCreateOJT = new Hashtable();
            htCreateOJT.Add("@sv_ojt_id_pk", clsOJT.OJTID);
            htCreateOJT.Add("@sv_ojt_created_by_fk", clsOJT.CreatedBy);
            htCreateOJT.Add("@sv_ojt_number", clsOJT.OJTNumber);
            htCreateOJT.Add("@sv_ojt_title", clsOJT.Title);
            htCreateOJT.Add("@sv_ojt_description", clsOJT.description);
            htCreateOJT.Add("@sv_ojt_location", clsOJT.location);
            htCreateOJT.Add("@sv_ojt_Trainer", clsOJT.Trainer);
            htCreateOJT.Add("@sv_ojt_date", clsOJT.Date);
            htCreateOJT.Add("@sv_ojt_start_time", clsOJT.StartTime);
            htCreateOJT.Add("@sv_ojt_end_time", clsOJT.EndTime);
            htCreateOJT.Add("@sv_ojt_Duration", clsOJT.Duration);
            htCreateOJT.Add("@sv_ojt_Type", clsOJT.Type);
            htCreateOJT.Add("@sv_ojt_issafty_brief", clsOJT.IsSaftyBrief);
            htCreateOJT.Add("@sv_ojt_frequency", clsOJT.frequency);
            htCreateOJT.Add("@sv_ojt_isharm_related", clsOJT.IsHarmRelated);
            htCreateOJT.Add("@sv_ojt_harm_id_fk", clsOJT.HarmId);
            //htCreateOJT.Add("@sv_ojt_harm_number", clsOJT.HarmNumber);
            htCreateOJT.Add("@sv_ojt_iscertification_related", clsOJT.isCertifiedRelated);
            htCreateOJT.Add("@sv_ojt_other", clsOJT.Other);
            htCreateOJT.Add("@sv_ojt_sent_to_user", clsOJT.SentToXML);
            htCreateOJT.Add("@sv_ojt_attachment", clsOJT.AttachmentXML);
            htCreateOJT.Add("@sv_ojt_is_acknowledge", clsOJT.IsAcknowledge);
            htCreateOJT.Add("@sv_ojt_certify_filename", clsOJT.CertifiedFileName);
            htCreateOJT.Add("@sv_ojt_certify_fileExt", clsOJT.CertifiedFileExt);
            
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_insert_OJT", htCreateOJT);

                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        //Upload and Download OJT Certificate file
        public static int OJTCertificateUpload(string OJTID, string path)
        {
            Hashtable htOJT = new Hashtable();
            htOJT.Add("@sv_ojt_id_pk", OJTID);
            htOJT.Add("@sv_file_path", path);
            try
            {
                int Result = DataProxy.FetchSPOutput("sv_wcf_ojt_Certificate_upload", htOJT);
                return Result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static string OJTCertificateDownload(string OJTID)
        {
            Hashtable htOJT = new Hashtable();
            htOJT.Add("@sv_ojt_id_pk", OJTID);
            try
            {
                string strResult = string.Empty;
                DataTable Result = DataProxy.FetchDataTable("sv_wcf_ojt_certificate_Download", htOJT);
                if (Result != null)
                {
                    if (Result.Rows.Count > 0)
                        strResult = Result.Rows[0][0].ToString();

                }
                return strResult;
            }

            catch (Exception)
            {
                throw;
            }
        }



        //Upload and Download attachments
        public static int OJTAttachmentsUpload(string attID, string path)
        {
            Hashtable htOJTAttachment = new Hashtable();
            htOJTAttachment.Add("@sv_ojt_attachments_id_pk", attID);
            htOJTAttachment.Add("@sv_file_path", path);
            try
            {
                int Result = DataProxy.FetchSPOutput("sv_wcf_ojt_attachments_upload", htOJTAttachment);
                return Result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static string OJTAttachmentsDownload(string attID)
        {
            Hashtable htOJTAttachment = new Hashtable();
            htOJTAttachment.Add("@sv_ojt_attachments_id_pk", attID);
            try
            {
                string strResult = string.Empty;
                DataTable Result = DataProxy.FetchDataTable("sv_wcf_ojt_attachments_Download", htOJTAttachment);
                if (Result != null)
                {
                    if (Result.Rows.Count > 0)
                        strResult = Result.Rows[0][0].ToString();

                }
                return strResult;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static string DeleteAttachOJT(string OJTAttachID)
        {
            Hashtable htDeleteAtt = new Hashtable();
            htDeleteAtt.Add("@sv_ojt_attachments_id_pk", OJTAttachID);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_delete_ojt_Attachment", htDeleteAtt);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static List<OJTAttachmentBE> GetDeleteAttachOJT(string strUserId)
        {
            Hashtable htDeleteAtt = new Hashtable();
            htDeleteAtt.Add("@sv_ojt_user_id", strUserId);
            try
            {
                OJTAttachmentBE objAttachment = null;
                List<OJTAttachmentBE> lstAttachments = new List<OJTAttachmentBE>();
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_user_delete_ojt_attach", htDeleteAtt);
                // return dtResult.Rows[0][0].ToString();
                if (dtResult.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        objAttachment = new OJTAttachmentBE();
                        objAttachment.AttachmentID = dr["sv_ojt_attachments_id_pk"].ToString();
                        objAttachment.OJTID = dr["sv_ojt_id_fk"].ToString();
                        objAttachment.FileType = dr["sv_file_type"].ToString();
                        objAttachment.OJTFileName = dr["sv_file_path"].ToString();
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

        public static string UpdateOJT(OJTBE clsOJT)
        {
            Hashtable htModify = new Hashtable();
            htModify.Add("@sv_ojt_id_pk", clsOJT.OJTID);
            htModify.Add("@sv_ojt_created_by_fk", clsOJT.CreatedBy);
            htModify.Add("@sv_ojt_number", clsOJT.OJTNumber);
            htModify.Add("@sv_ojt_title", clsOJT.Title);
            htModify.Add("@sv_ojt_description", clsOJT.description);
            htModify.Add("@sv_ojt_location", clsOJT.location);
            htModify.Add("@sv_ojt_Trainer", clsOJT.Trainer);
            htModify.Add("@sv_ojt_date", clsOJT.Date);
            htModify.Add("@sv_ojt_start_time", clsOJT.StartTime);
            htModify.Add("@sv_ojt_end_time", clsOJT.EndTime);
            htModify.Add("@sv_ojt_Duration", clsOJT.Duration);
            htModify.Add("@sv_ojt_Type", clsOJT.Type);
            htModify.Add("@sv_ojt_issafty_brief", clsOJT.IsSaftyBrief);
            htModify.Add("@sv_ojt_frequency", clsOJT.frequency);
            htModify.Add("@sv_ojt_isharm_related", clsOJT.IsHarmRelated);
            htModify.Add("@sv_ojt_harm_id_fk", clsOJT.HarmId);
            //htModify.Add("@sv_ojt_harm_title", clsOJT.HarmTitle);
            //htModify.Add("@sv_ojt_harm_number", clsOJT.HarmNumber);
            htModify.Add("@sv_ojt_iscertification_related", clsOJT.isCertifiedRelated);
            htModify.Add("@sv_ojt_other", clsOJT.Other);
            //htCreateOJT.Add("@sv_ojt_sent_to_user", clsOJT.SentToXML);
            htModify.Add("@sv_ojt_attachment", clsOJT.AttachmentXML);
            htModify.Add("@sv_ojt_is_acknowledge", clsOJT.IsAcknowledge);
            htModify.Add("@sv_ojt_certify_filename", clsOJT.CertifiedFileName);
            htModify.Add("@sv_ojt_certify_fileExt", clsOJT.CertifiedFileExt);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_Modify_ojt", htModify);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static OJTSavedWebToMobileBE GetUpdatedOJTWebToMobile(OJTBEBase clsOJT)
        {
            try
            {
                OJTSavedWebToMobileBE ObjReturn = new OJTSavedWebToMobileBE();
                OJTBE objOJT = null;
                OJTAttachmentBE objAttachment = null;

                List<OJTBE> lstOJT = null;
                List<OJTAttachmentBE> lstAttachments = null;
                Hashtable htGetOJT = new Hashtable();
                htGetOJT.Add("@sv_ojt_user_id", clsOJT.CreatedBy);
                DataSet dsResult = DataProxy.FetchDataSet("sv_wcf_get_user_modify_ojt", htGetOJT);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lstOJT = new List<OJTBE>();
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            objOJT = new OJTBE();
                            objOJT.OJTID = dr["sv_ojt_id_pk"].ToString();
                            objOJT.CreatedBy = dr["sv_ojt_created_by_fk"].ToString();
                            objOJT.OJTNumber = dr["sv_ojt_number"].ToString();
                            objOJT.Title = dr["sv_ojt_title"].ToString();
                            objOJT.description = dr["sv_ojt_description"].ToString();
                            objOJT.location = dr["sv_ojt_location"].ToString();
                            objOJT.Trainer = dr["sv_ojt_Trainer"].ToString();
                            objOJT.Date = dr["sv_ojt_date"].ToString();
                            objOJT.StartTime = dr["sv_ojt_start_time"].ToString();
                            objOJT.EndTime = dr["sv_ojt_end_time"].ToString();
                            objOJT.Duration = dr["sv_ojt_duration"].ToString();
                            objOJT.Type = dr["sv_ojt_type"].ToString();
                            objOJT.IsSaftyBrief = Convert.ToBoolean(dr["sv_ojt_issafty_brief"].ToString());
                            objOJT.frequency = dr["sv_ojt_frequency"].ToString();
                            objOJT.IsHarmRelated = Convert.ToBoolean(dr["sv_ojt_isharm_related"].ToString());
                            objOJT.HarmId = dr["sv_ojt_harm_id_fk"].ToString();
                            //objOJT.HarmTitle = dr["sv_ojt_harm_title"].ToString();
                            //objOJT.HarmNumber = dr["sv_ojt_harm_number"].ToString();
                            objOJT.isCertifiedRelated = Convert.ToBoolean(dr["sv_ojt_iscertification_related"].ToString());
                            objOJT.CertifiedFilePath = dr["sv_ojt_certify_filepath"].ToString();
                            objOJT.CertifiedFileName = dr["sv_ojt_certify_filename"].ToString();
                            objOJT.CertifiedFileExt = dr["sv_ojt_certify_fileExt"].ToString();
                            objOJT.Other = dr["sv_ojt_other"].ToString();
                            objOJT.CreatedDate = dr["sv_ojt_creation_date_time"].ToString();
                            objOJT.IsAcknowledge = Convert.ToBoolean(dr["sv_ojt_is_acknowledge"]);
                            lstOJT.Add(objOJT);
                        }
                    }
                    //Update from web Attach
                    if (dsResult.Tables.Count > 1)
                    {
                        if (dsResult.Tables[1].Rows.Count > 0)
                        {
                            lstAttachments = new List<OJTAttachmentBE>();
                            foreach (DataRow dr in dsResult.Tables[1].Rows)
                            {
                                objAttachment = new OJTAttachmentBE();
                                objAttachment.AttachmentID = dr["sv_ojt_attachments_id_pk"].ToString();
                                objAttachment.OJTID = dr["sv_ojt_id_fk"].ToString();
                                objAttachment.FileType = dr["sv_file_type"].ToString();
                                objAttachment.OJTFileName = dr["sv_file_name"].ToString();
                                lstAttachments.Add(objAttachment);
                            }
                        }
                    }

                    ObjReturn.LOJT = lstOJT;
                    ObjReturn.LOJTAttachments = lstAttachments;
                }
                return ObjReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string UpdatedOJTMobileResponse(OJTBEBase clsOJT)
        {
            Hashtable htChangeStatusUpdateOJT = new Hashtable();
            htChangeStatusUpdateOJT.Add("@sv_ojt_id", clsOJT.OJTID);
            htChangeStatusUpdateOJT.Add("@sv_ojt_created_by_fk", clsOJT.CreatedBy);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_ojt_modify_sync_mobile", htChangeStatusUpdateOJT);

                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }


        public static OJTSavedWebToMobileBE GetSavedOJTWebToMobile(OJTBEBase clsOJT)
        {
            try
            {
                OJTSavedWebToMobileBE ObjReturn = new OJTSavedWebToMobileBE();
                OJTBE objOJT = null;
                OJTAttachmentBE objAttachment = null;

                List<OJTBE> lstOJT = null;
                List<OJTAttachmentBE> lstAttachments = null;
                Hashtable htGetOJT = new Hashtable();
                htGetOJT.Add("@sv_ojt_user_id", clsOJT.CreatedBy);
                DataSet dsResult = DataProxy.FetchDataSet("sv_wcf_get_user_saved_ojt", htGetOJT);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lstOJT = new List<OJTBE>();
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            objOJT = new OJTBE();
                            objOJT.OJTID = dr["sv_ojt_id_pk"].ToString();
                            objOJT.CreatedBy = dr["sv_ojt_created_by_fk"].ToString();
                            objOJT.OJTNumber = dr["sv_ojt_number"].ToString();
                            objOJT.Title = dr["sv_ojt_title"].ToString();
                            objOJT.description = dr["sv_ojt_description"].ToString();
                            objOJT.location = dr["sv_ojt_location"].ToString();
                            objOJT.Trainer = dr["sv_ojt_Trainer"].ToString();
                            objOJT.Date = dr["sv_ojt_date"].ToString();
                            objOJT.StartTime = dr["sv_ojt_start_time"].ToString();
                            objOJT.EndTime = dr["sv_ojt_end_time"].ToString();
                            objOJT.Duration = dr["sv_ojt_duration"].ToString();
                            objOJT.Type = dr["sv_ojt_type"].ToString();
                            objOJT.IsSaftyBrief = Convert.ToBoolean(dr["sv_ojt_issafty_brief"].ToString());
                            objOJT.frequency = dr["sv_ojt_frequency"].ToString();
                            objOJT.IsHarmRelated = Convert.ToBoolean(dr["sv_ojt_isharm_related"].ToString());
                            objOJT.HarmId = dr["sv_ojt_harm_id_fk"].ToString();
                            //objOJT.HarmTitle = dr["sv_ojt_harm_title"].ToString();
                            //objOJT.HarmNumber = dr["sv_ojt_harm_number"].ToString();
                            objOJT.isCertifiedRelated = Convert.ToBoolean(dr["sv_ojt_iscertification_related"].ToString());
                            objOJT.CertifiedFilePath = dr["sv_ojt_certify_filepath"].ToString();
                            objOJT.CertifiedFileName = dr["sv_ojt_certify_filename"].ToString();
                            objOJT.CertifiedFileExt = dr["sv_ojt_certify_fileExt"].ToString();
                            objOJT.Other = dr["sv_ojt_other"].ToString();
                            objOJT.CreatedDate = dr["sv_ojt_creation_date_time"].ToString();
                            objOJT.IsAcknowledge =Convert.ToBoolean(dr["sv_ojt_is_acknowledge"]);
                            lstOJT.Add(objOJT);
                        }

                        if (dsResult.Tables.Count > 1)
                        {
                            lstAttachments = new List<OJTAttachmentBE>();
                            foreach (DataRow dr in dsResult.Tables[1].Rows)
                            {
                                objAttachment = new OJTAttachmentBE();
                                objAttachment.AttachmentID = dr["sv_ojt_attachments_id_pk"].ToString();
                                objAttachment.OJTID = dr["sv_ojt_id_fk"].ToString();
                                objAttachment.FileType = dr["sv_file_type"].ToString();
                                objAttachment.OJTFileName = dr["sv_file_name"].ToString();
                                lstAttachments.Add(objAttachment);
                            }
                        }
                    }
                    ObjReturn.LOJT = lstOJT;
                    ObjReturn.LOJTAttachments = lstAttachments;
                }
                return ObjReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string SavedOJTMobileResponse(OJTBEBase clsOJT)
        {
            Hashtable htChangeStatusSavedOJT = new Hashtable();
            htChangeStatusSavedOJT.Add("@sv_ojt_id", clsOJT.OJTID);
            htChangeStatusSavedOJT.Add("@sv_ojt_created_by_fk", clsOJT.CreatedBy);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_ojt_saved_sync_mobile", htChangeStatusSavedOJT);

                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static OJTReceivedWebToMobileBE GetReceivedOJT(string strUserID)
        {
            try
            {
                OJTReceivedWebToMobileBE objReturn = new OJTReceivedWebToMobileBE();
                OJTDownloadBE objOJT = null;
                OJTAttachmentBE objAttachment = null;
                List<OJTDownloadBE> lstOJT = null;
                List<OJTAttachmentBE> lstAttachments = null;
                Hashtable htGetUserOJT = new Hashtable();
                htGetUserOJT.Add("@sv_ojt_user_id", strUserID);
                DataSet dsResult = DataProxy.FetchDataSet("sv_wcf_get_received_ojt", htGetUserOJT);
                if (dsResult != null)
                {
                    if (dsResult.Tables[0].Rows.Count > 0)
                    {
                        lstOJT = new List<OJTDownloadBE>();
                        foreach (DataRow dr in dsResult.Tables[0].Rows)
                        {
                            objOJT = new OJTDownloadBE();
                            objOJT.OJTID = dr["sv_ojt_id_pk"].ToString();
                            objOJT.CreatedBy = dr["sv_ojt_created_by_fk"].ToString();
                            objOJT.CreatedByName = dr["u_username_enc"].ToString();
                            objOJT.OJTNumber = dr["sv_ojt_number"].ToString();
                            objOJT.Title = dr["sv_ojt_title"].ToString();
                            objOJT.description = dr["sv_ojt_description"].ToString();
                            objOJT.location = dr["sv_ojt_location"].ToString();
                            objOJT.Trainer = dr["sv_ojt_Trainer"].ToString();
                            objOJT.Date = dr["sv_ojt_date"].ToString();
                            objOJT.StartTime = dr["sv_ojt_start_time"].ToString();
                            objOJT.EndTime = dr["sv_ojt_end_time"].ToString();
                            objOJT.Duration = dr["sv_ojt_duration"].ToString();
                            objOJT.Type = dr["sv_ojt_type"].ToString();
                            objOJT.IsSaftyBrief = Convert.ToBoolean(dr["sv_ojt_issafty_brief"].ToString());
                            objOJT.frequency = dr["sv_ojt_frequency"].ToString();
                            objOJT.IsHarmRelated = Convert.ToBoolean(dr["sv_ojt_isharm_related"].ToString());
                            objOJT.HarmId = dr["sv_ojt_harm_id_fk"].ToString();
                            //objOJT.HarmTitle = dr["sv_ojt_harm_title"].ToString();
                            //objOJT.HarmNumber = dr["sv_ojt_harm_number"].ToString();
                            objOJT.isCertifiedRelated = Convert.ToBoolean(dr["sv_ojt_iscertification_related"].ToString());
                            objOJT.CertifiedFilePath = dr["sv_ojt_certify_filepath"].ToString();
                            objOJT.CertifiedFileName = dr["sv_ojt_certify_filename"].ToString();
                            objOJT.CertifiedFileExt = dr["sv_ojt_certify_fileExt"].ToString();
                            objOJT.Other = dr["sv_ojt_other"].ToString();
                            objOJT.CreatedDate = dr["sv_ojt_creation_date_time"].ToString();
                            objOJT.IsAcknowledge = Convert.ToBoolean(dr["sv_ojt_is_acknowledge"]);
                            lstOJT.Add(objOJT);
                        }
                    }
                    if (dsResult.Tables.Count > 1)
                    {
                        if (dsResult.Tables[1].Rows.Count > 0)
                        {
                            lstAttachments = new List<OJTAttachmentBE>();
                            foreach (DataRow dr in dsResult.Tables[1].Rows)
                            {
                                objAttachment = new OJTAttachmentBE();
                                objAttachment.AttachmentID = dr["sv_ojt_attachments_id_pk"].ToString();
                                objAttachment.OJTID = dr["sv_ojt_id_fk"].ToString();
                                objAttachment.FileType = dr["sv_file_type"].ToString();
                                objAttachment.OJTFileName = dr["sv_file_name"].ToString();
                                lstAttachments.Add(objAttachment);
                            }
                        }
                    }

                    objReturn.LOJT = lstOJT;
                    objReturn.LOJTAttachments = lstAttachments;
                }
                return objReturn;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static string GetReceivedOJTResponse(OJTSentToBE clsOJT)
        {
            try
            {
                Hashtable htChangeStatusReceivedOJT = new Hashtable();
                htChangeStatusReceivedOJT.Add("@sv_ojt_id", clsOJT.OJTID);
                htChangeStatusReceivedOJT.Add("@sv_ojt_user_id", clsOJT.SentTo);
                try
                {
                    DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_received_ojt_sync_mobile", htChangeStatusReceivedOJT);

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

        public static string AcknowledgeOJT(OJTSentToBE clsOJTSentTo)
        {
            Hashtable htAcknowledge = new Hashtable();
            htAcknowledge.Add("@sv_ojt_sent_to_user", clsOJTSentTo.SentTo);
            htAcknowledge.Add("@sv_ojt_id", clsOJTSentTo.OJTID);
            htAcknowledge.Add("@sv_ojt_acknowledge_date", clsOJTSentTo.AcknowledgeDate);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_user_acknowledge_ojt", htAcknowledge);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static List<OJTDownloadAcknowlegdeStatusBE> AcknowledgeOJTSync(string strUserID)
        {
            try
            {
                OJTDownloadAcknowlegdeStatusBE objReturn = null;
                List<OJTDownloadAcknowlegdeStatusBE> lstReturn = null;
                Hashtable htOJT = new Hashtable();
                htOJT.Add("@sv_ojt_created_by_fk", strUserID);
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_user_acknowledge_ojt_sync", htOJT);
                if (dtResult.Rows.Count > 0)
                {
                    lstReturn = new List<OJTDownloadAcknowlegdeStatusBE>();
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        objReturn = new OJTDownloadAcknowlegdeStatusBE();
                        objReturn.OJTID = dr["sv_ojt_id_pk"].ToString();
                        objReturn.SentToUser = dr["sv_ojt_sent_to_user_fk"].ToString();
                        objReturn.SentToUserByName = dr["u_username_enc"].ToString();
                        objReturn.AcknowledgeDate = dr["sv_ojt_acknowledge_date"].ToString();
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


        public static string UserArchiveOJT(OJTSentToBE clsOJTSentTo)
        {
            Hashtable htArchiveOJT = new Hashtable();
            htArchiveOJT.Add("@sv_ojt_user_id", clsOJTSentTo.SentTo);
            htArchiveOJT.Add("@sv_ojt_id", clsOJTSentTo.OJTID);
            try
            {
                //int Result = DataProxy.FetchSPOutput("sv_wcf_archive_fieldnote", htArchiveFieldNote);
                //return Result;

                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_archive_ojt", htArchiveOJT);
                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static List<OJTArchive> GetUserArchiveOJTFromWeb(string strUserId)
        {
            Hashtable htArchiveOJT = new Hashtable();
            htArchiveOJT.Add("@sv_ojt_user_id", strUserId);
            try
            {
                List<OJTArchive> lstReturn = null;
                OJTArchive objReturn = null;
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_get_web_archive_ojt", htArchiveOJT);
                if (dtResult.Rows.Count > 0)
                {
                    lstReturn = new List<OJTArchive>();
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        objReturn = new OJTArchive();
                        objReturn.OJTID = dr["ojt_Id"].ToString();
                        objReturn.UserId = dr["UserID"].ToString();
                        objReturn.UserIdByName = dr["UserName"].ToString();
                        objReturn.OJTType = dr["ojtType"].ToString();
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

        public static List<OJTHARM> GetHARMFromWeb()
        {
            //Hashtable htHARMOJT = new Hashtable();
            //htHARMOJT.Add("@u_user_id_fk", strUserId);
            try
            {
                List<OJTHARM> lstHARM = null;
                OJTHARM objHarm = null;
                DataTable dtResult = DataProxy.FetchDataTable("sv_get_harm");
                if (dtResult.Rows.Count > 0)
                {
                    lstHARM = new List<OJTHARM>();
                    foreach (DataRow dr in dtResult.Rows)
                    {
                        objHarm = new OJTHARM();
                        objHarm.HARMID = dr["h_harm_id_pk"].ToString();
                        objHarm.HARMNO = dr["h_harm_number"].ToString();
                        lstHARM.Add(objHarm);
                    }
                }
                return lstHARM;
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
