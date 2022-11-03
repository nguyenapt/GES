using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services.Interfaces
{
    public interface ICalendarService : IEntityService<I_CalenderEvents>
    {
        IList<EventListViewModel> GetCalendarEventsByCompanyId(long companyId);
        EventListViewModel GetCalendarEventById(long? eventId);
        EventListViewModel GetCalendarEventById(long? eventId, bool companyEvent);
        IEnumerable<EventAttendeeModel> GetAttendees(long? eventId);
        IEnumerable<EventListViewModel> GetCalendarEvents();
    }
}
