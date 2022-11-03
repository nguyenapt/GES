using System;
using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface IGesEventCalendarAttendeesRepository : IGenericRepository<GesEventCalendarUserAccept>
    {
        GesEventCalendarUserAccept GetById(Guid id);
        IEnumerable<GesEventCalendarUserAccept> GetCalendarsAttendees(long calenderEventsId);
        void DeleteRange(long eventIds);
    }
}
