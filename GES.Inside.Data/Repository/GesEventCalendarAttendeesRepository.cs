using System;
using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository
{
    public class GesEventCalendarAttendeesRepository : GenericRepository<GesEventCalendarUserAccept>, IGesEventCalendarAttendeesRepository
    {
        public GesEventCalendarAttendeesRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public GesEventCalendarUserAccept GetById(Guid id)
        {
            return this.SafeExecute<GesEventCalendarUserAccept>(() => _entities.Set<GesEventCalendarUserAccept>().FirstOrDefault(d => d.GesEventCalendarUserAcceptId == id));
        }

        public IEnumerable<GesEventCalendarUserAccept> GetCalendarsAttendees(long calenderEventsId)
        {
            return _dbset.Where(d => d.I_CalenderEvents_Id == calenderEventsId);
        }

        public void DeleteRange(long eventIds)
        {
            var eventsAttendees = _entities.Set<GesEventCalendarUserAccept>().Where(i => i.I_CalenderEvents_Id == eventIds);
            this.SafeExecute(() => _entities.Set<GesEventCalendarUserAccept>().RemoveRange(eventsAttendees));
        }
    }
}
