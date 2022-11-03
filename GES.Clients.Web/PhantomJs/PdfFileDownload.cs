using GES.Common.Services.Interface;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GES.Clients.Web.PhantomJs
{
    public class PdfFileDownload : IPdfFileDownload
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        public PdfFileDownload(IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
        }

        public string DownloadTempFile(string file, string mergedFileName, bool isDeleteSourceFile)
        {
            if (PdfReader.TestPdfFile(file) != 0)
            {
                using (var outputDocument = new PdfDocument())
                {
                    try
                    {

                        using (var inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import))
                        {
                            var count = inputDocument.PageCount;
                            for (var idx = 0; idx < count; idx++)
                            {
                                var page = inputDocument.Pages[idx];
                                outputDocument.AddPage(page);
                            }
                        }

                    }
                    finally
                    {
                        if (isDeleteSourceFile)
                        {
                            File.Delete(file);
                        }
                    }

                    var mergedFilePath = Path.Combine(_applicationSettingsService.ExportBlobPath, $"{mergedFileName}.pdf");
                    outputDocument.Save(mergedFilePath);
                }
                return mergedFileName;
            }
            return string.Empty;
        }
    }
}