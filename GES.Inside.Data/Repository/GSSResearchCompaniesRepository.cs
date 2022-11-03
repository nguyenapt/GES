using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Linq;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.StoredProcedureParams;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Repository
{
    public class GSSResearchCompaniesRepository : GenericRepository<I_Companies>, IGSSResearchCompaniesRepository
    {
        private readonly GesEntities _dbContext;
        private readonly IStoredProcedureRunner _storedProcedureRunner;
        
        public GSSResearchCompaniesRepository(GesEntities context, IGesLogger logger, IStoredProcedureRunner storedProcedureRunner) : base(context, logger)
        {
            _dbContext = context;
            _storedProcedureRunner = storedProcedureRunner;
        }

        public IQueryable<GssResearchCompanyViewModel> GetGssResearchCompanies()
        {
            var companies = from c in _dbContext.I_Companies
                join gc in _dbContext.I_GesCompanies on c.I_Companies_Id equals gc.I_Companies_Id into gcg from gc in gcg.DefaultIfEmpty()
                join rp in _dbContext.I_GesCaseReports on gc.I_GesCompanies_Id equals rp.I_GesCompanies_Id
                into grp from rp in grp.DefaultIfEmpty()
                where (rp.I_NormAreas_Id == null || rp.I_NormAreas_Id != 6) && !string.IsNullOrEmpty(c.Name)
                group rp by new
                {
                    c.I_Companies_Id,
                    c.IsParkedForGssResearchSince,
                    c.Name,
                    c.MasterI_Companies_Id,
                    c.Isin,
                    c.Sedol,
                    c.Website,
                    Location = c.Countries.Name,
                    sortPath = c.I_Companies2 != null
                            ? c.I_Companies2.Name.Trim() + "~~" + c.Name.Trim()
                            : c.Name.Trim()
                }
                into g
                select new GssResearchCompanyViewModel
                {
                    Id = g.Key.I_Companies_Id,
                    Name = g.Key.Name,
                    WorkflowStatus = "Published",
                    Assessment = "Compliant",
                    Analyst = "Aleksandra Krywko",
                    Reviewer = "Paulina Segreto",
                    Flags = g.Key.IsParkedForGssResearchSince != null?"Disabled": "Enabled",
                    GssocReview = "Yes",
                    IsParked = g.Key.IsParkedForGssResearchSince != null

                };
            return companies;
        }

       
    }
}
