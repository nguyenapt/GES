using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using GES.Inside.Web.Configs;

namespace GES.Inside.Web.Helpers
{
    public static class CommonHelper
    {
        public static string Serialize(object o)
        {
            var js = new JavaScriptSerializer();
            return js.Serialize(o);
        }

        public static string GetDocDownloadUrl(string filename)
        {
            //return $"{SiteSettings.BaseDownloadUrl}{filename}";
            return $"../DownloadFile?fileName={filename}";
        }
    }
}