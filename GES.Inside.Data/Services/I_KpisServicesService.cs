using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using Z.EntityFramework.Plus;

namespace GES.Inside.Data.Services
{
    public class I_KpisServicesService : EntityService<GesEntities, I_Kpis>, II_KpisService
    {
        private readonly GesEntities _dbContext;
        private readonly II_KpisRepository _kpisRepository;

        public I_KpisServicesService(IUnitOfWork<GesEntities> unitOfWork, II_KpisRepository kpisRepository , IGesLogger logger)
            : base(unitOfWork, logger, kpisRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _kpisRepository = kpisRepository;
        }
        public I_Kpis GetById(long id)
        {
            return _kpisRepository.GetById(id);
        }

        public IEnumerable<I_Kpis> GetKpisByEngagementTypesId(long engagementTypesId)
        {
            var kpises = from n in _dbContext.I_Kpis
                       where n.I_EngagementTypes_Id == engagementTypesId
                select n;

            return kpises;
        }
    }
}
