using System;
using System.ComponentModel;
using System.Linq;

namespace Sustainalytics.GSS.Entities
{
    public enum AssessmentType
    {
        Compliant = 0,
        Watchlist = 1,
        [Description("Non-Compliant")]
        NonCompliant = 2
    }

    public static class EnumExtension
    {
        public static string GetDescription(this Enum @enum)
        {
            if (@enum == null)
            {
                return null;
            }

            var enumText = @enum.ToString();
            var enumDescription = @enum.GetType().GetField(enumText).GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>().FirstOrDefault()?.Description;

            return string.IsNullOrWhiteSpace(enumDescription) ? enumText : enumDescription;
        }
    }
}
