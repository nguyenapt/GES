using System.Collections.Generic;
using System.Threading.Tasks;

namespace GES.Clients.Web.PhantomJs
{
    public interface IPhantomJsRunner
    {
        string Run(IList<string> urls);

        string Run(IList<string> urls, IList<string> appendFileNames);

    }
}
