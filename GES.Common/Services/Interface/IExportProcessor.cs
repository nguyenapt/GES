using GES.Common.Models;

namespace GES.Common.Services.Interface
{
    /// <summary>
    /// Define interface to export the current page
    /// </summary>
    public interface IExportProcessor
    {
        ExportResult Export(string url);
    }
}
