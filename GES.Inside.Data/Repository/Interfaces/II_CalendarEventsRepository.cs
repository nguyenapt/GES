using GES.Inside.Data.DataContexts;
using System.Collections;
using System.Collections.Generic;

namespace GES.Inside.Data.Repository.Interfaces
{
    public interface II_CalendarEventsRepository : IGenericRepository<I_CalenderEvents>
    {
        I_CalenderEvents GetById(long id);
        IEnumerable<I_CalenderEvents> GetCalendars(long companyId);
        IEnumerable<I_CalenderEvents> GetAllCalendars();
    }
}
