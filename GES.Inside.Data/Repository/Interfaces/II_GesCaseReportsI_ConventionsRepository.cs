using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_GesCaseReportsI_ConventionsRepository : IGenericRepository<I_GesCaseReportsI_Conventions>
    {
        IEnumerable<I_GesCaseReportsI_Conventions> GetGesCaseReportsConventionsByCaseReportId(long gesCaseReportsId);

        I_GesCaseReportsI_Conventions GetById(long id);

        IList<ConventionModel> GetAllConventions();
    }
}
