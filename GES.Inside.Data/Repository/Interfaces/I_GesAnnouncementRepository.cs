using GES.Inside.Data.DataContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface I_GesAnnouncementRepository: IGenericRepository<GesAnnouncement>
    {
        GesAnnouncement GetById(Guid id);
    }
}
