using System.Configuration;

namespace GES.Inside.Web.Configs
{
    public static class SiteSettings
    {
        public static string SiteName
        {
            get { return ConfigurationManager.AppSettings["siteName"];  }
        }
        public static string SiteVersion
        {
            get { return ConfigurationManager.AppSettings["siteVersion"]; }
        }

        // Job Scheduler Configuration Values
        public static int JobGenerateKeywordsMaxItemsEachRun => int.Parse(ConfigurationManager.AppSettings["jobGenerateKeywords_MaxItemsEachRun"]);
        public static int JobGenerateKeywordsDelayedStartInMinutes => int.Parse(ConfigurationManager.AppSettings["jobGenerateKeywords_DelayedStartInMinutes"]);
        public static int JobGenerateKeywordsMinutesInterval => int.Parse(ConfigurationManager.AppSettings["jobGenerateKeywords_MinutesInterval"]);

        // Templater Configuration Values
        public static string TemplaterCustomer => ConfigurationManager.AppSettings["templaterCustomer"];
        public static string TemplaterLicense => ConfigurationManager.AppSettings["templaterLicense"];

        public static string BaseDownloadUrl => ConfigurationManager.AppSettings["baseDownloadUrl"];

        public static string FilePathOnOldSystem => ConfigurationManager.AppSettings["FilePathOnOldSystem"];
    }
}