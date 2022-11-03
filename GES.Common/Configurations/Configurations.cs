using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace GES.Common.Configurations
{
    public class Configurations
    {
        public const string DateFormat = "yyyy-MM-dd";
        public const string DateWithTimeFormat = "yyyy-MM-dd HH:mm:ss tt";
        public const string DateTimeFormat = "yyyyMMddHHmmss";
        public const string BracketWrapper = "[{0}]";
        public static readonly int[] MilestoneValues = {1, 2, 3, 4, 5};
        public static string AllGesUsersCache = "AllGesUsers";
        public static string AllOldUsersCache = "AllOldUsers";
        public static string BaseImageLink = "../../Content/img/staff/{0}.png"; //Use https protocol to fix "Mixed content http and https" on Edge
        public static string OldEmailDomain = "@ges-invest.com";
        public static string EmailDomain = "@gesinternational.com";
        public static string SustainDomain = "@sustainalytics.com";
        public static string NeutralAnalystUser = "Neutral Analyst";
        public static string NullContent = "null";
        public static IDictionary<string, string> KpiPerformanceColors = ((Hashtable) ConfigurationManager.GetSection("kpiColorConfig"))?.Cast<DictionaryEntry>().ToDictionary(n => n.Key.ToString(), n => n.Value.ToString());
        public const long GovernanceServiceId = 53;
    }
}
