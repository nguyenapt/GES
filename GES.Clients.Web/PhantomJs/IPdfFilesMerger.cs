using System.Collections.Generic;

namespace GES.Clients.Web.PhantomJs
{
    public interface IPdfFilesMerger
    {
        string MergeTempFile(IEnumerable<string> files, string mergedFileName, bool isDeleteSourceFile);
    }
}
