using System;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using System.Globalization;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models.CaseProfiles;
using ikvm.extensions;

namespace GES.Inside.Data.Repository
{
    public class GesUngpAssessmentFormRepository : GenericRepository<GesUNGPAssessmentForm>, IGesUngpAssessmentFormRepository
    {
        private readonly GesEntities _dbContext;
        public GesUngpAssessmentFormRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public GesUNGPAssessmentForm GetById(Guid id)
        {
            return this.SafeExecute<GesUNGPAssessmentForm>(() => _entities.Set<GesUNGPAssessmentForm>().FirstOrDefault(d => d.GesUNGPAssessmentFormId == id));
        }

        public GesUNGPAssessmentForm GetGesUngpAssessmentFormByCaseProfileId(long caseProfileId)
        {
            return this.SafeExecute<GesUNGPAssessmentForm>(() => _entities.Set<GesUNGPAssessmentForm>().FirstOrDefault(d => d.I_GesCaseReports_Id == caseProfileId));
        }

        public GesUngpAuditViewModel GetGesUngpAssessmentFormHistoryByUngpId(long caseProfileId)
        {
            var auditViewModel = new GesUngpAuditViewModel();

            var upgpFormHistories = (from ufa in _dbContext.GesUNGPAssessmentForm_Audit
                where ufa.I_GesCaseReports_Id == caseProfileId
                select new GesUngpAssessmentFormAuditViewModel
                {
                    GesUngpAssessmentFormAuditId = ufa.GesUNGPAssessmentForm_Audit_Id,
                    GesUngpAssessmentFormId = ufa.GesUNGPAssessmentFormId,
                    GesCaseReportsId = ufa.I_GesCaseReports_Id,
                    AuditDmlAction = ufa.AuditDMLAction,
                    AuditDatetime = ufa.AuditDatetime,
                    AuditUser = ufa.AuditUser,

                }).OrderByDescending(x =>x.AuditDatetime).ToList();

            foreach (var history in upgpFormHistories)
            {
                var upgpFormHistoryDetails = (from d in _dbContext.GesUNGPAssessmentForm_Audit_Details
                    where d.GesUNGPAssessmentForm_Audit_Id == history.GesUngpAssessmentFormAuditId
                    select new GesUngpAssessmentFormAuditDetailsViewModel
                    {
                        GesUngpAssessmentFormAuditDetailsId = d.GesUNGPAssessmentForm_Audit_Details_Id,
                        GesUngpAssessmentFormAuditId = d.GesUNGPAssessmentForm_Audit_Id,
                        ColumnName = d.ColumnName,
                        NewValue = d.NewValue,
                        OldValue = d.OldValue,
                        AuditDataState = d.AuditDataState,
                        AuditUser = d.AuditUser,
                        ColumnNameDescription = d.ColumnNameDescription,
                        AuditDatetime = d.AuditDatetime

                    }).ToList();
                history.GesUngpAssessmentFormAuditDetailsViewModels = upgpFormHistoryDetails;

                var ungpResources = (from rs in _dbContext.GesUNGPAssessmentFormResource_Audit
                        where rs.GesUNGPAssessmentFormId == history.GesUngpAssessmentFormId && rs.AuditUser == history.AuditUser
                              && rs.AuditDatetime.Value.Year == history.AuditDatetime.Value.Year && rs.AuditDatetime.Value.Day == history.AuditDatetime.Value.Day && rs.AuditDatetime.Value.Month == history.AuditDatetime.Value.Month && rs.AuditDatetime.Value.Hour == history.AuditDatetime.Value.Hour && rs.AuditDatetime.Value.Minute == history.AuditDatetime.Value.Minute
                        select new GesUNGPAssessmentFormResource_Audit
                        {
                            

                        }
                    );

            }

            auditViewModel.GesUngpAssessmentFormAuditViewModels = upgpFormHistories;

            return auditViewModel;
        }
       
    }
}
