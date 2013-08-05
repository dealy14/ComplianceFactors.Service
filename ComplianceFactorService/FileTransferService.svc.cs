using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Net;
using System.IO;
using ComplianceFactorService.Utility;
using System.Configuration;
using CFService.BusinessComponent;

namespace ComplianceFactorService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FileTransferService" in code, svc and config file together.
    public class FileTransferService : IFileTransferService
    {
        string ftpPath = ConfigurationManager.AppSettings["FTPPath"];
        string ftpUsername = ConfigurationManager.AppSettings["FTPFNUserName"];
        string ftpPwd = ConfigurationManager.AppSettings["FTPFNPwd"];
        string ftpOJTUsername = ConfigurationManager.AppSettings["FTPOJTUserName"];
        string ftpOJTPwd = ConfigurationManager.AppSettings["FTPOJTPwd"];
        string ftpMIUsername = ConfigurationManager.AppSettings["FTPMIUserName"];
        string ftpMIPwd = ConfigurationManager.AppSettings["FTPMIPwd"];
        public string DoWork()
        {
            // return "Hello Report";
            return System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

        }

        public UploadResponseTest FieldNoteAttachmentsUpload(clsFieldNotesAttachment clsfieldsAtt)
        {
            try
            {
                string strUploadFilename = Guid.NewGuid().ToString();
                string strFileExtension = "." + clsfieldsAtt.FileName.ToString().Split('.')[1].ToString();
                strUploadFilename = strUploadFilename + strFileExtension;

                string ftpfullpath = ftpPath + strUploadFilename;
                Logger.WriteToErrorLog("FileTransferService:FieldNoteAttachmentsUpload() " , ftpfullpath);
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpUsername, ftpPwd);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                byte[] buf;  // byte array
                Stream stream = clsfieldsAtt.Attachments;  //initialise new stream
                buf = new byte[stream.Length];  //declare arraysize
                stream.Read(buf, 0, buf.Length);

                Stream ftpstream = ftp.GetRequestStream();
                //ftpstream.Write(clsfieldsAtt.Attachments, 0, clsfieldsAtt.Attachments.Length);
                ftpstream.Write(buf,0, buf.Length);
                ftpstream.Close();

                //byte[] buf;  // byte array
                //Stream stream = clsfieldsAtt.Attachments;  //initialise new stream
                //buf = new byte[stream.Length];  //declare arraysize
                //stream.Read(buf, 0, buf.Length);

                //Update the Attachement table with the upload image path
                int result = FieldNotesBLL.FieldnoteAttachmentsUpload(clsfieldsAtt.FNAttachmentID, strUploadFilename);

                return new UploadResponseTest { UploadSucceeded = true };
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("FileTransferService:FieldNoteAttachmentsUpload() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("FileTransferService:FieldNoteAttachmentsUpload() ", ex.Message);
                }
                return new UploadResponseTest
                {
                    UploadSucceeded = false
                };
            }
        }

        //File Download
        public byte[] FieldNoteAttachmentsDownload(string strAttachementFileID)  //OK
        {
            try
            {
                //string imagePath = "e:\\upload\\" + fileId;
                //FileStream fileStream = null;
                //BinaryReader reader = null;
                //byte[] imageBytes;
                //fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                //reader = new BinaryReader(fileStream);
                //imageBytes = reader.ReadBytes((int)fileStream.Length);
                //return imageBytes;

                //Get the image url from DB with the strAttachementFileID
                string fileName = FieldNotesBLL.FieldnoteAttachmentsDownload(strAttachementFileID);
                if (fileName == string.Empty)
                    return null;

                string ftpfullpath = ftpPath + fileName;
                Logger.WriteToErrorLog("FileTransferService:FieldNoteAttachmentsDownload() ", ftpfullpath);
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftpRequest.Credentials = new NetworkCredential(ftpUsername, ftpPwd);
                ftpRequest.KeepAlive = true;
                ftpRequest.UseBinary = true;
                ftpRequest.UseBinary = true;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();

                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("FileTransferService:FieldNoteAttachmentsDownload() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("FileTransferService:FieldNoteAttachmentsDownload() ", ex.Message);
                }
                return null;
            }
        }

        #region Upload and Download Using Base64

        public string Upload(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                clsFileDetails objFileDetails = new clsFileDetails();
                objFileDetails = Utility.Utility.JsonDeserialize<clsFileDetails>(strJsonString);

                string strUploadFilename = Guid.NewGuid().ToString();
                strUploadFilename = strUploadFilename + objFileDetails.FileExtension;

                //Convert Base64String to Byte[]
                Base64Decoder decoder = new Base64Decoder(objFileDetails.FileBase64string.ToCharArray());
                byte[] image = decoder.GetDecoded();

                string ftpfullpath = ftpPath + strUploadFilename;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpUsername, ftpPwd);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(image, 0, image.Length);
                //ftpstream.Write(objFileDetails.FileBase64string, 0, objFileDetails.FileBase64string.Length);
                ftpstream.Close();

                //byte[] buf;  // byte array
                //Stream stream = clsfieldsAtt.Attachments;  //initialise new stream
                //buf = new byte[stream.Length];  //declare arraysize
                //stream.Read(buf, 0, buf.Length);

                //Update the Attachement table with the upload image path
                int result = FieldNotesBLL.FieldnoteAttachmentsUpload(objFileDetails.FieldNotesAttID, strUploadFilename);
                if (result == 0)
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "success";
                }
                else
                {
                    objResponse.StatusCode = result.ToString();
                    objResponse.StatusMessage = "Db error";
                }
                
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("FileTransferService:Upload() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("FileTransferService:Upload() ", ex.Message);
                }
                objResponse.StatusCode = "-3";
                objResponse.StatusMessage = ex.Message.ToString();
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        // public string Download(string strJsonString)
        //{
        //    try
        //    {
        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        if (Logger.LogErrors == true)
        //        {
        //            if (ex.InnerException != null)
        //                Logger.WriteToErrorLog("FileTransferService:Download() ", ex.Message, ex.InnerException.Message);
        //            else
        //                Logger.WriteToErrorLog("FileTransferService:Download() ", ex.Message);
        //        }
        //        return "Fail";
        //    }
        //}
        #endregion


        #region OJT
        public byte[] OJTAttachmentsDownload(string strAttachementFileID)  //OK
        {
            try
            {
                //Get the image url from DB with the strAttachementFileID
                string fileName = OJTBLL.OJTAttachmentsDownload(strAttachementFileID);
                if (fileName == string.Empty)
                    return null;

                string ftpfullpath = ftpPath + fileName;
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftpRequest.Credentials = new NetworkCredential(ftpOJTUsername, ftpOJTPwd);
                ftpRequest.KeepAlive = true;
                ftpRequest.UseBinary = true;
                ftpRequest.UseBinary = true;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();

                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("FileTransferService:OJTAttachmentsDownload() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("FileTransferService:OJTAttachmentsDownload() ", ex.Message);
                }
                return null;
            }
        }

      

        public string OJTUpload(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                clsOJTFileDetails objFileDetails = new clsOJTFileDetails();
                objFileDetails = Utility.Utility.JsonDeserialize<clsOJTFileDetails>(strJsonString);

                string strUploadFilename = Guid.NewGuid().ToString();
                strUploadFilename = strUploadFilename + objFileDetails.FileExtension;

                //Convert Base64String to Byte[]
                Base64Decoder decoder = new Base64Decoder(objFileDetails.FileBase64string.ToCharArray());
                byte[] image = decoder.GetDecoded();

                string ftpfullpath = ftpPath + strUploadFilename;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpOJTUsername, ftpOJTPwd);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(image, 0, image.Length);
                //ftpstream.Write(objFileDetails.FileBase64string, 0, objFileDetails.FileBase64string.Length);
                ftpstream.Close();

                //byte[] buf;  // byte array
                //Stream stream = clsfieldsAtt.Attachments;  //initialise new stream
                //buf = new byte[stream.Length];  //declare arraysize
                //stream.Read(buf, 0, buf.Length);

                //Update the Attachement table with the upload image path
                int result = OJTBLL.OJTAttachmentsUpload(objFileDetails.OJTAttID, strUploadFilename);
                if (result == 0)
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "success";
                }
                else
                {
                    objResponse.StatusCode = result.ToString();
                    objResponse.StatusMessage = "Db error";
                }

            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("FileTransferService:OJTUpload() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("FileTransferService:OJTUpload() ", ex.Message);
                }
                objResponse.StatusCode = "-3";
                objResponse.StatusMessage = ex.Message.ToString();
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }

        public string OJTCertificateUpload(string strJsonString)
        {
            GeneralResponse objResponse = new GeneralResponse();
            try
            {
                clsOJTCertificateFileDetails objFileDetails = new clsOJTCertificateFileDetails();
                objFileDetails = Utility.Utility.JsonDeserialize<clsOJTCertificateFileDetails>(strJsonString);

                string strUploadFilename = Guid.NewGuid().ToString();
                strUploadFilename = strUploadFilename + objFileDetails.FileExtension;

                //Convert Base64String to Byte[]
                Base64Decoder decoder = new Base64Decoder(objFileDetails.FileBase64string.ToCharArray());
                byte[] image = decoder.GetDecoded();

                string ftpfullpath = ftpPath + strUploadFilename;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential(ftpOJTUsername, ftpOJTPwd);
                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(image, 0, image.Length);
                //ftpstream.Write(objFileDetails.FileBase64string, 0, objFileDetails.FileBase64string.Length);
                ftpstream.Close();

                //byte[] buf;  // byte array
                //Stream stream = clsfieldsAtt.Attachments;  //initialise new stream
                //buf = new byte[stream.Length];  //declare arraysize
                //stream.Read(buf, 0, buf.Length);

                //Update the Attachement table with the upload image path
                int result = OJTBLL.OJTCertificateUpload(objFileDetails.OJTID, strUploadFilename);
                if (result == 0)
                {
                    objResponse.StatusCode = "0";
                    objResponse.StatusMessage = "success";
                }
                else
                {
                    objResponse.StatusCode = result.ToString();
                    objResponse.StatusMessage = "Db error";
                }

            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("FileTransferService:OJTCertificateUpload() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("FileTransferService:OJTCertificateUpload() ", ex.Message);
                }
                objResponse.StatusCode = "-3";
                objResponse.StatusMessage = ex.Message.ToString();
            }
            string strResponse = Utility.Utility.JsonSerialize(objResponse.GetType(), objResponse);
            return strResponse;
        }


        public byte[] OJTCertificateDownload(string strOJTID)  
        {
            try
            {
                //Get the image url from DB with the strAttachementFileID
                string fileName = OJTBLL.OJTCertificateDownload(strOJTID); //Need to create SP for get certify file
                if (fileName == string.Empty)
                    return null;

                string ftpfullpath = ftpPath + fileName;
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftpRequest.Credentials = new NetworkCredential(ftpOJTUsername, ftpOJTPwd);
                ftpRequest.KeepAlive = true;
                ftpRequest.UseBinary = true;
                ftpRequest.UseBinary = true;
                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();

                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                if (Logger.LogErrors == true)
                {
                    if (ex.InnerException != null)
                        Logger.WriteToErrorLog("FileTransferService:OJTCertificateDownload() ", ex.Message, ex.InnerException.Message);
                    else
                        Logger.WriteToErrorLog("FileTransferService:OJTCertificateDownload() ", ex.Message);
                }
                return null;
            }
        }

        #endregion

    }
}
