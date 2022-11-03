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
    public class I_EngagementTypeNewsService : EntityService<GesEntities, I_EngagementTypeNews>, II_EngagementTypeNewsService
    {
        private readonly GesEntities _dbContext;
        private readonly II_EngagementTypeNewsRepository _engagementTypeNewsRepository;

        public I_EngagementTypeNewsService(IUnitOfWork<GesEntities> unitOfWork, II_EngagementTypeNewsRepository engagementTypeNewsRepository, IGesLogger logger)
            : base(unitOfWork, logger, engagementTypeNewsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _engagementTypeNewsRepository = engagementTypeNewsRepository;
        }


        public I_EngagementTypeNews GetById(long id)
        {
            return _engagementTypeNewsRepository.GetById(id);
        }

        public IEnumerable<I_EngagementTypeNews> GetEngagementTypesNewsByEngagementTypesId(long engagementTypesId)
        {
            var news = from n in _dbContext.I_EngagementTypeNews
                where n.I_EngagementTypes_Id == engagementTypesId
                select n;

            return news;
        }
    }
}
