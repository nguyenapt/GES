using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesCaseReportSignUpRepository : IGenericRepository<GesCaseReportSignUp>
    {
        GesCaseReportSignUp GetById(long id);

        string GetUserResigned(long caseReportId, long orgId);

        GesCaseReportSignUp GetCaseReportSignUpBy(long caseReportId, long invididualId);

        IEnumerable<CaseReportSignUpListViewModel> GetCaseReportSignUpListViewModel();
    }
}
