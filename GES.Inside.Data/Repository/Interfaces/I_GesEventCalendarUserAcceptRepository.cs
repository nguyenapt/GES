using GES.Inside.Data.DataContexts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface I_GesEventCalendarUserAcceptRepository : IGenericRepository<GesEventCalendarUserAccept>
    {
        GesEventCalendarUserAccept GetById(Guid id);

        GesEventCalendarUserAccept GetByEventIdAndEmail(long eventId, string email);

        IEnumerable<GesEventCalendarUserAccept> GetAttendees(long eventId);
    }
}
