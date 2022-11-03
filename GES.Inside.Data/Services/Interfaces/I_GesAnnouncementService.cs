using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface I_GesAnnouncementService : IEntityService<GesAnnouncement>
    {
        GesAnnouncement GetById(Guid id);

        IList<GesAnnouncementModel> GetModels();

        IList<GesAnnouncementModel> GetRssModels(string url);
    }
}
