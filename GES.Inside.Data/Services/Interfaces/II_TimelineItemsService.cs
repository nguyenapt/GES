using System.Collections.Generic;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface II_TimelineItemsService : IEntityService<I_TimelineItems>
    {
        I_TimelineItems GetById(long id);
        IEnumerable<I_TimelineItems> GetTimelineItemsByEngagementTypesId(long engagementTypesId);
    }
}
