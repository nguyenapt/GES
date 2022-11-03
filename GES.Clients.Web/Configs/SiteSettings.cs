using System;
using System.Collections.Generic;
using System.Configuration;

namespace GES.Clients.Web.Configs
{
    public static class SiteSettings
    {
        // common
        public static string SiteName => ConfigurationManager.AppSettings["siteName"];
        public static string SiteVersion => ConfigurationManager.AppSettings["siteVersion"];

        // mail accounts
        public static string MailNoReply => ConfigurationManager.AppSettings["mailNoReply"];
        public static string MailNoReplyName => ConfigurationManager.AppSettings["mailNoReplyName"];
        public static string MailContact => ConfigurationManager.AppSettings["mailContact"];
        public static string MailContactName => ConfigurationManager.AppSettings["mailContactName"];

        // mailing info
        public static string MailPrefix => ConfigurationManager.AppSettings["mailPrefix"];
        public static string MailCompanyName => ConfigurationManager.AppSettings["mailCompanyName"];

        // mail templates
        public static string TemplaterCustomer => ConfigurationManager.AppSettings["templaterCustomer"];
        public static string TemplaterLicense => ConfigurationManager.AppSettings["templaterLicense"];

        // mandrill api key
        public static string MandrillApiKey => ConfigurationManager.AppSettings["mandrillApiKey"];
        
        //SMTP information
        public static string SmtpHost => ConfigurationManager.AppSettings["smtpHost"];
        public static int SmtpPort => Convert.ToInt32(ConfigurationManager.AppSettings["smtpPort"]);
        public static string SmtpUserName => ConfigurationManager.AppSettings["smtpUserName"];
        public static string SmtpUserPassword => ConfigurationManager.AppSettings["smtpUserPassword"];
        public static bool SmtpEnableSsl => Convert.ToBoolean(ConfigurationManager.AppSettings["smtpEnableSsl"]);
        public static string EventSenderEmailAddress => ConfigurationManager.AppSettings["eventSenderEmailAddress"];
        public static string EventSenderEmailName => ConfigurationManager.AppSettings["eventSenderEmailName"];


        // links
        public static string OldClientsSiteUrl => ConfigurationManager.AppSettings["oldClientsSiteUrl"];
        public static string BaseDownloadUrl => ConfigurationManager.AppSettings["baseDownloadUrl"];

        // other settings
        public static int GlossaryTruncateLimit => int.Parse(ConfigurationManager.AppSettings["glossaryTruncateLimit"]);
        public static int ClickyTrackingId => int.Parse(ConfigurationManager.AppSettings["clickyTrackingId"]);

        public static string AnnouncementFeedUrl => ConfigurationManager.AppSettings["announcementFeedUrl"];

        public static string BlogFeedUrl => ConfigurationManager.AppSettings["blogFeedUrl"];

        public static List<string> ToBeClearedActionMethods => new List<string>()
                {
                    "DashboardGetLatestNews",
                    "DashboardGetLatestMilestones",
                    "DashboardGetCalendarEvents",
                    "DashboardGetDoughnutChartData",
                    "DashboardGetMapData",
                    "DashboardGetInfoBoxData",
                };

        public static double RecentLoggedInWindow => -24.0; // in hours
    }
}