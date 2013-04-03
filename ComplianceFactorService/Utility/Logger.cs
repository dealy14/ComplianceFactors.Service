using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Hosting;

namespace ComplianceFactorService.Utility
{
    public static class Logger
    {
        #region "Member variables"
        public static string APP_PATH = HostingEnvironment.ApplicationPhysicalPath;
        //public static string APP_PATH = HttpContext.Current.Server.MapPath("~/Logs/");
        public static string APP_LOG_FILE_PATH = APP_PATH + "Logs\\AppLog.txt";
        public static string ERROR_LOG_FILE_PATH = APP_PATH + "Logs\\ErrorLog.txt";
        public static string NEW_LINE = "\r\n";
        #endregion

        #region "Error Log"
        /// <summary>
        /// To write error message to error log
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteToErrorLog(string pageName, string pLogMessage)
        {
            try
            {
                if (pLogMessage.Trim().Length > 0)
                {
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(ERROR_LOG_FILE_PATH, true);
                    writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " : " + pageName + " : " + pLogMessage + NEW_LINE);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// Writes detailed error message to error log
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteToErrorLog(string pageName, string pLogMessage, string pLogInnerException)
        {
            try
            {
                if (pLogMessage.Trim().Length > 0)
                {
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(ERROR_LOG_FILE_PATH, true);
                    writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " : " + pageName + " : " + pLogMessage + NEW_LINE + pLogInnerException + NEW_LINE);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
            }
            catch { }
        }
        /// <summary>
        /// To write error message to error log
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteToApplicationLog(string pLogMessage)
        {
            try
            {
                if (pLogMessage.Trim().Length > 0)
                {
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(APP_LOG_FILE_PATH, true);
                    writer.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") + " : " + pLogMessage + NEW_LINE);
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// To get application log
        /// </summary>
        /// <param name="msg"></param>
        public static string GetLog(LogFileType pLogFileType)
        {
            string retStr = "";
            try
            {
                string filePath = "";
                switch (pLogFileType)
                {
                    case LogFileType.Application:
                        filePath = APP_LOG_FILE_PATH;
                        break;
                    case LogFileType.Error:
                        filePath = ERROR_LOG_FILE_PATH;
                        break;
                }
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.StreamReader reader = new System.IO.StreamReader(filePath);
                    retStr = reader.ReadToEnd();
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
            }
            catch { }
            return retStr;
        }

        /// <summary>
        /// Used to clear the application log
        /// </summary>
        /// <param name="pLogFileType"></param>
        /// <returns></returns>
        public static void ClearLog(LogFileType pLogFileType)
        {
            try
            {
                string filePath = "";
                switch (pLogFileType)
                {
                    case LogFileType.Application:
                        filePath = APP_LOG_FILE_PATH;
                        break;
                    case LogFileType.Error:
                        filePath = ERROR_LOG_FILE_PATH;
                        break;
                }
                try
                {
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath, false);
                        writer.WriteLine("-------------------------------------------------------------------------");
                        writer.WriteLine("Log cleared at " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                        writer.WriteLine("-------------------------------------------------------------------------");
                        writer.Flush();
                        writer.Close();
                        writer.Dispose();
                        writer = null;
                    }
                }
                catch { }
            }
            catch { }
        }


        //public static String GetXMLValue(String name)
        // {
        //     try
        //     {
        //         objXML = XDocument.Load(String.Format(bundleXMLpath, Application.StartupPath));

        //         System.Collections.Generic.IEnumerable<XElement> xmlValue = from v in objXML.Descendants("Settings").Elements(name).ToList() select v;

        //         if (xmlValue.Count() > 0)
        //         {
        //             return xmlValue.ToList()[0].Value;
        //         }
        //         return String.Empty;
        //     }
        //     catch (Exception ex)
        //     {
        //         WriteToErrorLog(String.Concat(ex.Message, "  ", ex.StackTrace));
        //         throw;
        //     }
        // }

        /// <summary>
        /// To handle Log file name
        /// </summary>
        public enum LogFileType
        {
            Application,
            Error
        }
        #endregion

        public static bool LogErrors
        {
            get
            {
                bool logErrors = false;
                Boolean.TryParse(ConfigurationManager.AppSettings["LogErrors"], out logErrors);
                return logErrors;
            }
        }
    }
}