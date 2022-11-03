using System;

namespace GES.Inside.Data.Models
{
    public class EventAttendeeModel
    {
        public Guid GesEventCalendarUserAcceptId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? SendDate { get; set; }
        public long I_CalenderEvents_Id { get; set; }
        public bool? IsSentUpdate { get; set; }
        public DateTime? UpdateSentDate { get; set; }
    }
}