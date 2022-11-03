using System.Collections.Generic;
using System.IO;
using GES.Common.Services.Interface;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace GES.Clients.Web.PhantomJs
{
    public class PdfFilesMerger : IPdfFilesMerger
    {
        private readonly IApplicationSettingsService _applicationSettingsService;

        public PdfFilesMerger(IApplicationSettingsService applicationSettingsService)
        {
            _applicationSettingsService = applicationSettingsService;
        }

        public string MergeTempFile(IEnumerable<string> files, string mergedFileName, bool isDeleteSourceFile)
        {
            using (var outputDocument = new PdfDocument())
            {
                foreach (var file in files)
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
                }
                var mergedFilePath = Path.Combine(_applicationSettingsService.ExportBlobPath, $"{mergedFileName}.pdf");
                outputDocument.Save(mergedFilePath);
            }
            return mergedFileName;
        }
    }
}