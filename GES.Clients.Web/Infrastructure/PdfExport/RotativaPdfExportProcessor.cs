namespace GES.Clients.Web.Infrastructure.PdfExport
{
    using GES.Common.Services.Interface;
    using System.Web;
    using GES.Common.Models;
    using Rotativa;
    using Rotativa.Options;
    using System.Configuration;
    using System.Web.Security;

    public class RotativaPdfExportProcessor : IExportProcessor
    {
        public ExportResult Export(string url)
        {
            var rotativaRelativeFolder = ConfigurationManager.AppSettings["rotativa:folder"];
            var physicalFolder = System.Web.Hosting.HostingEnvironment.MapPath(rotativaRelativeFolder);
            var cookie = HttpContext.Current.Request.Cookies[ConfigurationManager.AppSettings["authen:cookiekey"]];

            return new ExportResult()
            {
                DataFile = this.BuildPdfFile(url, physicalFolder, cookie),
                MimeType = "application/pdf"
            };
        }
        
        private byte[] BuildPdfFile(string url, string rotativaFolder, HttpCookie authenCookie)
        {
            return new GesPageAsPdf(url)
            {
                PageOrientation = Orientation.Landscape,
                CustomSwitches = "--print-media-type --enable-javascript   --javascript-delay 15000 --minimum-font-size 12"
            }.BuildPdf(rotativaFolder, authenCookie);
        }

        /// <summary>
        /// Custom class to export pdf using Rotativa library
        /// </summary>
        private class GesPageAsPdf : UrlAsPdf
        {
            private readonly string _url;

            public GesPageAsPdf(string url) : base(url)
            {
                this._url = url;
            }

            public byte[] BuildPdf(string rotativaFolder, HttpCookie httpCookie)
            {
                this.WkhtmltopdfPath = string.IsNullOrEmpty(this.WkhtmltopdfPath) ? rotativaFolder : this.WkhtmltopdfPath;

                return this.CallTheDriver(httpCookie);
            }

            private byte[] CallTheDriver(HttpCookie httpCookie)
            {
                string wkParams = this.GetWkParams(httpCookie);
                return WkhtmltopdfDriver.Convert(this.WkhtmltopdfPath, wkParams);
            }

            private string GetWkParams(HttpCookie httpCookie)
            {
                string text = string.Empty;
                if (httpCookie != null)
                {
                    string value = httpCookie.Value;
                    string text2 = text;
                    text = string.Concat(new string[]
                    {
                        text2,
                        " --cookie ",
                        ConfigurationManager.AppSettings["authen:cookiekey"],
                        " ",
                        value
                    });
                }
                text = text + " " + this.GetConvertOptions();
                return text + " " + _url;
            }
        }
    }
}