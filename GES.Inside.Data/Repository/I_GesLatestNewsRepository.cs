using System;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class I_GesLatestNewsRepository : GenericRepository<I_GesLatestNews>, II_GesLatestNewsRepository
    {
        private readonly GesEntities _dbContext;
        public I_GesLatestNewsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }


        public IEnumerable<I_GesLatestNews> GetGesLatestNewsByCaseProfileId(long gesCaseReportsId){
                                        
            var latestNewsList = from n in _dbContext.I_GesLatestNews
                where n.I_GesCaseReports_Id == gesCaseReportsId
                select n;

            return latestNewsList;
        }

        public I_GesLatestNews GetById(long id)
        {
            return this.SafeExecute<I_GesLatestNews>(() => _entities.Set<I_GesLatestNews>().FirstOrDefault(d => d.I_GesLatestNews_Id == id));
        }
    }
}
