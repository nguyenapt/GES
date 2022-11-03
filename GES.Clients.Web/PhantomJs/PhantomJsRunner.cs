using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using GES.Common.Logging;
using GES.Common.Services.Interface;

namespace GES.Clients.Web.PhantomJs
{
    public class PhantomJsRunner : IPhantomJsRunner
    {
        private readonly IApplicationSettingsService _applicationSettingsService;
        private readonly IPdfFilesMerger _pdfFilesMerger;
        private readonly IGesLogger _logger;
        private const string SeparatorChars = ";#$";
        private const int PhantomJsTimeout = 30 * 1000;
        private const string ExportSuccessMessage = "Export success";

        public PhantomJsRunner(IApplicationSettingsService applicationSettingsService, IPdfFilesMerger pdfFilesMerger, IGesLogger logger)
        {
            _applicationSettingsService = applicationSettingsService;
            _pdfFilesMerger = pdfFilesMerger;
            _logger = logger;
        }

        public string Run(IList<string> urls)
        {
            var pdfFiles = GeneratePdf(urls);

            if (pdfFiles == null || !pdfFiles.Any()) return string.Empty;

            var mergedResultFile = Guid.NewGuid().ToString();
            return _pdfFilesMerger.MergeTempFile(pdfFiles, mergedResultFile, true);
        }
        
        public string Run(IList<string> urls, IList<string> appendFileNames)
        {
            var pdfFiles = GeneratePdf(urls).ToList();

            if (pdfFiles == null || !pdfFiles.Any()) return string.Empty;

            pdfFiles.AddRange(appendFileNames);

            var mergedResultFile = Guid.NewGuid().ToString();

            return _pdfFilesMerger.MergeTempFile(pdfFiles, mergedResultFile,false);
        }

        private static string GetPhantomJsFilePath()
        {
            return HostingEnvironment.MapPath("~/PhantomJs/Library/phantomjs.exe");
        }

        private static string GetRasterizeJsFilePath()
        {
            return HostingEnvironment.MapPath("~/PhantomJs/Library/rasterize.js");
        }

        public IList<string> GeneratePdf(IList<string> urls)
        {
            var outputFolder = _applicationSettingsService.ExportBlobPath;

            var exists = Directory.Exists(outputFolder);

            if (!exists)
                Directory.CreateDirectory(outputFolder);

            var newFileNamePaths = new List<string>();

            for (var i = 0; i < urls.Count; i++)
            {
                newFileNamePaths.Add($"{outputFolder}{Guid.NewGuid()}.pdf");
            }

            var phantomJsArguments = $"--ignore-ssl-errors=\"yes\" {GetRasterizeJsFilePath()} {string.Join(SeparatorChars, urls)} {string.Join(SeparatorChars, newFileNamePaths)}";

            return TryExecutePhantomJs(phantomJsArguments) ? newFileNamePaths : null;
        }

        private bool TryExecutePhantomJs(string phantomJsArguments)
        {
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = GetPhantomJsFilePath(),
                Arguments = phantomJsArguments
            };

            var process = Process.Start(startInfo) ?? new Process();
            process.WaitForExit(PhantomJsTimeout);
            if (!process.HasExited)
            {
                process.Kill();
                _logger.Error("PhantomJs process wait timeout expired.");
                return false;
            }

            var output = process.StandardOutput.ReadToEnd();
            process.Close();

            if (!string.IsNullOrWhiteSpace(output) && output.Contains(ExportSuccessMessage))
            {
                _logger.Information("PhantomJs process executed successfully.");
                return true;
            }
            _logger.Error($"PhantomJs process failed with message {output}.");
            return false;
        }
    }
    
}