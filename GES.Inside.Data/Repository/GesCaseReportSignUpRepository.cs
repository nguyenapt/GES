using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Models;
using System.Collections.Generic;
using System.Linq.Dynamic;

namespace GES.Inside.Data.Repository
{
    public class GesCaseReportSignUpRepository : GenericRepository<GesCaseReportSignUp>, IGesCaseReportSignUpRepository
    {
        private GesEntities _dbContext;
        public GesCaseReportSignUpRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public GesCaseReportSignUp GetById(long id)
        {
            return this.SafeExecute<GesCaseReportSignUp>(() => _entities.Set<GesCaseReportSignUp>().FirstOrDefault(d => d.GesCaseReportSignUpId == id));
        }

        public GesCaseReportSignUp GetCaseReportSignUpBy(long caseReportId, long invididualId)
        {
            return _dbset.FirstOrDefault(d => d.I_GesCaseReports_Id == caseReportId && d.G_Individuals_Id == invididualId);
        }

        public IEnumerable<CaseReportSignUpListViewModel> GetCaseReportSignUpListViewModel()
        {
            return from s in _dbContext.GesCaseReportSignUp
                   join rp in _dbContext.I_GesCaseReports on s.I_GesCaseReports_Id equals rp.I_GesCaseReports_Id
                   join gc in _dbContext.I_GesCompanies on rp.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                   join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                   join i in _dbContext.G_Individuals on s.G_Individuals_Id equals i.G_Individuals_Id into gi from i in gi.DefaultIfEmpty()
                   join o in _dbContext.G_Organizations on s.G_Organizations_Id equals o.G_Organizations_Id

                   select new CaseReportSignUpListViewModel()
                   {
                       Id = s.GesCaseReportSignUpId,
                       CaseProfileId = rp.I_GesCaseReports_Id,
                       CaseName = rp.ReportIncident,
                       CompanyName = c.Name,
                       CompanyId = c.I_Companies_Id,
                       OrganizationName = o.Name,
                       FullName = i != null? i.FirstName + " " + i.LastName: "",
                       Email = i!= null ? i.Email : "",
                       Endorsement = s.Active ? "Active" : "Passive"
                   };
        }

        public string GetUserResigned(long caseReportId, long orgId)
        {
            var result = from s in _dbContext.GesCaseReportSignUp
                join o in _dbContext.G_Organizations on s.G_Organizations_Id equals o.G_Organizations_Id
                where s.I_GesCaseReports_Id == caseReportId && s.Active
                select o.Name;

            return this.SafeExecute<string>(() =>
            {
                return string.Join(", ", result.Distinct().ToList());
            });
        }

    }
}
