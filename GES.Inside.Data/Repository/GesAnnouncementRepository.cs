using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository
{
    public class GesAnnouncementRepository : GenericRepository<GesAnnouncement>, I_GesAnnouncementRepository
    {
        public GesAnnouncementRepository(GesEntities context, IGesLogger logger): base(context, logger)
        {

        }

        public GesAnnouncement GetById(Guid id)
        {
            return this.SafeExecute<GesAnnouncement>(() => _entities.Set<GesAnnouncement>().FirstOrDefault(d => d.GesAnnouncementId == id));
        }
    }
}
