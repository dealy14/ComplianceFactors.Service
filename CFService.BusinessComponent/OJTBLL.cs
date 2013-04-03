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
            htCreateOJT.Add("@", clsOJT.CreatedBy);
            htCreateOJT.Add("@", clsOJT.Title);
            htCreateOJT.Add("@", clsOJT.description);
            htCreateOJT.Add("@", clsOJT.location);
            htCreateOJT.Add("@", clsOJT.SentTo);
            try
            {
                DataTable dtResult = DataProxy.FetchDataTable("sv_wcf_insert_ojt", htCreateOJT);

                return dtResult.Rows[0][0].ToString();
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static int CreateOJTAttachment(OJTAttachmentBE clsOJTAttachment)
        {
            Hashtable htCreateOJTAttachment = new Hashtable();
            htCreateOJTAttachment.Add("@", clsOJTAttachment.OJTID);
            htCreateOJTAttachment.Add("@", clsOJTAttachment.FilePath);
            htCreateOJTAttachment.Add("@", clsOJTAttachment.FileName);
            try
            {
                int Result = DataProxy.FetchSPOutput("", htCreateOJTAttachment);
                return Result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static DataSet GetUserOJT(OJTSentToBE clsOJTSentTo)
        {
            Hashtable htGetUserOJT = new Hashtable();
            htGetUserOJT.Add("@", clsOJTSentTo.SentTo);
            try
            {
                DataSet dsResult = DataProxy.FetchDataSet("", htGetUserOJT);

                return dsResult;
            }

            catch (Exception)
            {
                throw;
            }
        }


        public static int SyncOJT(OJTSentToBE clsOJTSentTo)
        {
            Hashtable htSyncOJT = new Hashtable();
            htSyncOJT.Add("@sv_fieldnote_sent_to_user", clsOJTSentTo.OJTID);
            htSyncOJT.Add("@sv_fieldnote_id", clsOJTSentTo.SentTo);
            try
            {
                int Result = DataProxy.FetchSPOutput("", htSyncOJT);

                return Result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static int AcknowledgeOJT(OJTSentToBE clsOJTSentTo)
        {
            Hashtable htAcknowledgeOJT = new Hashtable();
            htAcknowledgeOJT.Add("@", clsOJTSentTo.OJTID);
            htAcknowledgeOJT.Add("@", clsOJTSentTo.SentTo);
            try
            {
                int Result = DataProxy.FetchSPOutput("", htAcknowledgeOJT);

                return Result;
            }

            catch (Exception)
            {
                throw;
            }
        }

        public static int UserArchiveOJT(OJTSentToBE clsOJTSentTo)
        {
            Hashtable htArchiveOJT = new Hashtable();
            htArchiveOJT.Add("@", clsOJTSentTo.OJTID);
            htArchiveOJT.Add("@", clsOJTSentTo.SentTo);
            try
            {
                int Result = DataProxy.FetchSPOutput("", htArchiveOJT);

                return Result;
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
