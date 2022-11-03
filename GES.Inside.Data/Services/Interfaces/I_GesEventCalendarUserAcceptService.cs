using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface I_GesEventCalendarUserAcceptService : IEntityService<GesEventCalendarUserAccept>
    {
        GesEventCalendarUserAccept GetById(Guid id);
        GesEventCalendarUserAccept GetByEventIdAndEmail(long eventId, string email);
        IEnumerable<GesEventCalendarUserAccept> GetAttendees(long eventId);
    }
}
