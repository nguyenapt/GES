using System;
using System.Configuration;
using System.IO;
using GES.Common.Services.Interface;
using System.Linq;

namespace GES.Common.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        public virtual string BlobPath => ConfigurationManager.AppSettings["FileUploadPath"];
        public virtual string FilePathOnOldSystem => ConfigurationManager.AppSettings["FilePathOnOldSystem"];

        

        public virtual string GetSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public virtual T GetSetting<T>() where T : class, new()
        {
            var metaData = new T();
            var metaType = typeof(T);
            foreach(var prop in metaType.GetProperties().Where(s => s.GetAccessors().Any(a => a.IsPublic)))
            {
                var key = $"{metaType.Name.ToLower()}:{prop.Name.ToLower()}";

                var value = ConfigurationManager.AppSettings[key];

                if(!string.IsNullOrEmpty(value))
                {
                    prop.SetValue(metaData, value);
                }
            }

            return metaData;
        }

        public string ExportBlobPath => ConfigurationManager.AppSettings["TempExportPdfFolder"];
        public int LongCommandTimeout => int.Parse(ConfigurationManager.AppSettings["LongCommandTimeout"]);
        public string GesReportFileUploadPathForOldSystem => ConfigurationManager.AppSettings["FilePathTempUploadOnOldSystem"];
        
        //SMTP information
        public string SmtpHost => ConfigurationManager.AppSettings["smtpHost"];
        public int SmtpPort => Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
        public string SmtpUserName => ConfigurationManager.AppSettings["smtpUserName"];
        public string SmtpUserPassword => ConfigurationManager.AppSettings["smtpUserPassword"];
        public bool SmtpEnableSsl => Convert.ToBoolean(ConfigurationManager.AppSettings["smtpEnableSsl"]);
        public string EventSenderEmailAddress => ConfigurationManager.AppSettings["eventSenderEmailAddress"];
        public string EventSenderEmailName => ConfigurationManager.AppSettings["eventSenderEmailName"];
        public bool AllowViewAttendees => Convert.ToBoolean(ConfigurationManager.AppSettings["allowViewAttendees"]);


        public string MailCompanyName => ConfigurationManager.AppSettings["mailCompanyName"];
        public string MailNoReply => ConfigurationManager.AppSettings["mailNoReply"];
    }
}
