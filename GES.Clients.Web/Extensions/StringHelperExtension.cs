using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using System.Windows.Forms;
using Autofac;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Services;
using GES.Inside.Data.Services.Interfaces;

namespace GES.Clients.Web.Extensions
{
    public static class StringHelperExtension
    {
        public static string TruncateAtWord(this string value, int length)
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length, StringComparison.Ordinal) == -1)
                return value;

            return value.Substring(0, value.IndexOf(" ", length, StringComparison.Ordinal)) + " ...";
        }
    }
}