using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository
{
    public class I_GesAssociatedCorporationsRepository : GenericRepository<I_GesAssociatedCorporations>, II_GesAssociatedCorporationsRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesAssociatedCorporationsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public IList<AssociatedCorporationsViewModel> GetAssociatedCorporationsByCaseProfile(long caseprofileId)
        {
            return SafeExecute(() =>
            {
                var associatedCorporations = from asc in _dbContext.I_GesAssociatedCorporations
                                       where asc.I_GesCaseReports_Id == caseprofileId
                                       select new AssociatedCorporationsViewModel()
                                       {
                                           AssociatedCorporationId = asc.I_GesAssociatedCorporations_Id,
                                           CaseReportId = asc.I_GesCaseReports_Id,
                                           Name = asc.Name,
                                           Ownership = asc.Ownership,
                                           Comment = asc.Comment,
                                           Traded = asc.Traded,
                                           ShowInReport = asc.ShowInReport == 1 ? true : false
                                       };
                return associatedCorporations.ToList();
            });
        }

        public I_GesAssociatedCorporations GetById(long id)
        {
            return this.SafeExecute<I_GesAssociatedCorporations>(() => _entities.Set<I_GesAssociatedCorporations>().Find(id));
        }
    }
}
