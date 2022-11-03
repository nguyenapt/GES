using System;
using System.Collections.Generic;
using GES.Common.Configurations;

namespace GES.Inside.Data.Models
{
    public class EventListViewModel
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string CompanyNameOrEngagementName { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime EventDate { get; set; }

        public string EventDateString => EventDate.ToString(Configurations.DateFormat) ?? string.Empty;
        public bool IsGesEvent { get; set; }
        public long? EngagementTypeId { get; set; }
        public DateTime Created { get; set; }
        public string EventTitle { get; set; }
        public string EventLocation { get; set; }
        public DateTime? EventEndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool? AllDayEvent { get; set; }
        public bool IsCompanyEvent { get; set; }
        
        public IEnumerable<EventAttendeeModel> Attendees { get; set; }
    }
}