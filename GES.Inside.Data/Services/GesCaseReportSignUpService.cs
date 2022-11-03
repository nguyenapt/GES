using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Data.Services
{
    public class GesCaseReportSignUpService : EntityService<GesEntities, GesCaseReportSignUp>, IGesCaseReportSignUpService
    {
        private readonly GesEntities _dbContext;
        private readonly IGesCaseReportSignUpRepository _caseReportSignUpRepository;

        public GesCaseReportSignUpService(IUnitOfWork<GesEntities> unitOfWork,
            IGesCaseReportSignUpRepository caseReportSignUpRepository, IGesLogger logger) 
            : base(unitOfWork, logger, caseReportSignUpRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _caseReportSignUpRepository = caseReportSignUpRepository;
        }

        public GesCaseReportSignUp GetGesCaseReportSignUpById(long gesCaseReportId, long individualId)
        {
            return this.SafeExecute<GesCaseReportSignUp>(() => this._caseReportSignUpRepository.GetCaseReportSignUpBy(gesCaseReportId, individualId));
        }

        public bool RemoveGesCaseReportSignUp(long gesCaseReportId, long individualId)
        {
            this.SafeExecute(() => this._caseReportSignUpRepository.BatchDelete(d => d.I_GesCaseReports_Id == gesCaseReportId && d.G_Individuals_Id == individualId));

            return true;
        }

        public string GetUserResigned(long caseReportId, long orgId)
        {
            return this.SafeExecute<string>(() => _caseReportSignUpRepository.GetUserResigned(caseReportId, orgId));
        }


        #region Inside site

        public PaginatedResults<CaseReportSignUpListViewModel> GetCaseReportSignUpList(JqGridViewModel jqGridParams)
        {
            var query = this._caseReportSignUpRepository.GetCaseReportSignUpListViewModel();

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "companyname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.CompanyName).ThenBy(d=>d.CaseName)
                            : query.OrderByDescending(x => x.CompanyName).ThenBy(d=>d.CaseName);
                        break;
                    case "casename":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.CaseName)
                            : query.OrderByDescending(x => x.CaseName);
                        break;
                    case "organizationname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.OrganizationName).ThenBy(d => d.CompanyName).ThenBy(d => d.CaseName)
                            : query.OrderByDescending(x => x.OrganizationName).ThenBy(d => d.CompanyName).ThenBy(d => d.CaseName);
                        break;
                    case "fullname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.FullName)
                            : query.OrderByDescending(x => x.FullName);
                        break;
                    case "email":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Email)
                            : query.OrderByDescending(x => x.Email);
                        break;
                    case "endorsement":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Endorsement).ThenBy(d=>d.CompanyName).ThenBy(d=>d.CaseName)
                            : query.OrderByDescending(x => x.Endorsement).ThenBy(d => d.CompanyName).ThenBy(d => d.CaseName);
                        break;
                    default:
                        query = query.OrderBy(x => x.CompanyName).ThenBy(d => d.CaseName);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<CaseReportSignUpListViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public PaginatedResults<CaseReportSignUpUserListViewModel> GetCaseReportSignUpUserList(JqGridViewModel jqGridParams, long caseReportId)
        {
            var query = from s in _dbContext.GesCaseReportSignUp
                        join u in _dbContext.G_Users on s.G_Individuals_Id equals u.G_Individuals_Id
                        join i in _dbContext.G_Individuals on s.G_Individuals_Id equals i.G_Individuals_Id
                        join o in _dbContext.G_Organizations on i.G_Organizations_Id equals o.G_Organizations_Id
                        where s.I_GesCaseReports_Id == caseReportId
                        select new CaseReportSignUpUserListViewModel()
                        {
                            Id = s.GesCaseReportSignUpId,
                            UserName = u.Username,
                            OrganizationName = o.Name,
                            Email = i.Email,
                            SignUpValue = s.Active?"Active":"Passive"
                        };

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "organizationname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.OrganizationName).ThenBy(d => d.UserName)
                            : query.OrderByDescending(x => x.OrganizationName).ThenBy(d => d.UserName);
                        break;
                    case "username":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.UserName)
                            : query.OrderByDescending(x => x.UserName);
                        break;
                    case "email":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Email)
                            : query.OrderByDescending(x => x.Email);
                        break;
                    case "signupvalue":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.SignUpValue)
                            : query.OrderByDescending(x => x.SignUpValue);
                        break;
                    default:
                        query = query.OrderBy(x => x.UserName);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<CaseReportSignUpUserListViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public List<CaseReportSignUpListViewModel> ExportEndorsements()
        {
            var query = from s in _dbContext.GesCaseReportSignUp
                join rp in _dbContext.I_GesCaseReports on s.I_GesCaseReports_Id equals rp.I_GesCaseReports_Id
                join gc in _dbContext.I_GesCompanies on rp.I_GesCompanies_Id equals gc.I_GesCompanies_Id
                join c in _dbContext.I_Companies on gc.I_Companies_Id equals c.I_Companies_Id
                join i in _dbContext.G_Individuals on s.G_Individuals_Id equals i.G_Individuals_Id
                join o in _dbContext.G_Organizations on s.G_Organizations_Id equals o.G_Organizations_Id

                select new CaseReportSignUpListViewModel()
                {
                    Id = s.GesCaseReportSignUpId,
                    CaseProfileId = rp.I_GesCaseReports_Id,
                    CaseName = rp.ReportIncident,
                    CompanyName = c.Name,
                    CompanyId = c.I_Companies_Id,
                    SustainalyticsID = c.Id,
                    OrganizationName = o.Name,
                    FullName = i.FirstName + " " + i.LastName,
                    Email = i.Email,
                    Endorsement = s.Active ? "Active" : "Passive"
                };

            return query.OrderBy(d=>d.CompanyName).ThenBy(d=>d.CaseName).ToList();
        }

        #endregion

        }
}