using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Clients.Web.PhantomJs
{
    public interface IPdfFileDownload
    {
        string DownloadTempFile(string files, string mergedFileName, bool isDeleteSourceFile);
    }
}
