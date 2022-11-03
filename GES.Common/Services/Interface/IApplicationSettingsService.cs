namespace GES.Common.Services.Interface
{
    public interface IApplicationSettingsService
    {
        string BlobPath { get; }
        string FilePathOnOldSystem { get; }
        string GetSetting(string key);
        T GetSetting<T>() where T : class, new();
        string ExportBlobPath { get; }
        string GesReportFileUploadPathForOldSystem { get; }
        int LongCommandTimeout { get; }

        string SmtpUserName { get; }
        string SmtpUserPassword { get; }
        string SmtpHost { get; }
        int SmtpPort { get; }
        bool SmtpEnableSsl { get; }
        string EventSenderEmailAddress { get; }
        string EventSenderEmailName{ get; }
        bool AllowViewAttendees{ get; }
        
        string MailCompanyName { get; }
        string MailNoReply { get; }
    }
}