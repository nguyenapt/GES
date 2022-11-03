using System;
using System.Collections.Generic;
using System.Linq;
using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Data.Repository
{
    public class GesCaseReportSdgRepository : GenericRepository<GesCaseReportSdg>, IGesCaseReportSdgRepository
    {
        private readonly GesEntities _dbContext;

        public GesCaseReportSdgRepository(GesEntities context, IGesLogger logger) : base(context, logger)
        {
            _dbContext = context;
        }

        public IList<long> GetSdgIdsByCaseProfile(long caseProfileId)
        {
            return _dbContext.GesCaseReportSdg.Where(x => x.GesCaseReport_Id == caseProfileId).OrderBy(x => x.SortOrder).Select(x => x.Sdg_Id).ToList();
        }


        public GesCaseReportSdg GetCaseReportSdg(long caseProfileId, long sdgId)
        {
            return _dbContext.GesCaseReportSdg.FirstOrDefault(x => x.GesCaseReport_Id == caseProfileId && x.Sdg_Id == sdgId);
        }

        public GesUNGPAssessmentForm GesUngpAssessmentByCaseProfile(long? id)
        {
            return _dbContext.GesUNGPAssessmentForm.FirstOrDefault(x => x.I_GesCaseReports_Id == id);
        }

        public bool TryUpdateSdgsForCaseProfile(long caseProfileId, IList<long> sdgIds)
        {
            var oldSdgIds = GetSdgIdsByCaseProfile(caseProfileId);
            var deletedSdgIds = sdgIds == null || sdgIds.Count == 0 ? oldSdgIds : oldSdgIds.Where(x => !sdgIds.Contains(x)).ToList();
            try
            {
                UpdateNewSdgForCaseProfile(caseProfileId, sdgIds);
                DeleteSdgForCaseProfile(caseProfileId, deletedSdgIds);
                return true;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to update sdg");
                return false;
            }
        }

        private void DeleteSdgForCaseProfile(long caseProfileId, IList<long> sdgIds)
        {
            foreach (var sdgId in sdgIds)
            {
                var gesCaseReportSdg = GetCaseReportSdg(caseProfileId, sdgId);
                Delete(gesCaseReportSdg);
            }
            Save();
        }

        private void UpdateNewSdgForCaseProfile(long caseProfileId, IList<long> sdgIds)
        {
            if (sdgIds == null || sdgIds.Count == 0)
                return;

            foreach (var sdgId in sdgIds)
            {
                var gesCaseReportSdg = GetCaseReportSdg(caseProfileId, sdgId);
                if (gesCaseReportSdg != null)
                {
                    gesCaseReportSdg.SortOrder = sdgIds.IndexOf(sdgId);
                    Edit(gesCaseReportSdg);
                }
                else
                {
                    Add(new GesCaseReportSdg
                    {
                        GesCaseReport_Id = caseProfileId,
                        Sdg_Id = sdgId,
                        SortOrder = sdgIds.IndexOf(sdgId)
                    });
                }
            }
            Save();
        }
    }
}
