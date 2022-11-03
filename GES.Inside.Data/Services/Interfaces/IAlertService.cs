using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface IAlertService : IEntityService<I_NaArticles>
    {
        IEnumerable<AlertListViewModel> GetAlertsByCompanyId(long companyId);
        AlertListViewModel GetAlertById(long id);
    }
}
