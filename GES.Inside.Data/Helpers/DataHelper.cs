using System.Globalization;
using GES.Common.Enumeration;
using ikvm.extensions;

namespace GES.Inside.Data.Helpers
{
    public static class DataHelper
    {
        public static long CalcDevelopmentGrade(float progressGrade, float responseGrade)
        {
            var newAvg = (progressGrade + responseGrade) / 2;
            var devGrade = 2;

            if (progressGrade <= 2 && responseGrade <= 2)
            {
                devGrade = 3;
            }
            else if (newAvg > 3 && responseGrade >= 3 && progressGrade >= 3)
            {
                devGrade = 1;
            }

            if (newAvg == 0)
            {
                devGrade = 0;
            }

            return devGrade;
        }
        
        public static long CalcDevelopmentGradeForExcelExport(long progressGrade, long responseGrade)
        {
            var newAvg = (progressGrade + responseGrade) / 2;
            var devGrade = 2;

            if (progressGrade <= 2 && responseGrade <= 2)
            {
                devGrade = 3;
            }
            else if (newAvg > 4 && responseGrade >= 4 && progressGrade >= 4)
            {
                devGrade = 1;
            }

            if (newAvg == 0)
            {
                devGrade = 0;
            }

            return devGrade;
        }

        public static string ConvertDevelopmentGradeToText(long? developmentGrade)
        {
            switch (developmentGrade)
            {
                case 1:
                    return "High";
                case 2:
                    return "Medium";
                case 3:
                    return "Low";
                default:
                    return "";
            }
        }

        public static string FormatServiceName(string origName, bool removeEngagementText = true, bool isExternal = false)
        {
            var finalName = origName.ToLower()
                            .Replace("ges", string.Empty)
                            .Replace("service", string.Empty)
                            .Replace("<sup>&reg;</sup>", string.Empty);

            finalName = removeEngagementText ? finalName.replace("engagement", "") : finalName;

            if (finalName == "conventions")
                finalName = "Global Standards";

            if (finalName == "extended – taxation")
                finalName = "Global Standards";

            finalName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(finalName.Trim());

            return isExternal ? "GES " + finalName : finalName;
        }

        public static string FormatEngagementTypeName(string origName)
        {
            return origName.Replace("Engagement", string.Empty).Trim();
        }

        public static string[] GetCacheTags(string tagKey, long orgId, long individualId)
        {
            var suffixOrg = "-o" + orgId;
            var suffixIndividual = "-i" + individualId;
            return new[] { tagKey, $"{tagKey}{suffixOrg}", $"{tagKey}{suffixIndividual}" };
        }

        public static string GetEngagementThemeNorm(string engagementTypeCategory, string norm, string engagementType, long engagementTypeId)
        {
            var engagementTheme = GetEngagementTheme(engagementTypeCategory, engagementType);
            var engagementNorm = GetEngagementNorm(norm, engagementType, engagementTypeId);

            var finalName = engagementTheme;

            if (!string.IsNullOrEmpty(engagementNorm))
            {
                finalName += $" - {engagementNorm}";
            }
         
            return finalName;
        }

        public static string GetEngagementNorm(string norm, string engagementType, long engagementTypeId)
        {
            return (engagementTypeId == (int)EngagementTypeEnum.Conventions ? norm : engagementType?.Replace("Engagement", "").Replace("engagement", ""))?.Trim();
        }

        public static string GetEngagementTheme(string engagementTypeCategory,  string engagementType)
        {
            return (!string.IsNullOrEmpty(engagementTypeCategory) && !engagementTypeCategory.ToLower().Contains("conduct")
                ? engagementTypeCategory.Replace("Engagement", "")
                : !string.IsNullOrEmpty(engagementType) && engagementType.ToLower().Contains("extended") ? engagementType : "Business Conduct").Trim();
        }
    }
}
