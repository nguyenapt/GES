using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using com.sun.org.apache.bcel.@internal.generic;
using GES.Common.Configurations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using ikvm.extensions;
using sun.net.www.content.image;

namespace GES.Inside.Data.Services
{
    public class CalendarService : EntityService<GesEntities, I_CalenderEvents>, ICalendarService
    {
        private readonly GesEntities _dbContext;
        private readonly II_CalendarEventsRepository _calendarReporsitory;

        public CalendarService(IUnitOfWork<GesEntities> unitOfWork, II_CalendarEventsRepository calendarReporsitory, IGesLogger logger) : base(unitOfWork, logger, calendarReporsitory)
        {
            _dbContext = unitOfWork.DbContext;
            _calendarReporsitory = calendarReporsitory;
        }

        public IList<EventListViewModel> GetCalendarEventsByCompanyId(long companyId)
        {
            var events = this.SafeExecute<IList<EventListViewModel>>(() =>
                this._calendarReporsitory.GetCalendars(companyId)
                    .Select(x => new EventListViewModel
                    {
                        Id = x.I_CalenderEvents_Id,
                        CompanyId = x.I_Companies_Id,
                        Heading = x.EventTitle ?? (x.Description?.length() > 100 ? x.Description?.substring(0, 100) : x.Description),
                        Description = x.Description,
                        EventDate = x.EventDate,
                        IsGesEvent = x.GesEvent,
                        EventTitle = x.EventTitle,
                        EventLocation = x.EventLocation,
                        EventEndDate = x.EventEndDate,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        AllDayEvent = x.AllDayEvent
                    }).ToList());

            var calendarEventsByCompanyId = events.ToList();
            foreach (var e in calendarEventsByCompanyId)
            {
                e.Attendees = GetAttendees(e.Id);
            }

            return calendarEventsByCompanyId;
        }

        public EventListViewModel GetCalendarEventById(long? eventId)
        {
            var result = this.SafeExecute(() => _calendarReporsitory.FindBy(x => x.I_CalenderEvents_Id == eventId).Select(x => new EventListViewModel
            {
                Id = x.I_CalenderEvents_Id,
                CompanyId = x.I_Companies_Id,
                Heading = x.EventTitle ?? (x.Description?.length() > 100 ? x.Description?.substring(0, 100) : x.Description),
                Description = x.Description,
                EventDate = x.EventDate,
                IsGesEvent = x.GesEvent,
                EventLocation = x.EventLocation,
                IsCompanyEvent = true,
                EventEndDate = x.EventEndDate,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                AllDayEvent = x.AllDayEvent
            }).FirstOrDefault());

            result.Attendees = GetAttendees(result.Id);

            return result;

        }

        public EventListViewModel GetCalendarEventById(long? eventId, bool companyEvent)
        {
            if (companyEvent)
            {
                var result = this.SafeExecute(() => _calendarReporsitory.FindBy(x => x.I_CalenderEvents_Id == eventId).Select(x => new EventListViewModel
                {
                    Id = x.I_CalenderEvents_Id,
                    CompanyId = x.I_Companies_Id,
                    Heading = x.EventTitle ?? (x.Description?.length() > 100 ? x.Description?.substring(0, 100) : x.Description),
                    Description = x.Description,
                    EventDate = x.EventDate,
                    IsGesEvent = x.GesEvent,
                    EventLocation = x.EventLocation,
                    IsCompanyEvent = true,
                    EventEndDate = x.EventEndDate,
                    StartTime = x.StartTime,
                    EndTime = x.EndTime,
                    AllDayEvent = x.AllDayEvent
                }).FirstOrDefault());

                result.Attendees = GetAttendees(result.Id);
                return result;
            }

            var engagementTypeTimeLine = (from e in _dbContext.I_EngagementTypes
                    join t in _dbContext.I_TimelineItems on e.I_EngagementTypes_Id equals t.I_EngagementTypes_Id
                    where t.I_TimelineItems_Id == eventId
                    select new EventListViewModel
                    {
                        EventDate = t.Date ?? DateTime.Now,
                        Heading = t.Description,
                        CompanyNameOrEngagementName = e.Name,
                        EngagementTypeId = t.I_EngagementTypes_Id,
                        EventLocation = "",
                        Id = t.I_TimelineItems_Id,
                        Description = t.Description,
                        AllDayEvent = true,
                        Created = t.Created,
                        IsCompanyEvent = false
                    }
                ).FirstOrDefault();

            if (engagementTypeTimeLine != null)
            {
                engagementTypeTimeLine.Heading = (engagementTypeTimeLine.Heading?.length() > 100
                    ? engagementTypeTimeLine.Heading.substring(0, 100)
                    : engagementTypeTimeLine.Heading);
                
            }

            return engagementTypeTimeLine;
        }

        public IEnumerable<EventAttendeeModel> GetAttendees(long? eventId)
        {
            var query = from c in _dbContext.GesEventCalendarUserAccept where c.I_CalenderEvents_Id == (long)eventId select c;
            query = query.OrderBy(x => x.Email);

            var result = this.SafeExecute<IEnumerable<GesEventCalendarUserAccept>>(() => query);

            return result.Select(x => new EventAttendeeModel
            {
                Email = x.Email,
                FullName = x.FullName,
                IsSentUpdate = x.IsSentUpdate,
                SendDate = x.SendDate,
                UpdateSentDate = x.UpdateSentDate,
                GesEventCalendarUserAcceptId = x.GesEventCalendarUserAcceptId,
                I_CalenderEvents_Id = x.I_CalenderEvents_Id

            }).ToList();
        }

        public IEnumerable<EventListViewModel> GetCalendarEvents()
        {
            var companyEvents = (from ca in _dbContext.I_CalenderEvents
                join c in _dbContext.I_Companies on ca.I_Companies_Id equals c.I_Companies_Id
                select new EventListViewModel
                {
                    Id = ca.I_CalenderEvents_Id,
                    CompanyId = ca.I_Companies_Id,
                    CompanyNameOrEngagementName = c.Name,
                    Heading = ca.EventTitle, //?? (ca.Description != null && ca.Description.length() > 100? ca.Description.substring(0, 100): ca.Description),
                    Description = ca.Description,
                    EventDate = ca.EventDate,
                    IsGesEvent = ca.GesEvent,
                    EventTitle = ca.EventTitle,
                    EventLocation = ca.EventLocation,
                    EventEndDate = ca.EventEndDate,
                    StartTime = ca.StartTime,
                    EndTime = ca.EndTime,
                    AllDayEvent = ca.AllDayEvent,
                    IsCompanyEvent = true
                }).ToList();

            var result = companyEvents.Count > 0 ? companyEvents : new List<EventListViewModel>();
            
            var engagementTypeTimeLine = (from e in _dbContext.I_EngagementTypes
                    join t in _dbContext.I_TimelineItems on e.I_EngagementTypes_Id equals t.I_EngagementTypes_Id
                    select new EventListViewModel
                    {
                        EventDate = t.Date ?? DateTime.Now,
                        Heading = t.Description,
                        CompanyNameOrEngagementName = e.Name,
                        EngagementTypeId = t.I_EngagementTypes_Id,
                        EventLocation = "",
                        Id = t.I_TimelineItems_Id,
                        Created = t.Created,
                        IsCompanyEvent = false
                    }
                ).ToList();

            foreach (var item in engagementTypeTimeLine)
            {
                item.AllDayEvent = (item.EventDate.Hour == 0);
                result.Add(item);
            }

            return result.OrderByDescending(d => d.EventDate);
        }
    }
}