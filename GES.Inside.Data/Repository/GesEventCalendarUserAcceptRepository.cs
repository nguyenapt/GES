using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;
using System;

namespace GES.Inside.Data.Repository
{
    public class GesEventCalendarUserAcceptRepository : GenericRepository<GesEventCalendarUserAccept>, I_GesEventCalendarUserAcceptRepository
    {
        public GesEventCalendarUserAcceptRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public IEnumerable<GesEventCalendarUserAccept> GetAttendees(long eventId)
        {
            return _dbset.Where(d => d.I_CalenderEvents_Id == eventId);
        }

        public GesEventCalendarUserAccept GetByEventIdAndEmail(long eventId, string email)
        {
            return _dbset.Where(d => d.I_CalenderEvents_Id == eventId && d.Email ==email).FirstOrDefault();
        }

        public GesEventCalendarUserAccept GetById(Guid id)
        {
            return this.SafeExecute<GesEventCalendarUserAccept>(() => _entities.Set<GesEventCalendarUserAccept>().FirstOrDefault(d => d.GesEventCalendarUserAcceptId == id));
        }
    }
}
