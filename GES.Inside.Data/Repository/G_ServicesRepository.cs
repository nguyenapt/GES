using System.Linq;
using DocumentFormat.OpenXml.Drawing.Charts;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Repository
{
    public class G_ServicesRepository : GenericRepository<G_Services>, IG_ServicesRepository
    {
        private readonly GesEntities _dbContext;

        public G_ServicesRepository(GesEntities context, IGesLogger logger)
            : base(context, logger)
        {
            _dbContext = context;
        }

        public G_Services GetById(long id)
        {
            return this.SafeExecute<G_Services>(() =>
                _entities.Set<G_Services>().FirstOrDefault(d => d.G_Services_Id == id));
        }

        public IQueryable<ServicesModel> GetGesServices()
        {
            var gesServices = from s in _dbContext.G_Services
                join e in _dbContext.I_EngagementTypes on s.I_EngagementTypes_Id equals e.I_EngagementTypes_Id into sg
                from e in sg.DefaultIfEmpty()
                select new ServicesModel
                {
                    GServicesId = s.G_Services_Id,
                    Name = s.Name,
                    Url = s.Url,
                    ReportLetter = s.ReportLetter,
                    ShowInClient = s.ShowInClient,
                    ShowInNavigation = s.ShowInNavigation,
                    EngagementTypesId = s.I_EngagementTypes_Id,
                    Sort = s.Sort,
                    EngagementTypesName = e.Name
                };

            return gesServices;
        }
    }
}
