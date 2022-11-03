using System.Data.Entity;
using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository
{
    public class I_CalendarEventsRepository : GenericRepository<I_CalenderEvents>, II_CalendarEventsRepository
    {
        public I_CalendarEventsRepository(GesEntities context, IGesLogger logger)
           : base(context, logger)
        {

        }

        public I_CalenderEvents GetById(long id)
        {
            return this.SafeExecute<I_CalenderEvents>(() => _entities.Set<I_CalenderEvents>().FirstOrDefault(d => d.I_CalenderEvents_Id == id));
        }

        public IEnumerable<I_CalenderEvents> GetCalendars(long companyId)
        {
            return _dbset.Where(d => d.I_Companies_Id == companyId);
        }

        public IEnumerable<I_CalenderEvents> GetAllCalendars()
        {
            return _dbset;
        }
    }
}
