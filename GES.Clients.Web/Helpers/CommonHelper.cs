using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using GES.Clients.Web.Configs;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Interfaces;

namespace GES.Clients.Web.Helpers
{
    public static class CommonHelper
    {
        private const string InitMilestone = "0";
        private const string MultipleMilestonePattern = @"Milestone[s]?\s?\d\s?&\s?\d\s?[a-zA-Z\s]*[,.:]";
        private const string MultipleMilestoneValuePattern = @"\d\s?&\s?\d";
        private const string SingleMilestonePattern = @"Milestone[s]?\s?\d\s?[a-zA-Z\s]*[,.:]";
        private const string SingleMilestoneValuePattern = @"\d";

        public static string Serialize(object o)
        {
            var js = new JavaScriptSerializer();
            return js.Serialize(o);
        }

        public static string GetDocDownloadUrl(string filename)
        {
            return $"{SiteSettings.BaseDownloadUrl}{HttpUtility.UrlEncode(filename)}";
            //return $"../../DownloadFile/{HttpUtility.UrlEncode(filename)}";
        }

        public static string GetOldSiteUrl(string toAppend)
        {
            var oldClientSiteBaseUrl = SiteSettings.OldClientsSiteUrl;
            oldClientSiteBaseUrl = oldClientSiteBaseUrl.EndsWith("/") ? oldClientSiteBaseUrl : oldClientSiteBaseUrl + "/";
            return string.Format("{0}{1}", oldClientSiteBaseUrl, toAppend);
        }

        public static string RenderExplanationTooltip(string content)
        {
            return string.Format("<a class='explanation-tooltip' title='{0}'><i class='fa fa-question-circle-o'></i></a>", content);
        }

        public static string GetFaFileStr(string ext)
        {
            switch (ext)
            {
                case "jpg":
                case "jpeg":
                case "gif":
                case "bmp":
                case "png":
                    return "image";
                case "xls":
                case "xlsx":
                    return "excel";
                case "doc":
                case "docx":
                    return "word";
                case "txt":
                    return "text";
                case "ppt":
                case "pptx":
                    return "powerpoint";
                case "pdf":
                    return "pdf";
                case "zip":
                case "rar":
                case "tar":
                case "7z":
                    return "archive";
                default:
                    return "unk";
            }
        }

        public static string GetChartColor(int num, bool reverted = true)
        {
            var colors = new List<string>
            {
                "#5092CC",
                "#E38002",
                "#ACAB00",
                "#9A527A",
                "#E6C006",
                "#3D709C",
                "#C45B00",
                "#7D7F00",
                "#6B225E",
                "#BD980B",
                "#2C5171",
                "#A34C00",
                "#646600",
                "#511A47",
                "#A38309",
                "#1F3A51",
                "#853E00",
                "#4B4C00",
                "#3F1337",
                "#7F6607"
            };

            // num: from 0
            if (num > colors.Count - 1)
                return colors[0];

            if (!reverted)
            {
                return colors[num]; 
            }

            return colors[colors.Count - 1 - num];
        }

        public static List<PrimaryAndForeignKeyModel> GetRecentLoggedInIndividualIdsAndOrgIds(IGesUserService gesUserService, IOldUserService oldUserService)
        {
            var timeWindow = SiteSettings.RecentLoggedInWindow;

            var recentLoggedInOldUserIds = gesUserService.GetOldUserIdLastLogin(timeWindow);
            return oldUserService.GetIndividualIdsAndOrgIdsLastLogin(recentLoggedInOldUserIds);
        }

        public static string ConvertNameToHtmlElementId(string name)
        {
            return name.ToLower().Trim().Replace("'", "").Replace(" ", "-");
        }

        private static string GetMilestoneIcon(string latestMilestone)
        {
            const string milestoneIcon = "milestone_{0}.png";

            string milestoneAchieved;

            if (!TryGetHighestMilestone(latestMilestone, out milestoneAchieved))
            {
                milestoneAchieved = GetSingleMilestone(latestMilestone);
            }
            return string.Format(milestoneIcon, milestoneAchieved);
        }
        
        public static string GetMilestoneIcon(string latestMilestone, int level)
        {
            if (level == 0) return GetMilestoneIcon(latestMilestone);
            
            const string milestoneIcon = "milestone_{0}.png";
            return string.Format(milestoneIcon, level);

        }

        public static string RemoveMilestoneAchived(string latestMilestone)
        {
            var milestoneAchieved = GetMilestoneAchieved(latestMilestone, MultipleMilestonePattern);
            if (string.IsNullOrWhiteSpace(milestoneAchieved?.Value))
            {
                milestoneAchieved = GetMilestoneAchieved(latestMilestone, SingleMilestonePattern);
            }

            if (!string.IsNullOrWhiteSpace(milestoneAchieved?.Value))
            {
                var cropLatestMilestone = latestMilestone.Replace(milestoneAchieved.Value, "").Trim();
                if (!string.IsNullOrEmpty(cropLatestMilestone))
                {
                    return char.ToUpperInvariant(cropLatestMilestone[0]) + cropLatestMilestone.Substring(1);
                }
                return string.Empty;
            }
            return latestMilestone;
        }

        private static bool TryGetHighestMilestone(string latestMilestone, out string milestoneStr)
        {
            milestoneStr = InitMilestone;
            var milestonesAchieved = GetMilestoneAchieved(latestMilestone, MultipleMilestonePattern);
            if (string.IsNullOrWhiteSpace(milestonesAchieved?.Value))
            {
                return false;
            }

            var match = Regex.Match(milestonesAchieved.Value, MultipleMilestoneValuePattern).Value;

            if (match.Contains("&"))
            {
                var milestones = match.Split('&');
                var milestoneList = new List<int>();
                foreach (var milestone in milestones)
                {
                    int milestoneValue;
                    if (int.TryParse(milestone, out milestoneValue))
                    {
                        milestoneList.Add(milestoneValue);
                    }
                }
                if (milestoneList.Any())
                {
                    milestoneStr = milestoneList.Max(x => x).ToString();
                    return true;
                }
                    
            }
            return false;
        }

        private static string GetSingleMilestone(string latestMilestone)
        {
            var milestoneAchieved = GetMilestoneAchieved(latestMilestone, SingleMilestonePattern);
            if (string.IsNullOrWhiteSpace(milestoneAchieved?.Value)) return InitMilestone;

            return Regex.Match(milestoneAchieved.Value, SingleMilestoneValuePattern).Value;
        }

        private static Match GetMilestoneAchieved(string latestMilestone, string pattern)
        {
            if (string.IsNullOrEmpty(latestMilestone))
                return null;
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.Match(latestMilestone);
        }
    }
}