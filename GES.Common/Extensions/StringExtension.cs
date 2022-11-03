using System;

namespace GES.Common.Extensions
{
    public static class StringExtension
    {
        public static bool Contains(this string target, string value, StringComparison comparison)
        {
            return target.IndexOf(value, comparison) >= 0;
        }

        public static string ToTitleCase(this string sample)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(sample.ToLower());
        }
    }
}
