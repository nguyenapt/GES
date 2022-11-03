using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using GES.Common.Resources;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Models.Anonymous;
using GES.Inside.Data.Models.StoredProcedureParams;

namespace GES.Inside.Data.Services
{
    public class GSSResearchCompaniesService : EntityService<GesEntities, I_Companies>, IGSSResearchCompaniesService
    {
        private readonly GesEntities _dbContext;
        private readonly IGSSResearchCompaniesRepository _gssResearchCompaniesRepository;


        public GSSResearchCompaniesService(IUnitOfWork<GesEntities> unitOfWork, IGSSResearchCompaniesRepository gssResearchCompaniesRepository, IGesLogger logger)
            : base(unitOfWork, logger, gssResearchCompaniesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gssResearchCompaniesRepository = gssResearchCompaniesRepository;

        }


        public PaginatedResults<GssResearchCompanyViewModel> GetGSSResearchCompanies(JqGridViewModel jqGridParams)
        {
            
            var companies = _gssResearchCompaniesRepository.GetGssResearchCompanies();
            
            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GssResearchCompanyViewModel>(jqGridParams);
                companies = string.IsNullOrEmpty(finalRules) ? companies : companies.Where(finalRules);
            }
            
            var sortedColumn = jqGridParams.sidx.ToLower();
            var sortedDirection = jqGridParams.sord.ToLower();
            
            if (!(string.IsNullOrEmpty(sortedColumn) && string.IsNullOrEmpty(sortedDirection)))
            {
                switch (sortedColumn)
                {
                    case "name":
                        companies = sortedDirection == "asc"
                            ? companies.OrderBy(x => x.Name)
                            : companies.OrderByDescending(x => x.Name);
                        break;
                    case "workflowstatus":
                        companies = sortedDirection == "asc"
                            ? companies.OrderBy(x => x.WorkflowStatus)
                            : companies.OrderByDescending(x => x.WorkflowStatus);
                        break;
                    case "assessment":
                        companies = sortedDirection == "asc"
                            ? companies.OrderBy(x => x.Assessment)
                            : companies.OrderByDescending(x => x.Assessment);
                        break;                    
                    case "analyst":
                        companies = sortedDirection == "asc"
                            ? companies.OrderBy(x => x.Analyst)
                            : companies.OrderByDescending(x => x.Analyst);
                        break;                    
                    case "reviewer":
                        companies = sortedDirection == "asc"
                            ? companies.OrderBy(x => x.Reviewer)
                            : companies.OrderByDescending(x => x.Reviewer);
                        break;
                    case "flags":
                        companies = sortedDirection == "asc"
                            ? companies.OrderBy(x => x.Flags)
                            : companies.OrderByDescending(x => x.Flags);
                        break;                    
                    case "gssocreview":
                        companies = sortedDirection == "asc"
                            ? companies.OrderBy(x => x.GssocReview)
                            : companies.OrderByDescending(x => x.GssocReview);
                        break;
                    
                    default:
                        companies = companies.OrderByDescending(d => d.Name);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GssResearchCompanyViewModel>(jqGridParams);
                companies = String.IsNullOrEmpty(finalRules) ? companies : companies.Where(finalRules);
            }

            var result = companies.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
            
            return result;
        }
       
    }
}
