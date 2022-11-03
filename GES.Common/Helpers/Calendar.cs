using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Attachment = Ical.Net.DataTypes.Attachment;

namespace GES.Common.Helpers
{
    public class GesCalendar
    {
        private Calendar GennerateCalendar(GesCalendarEvent calendarEvent)
        {
            var startUtc = new DateTime(calendarEvent.Start.Year, calendarEvent.Start.Month,
                calendarEvent.Start.Day, calendarEvent.Start.Hour, calendarEvent.Start.Minute, 0,
                DateTimeKind.Utc);

            var endUtc = new DateTime(calendarEvent.End.Year, calendarEvent.End.Month,
                calendarEvent.End.Day, calendarEvent.End.Hour, calendarEvent.End.Minute, 0,
                DateTimeKind.Utc);
            
            var e = new CalendarEvent
            {
                Start = new CalDateTime(startUtc),
                End = new CalDateTime(endUtc),
                Description = calendarEvent.Description,
                Summary = calendarEvent.Summary,
                Location = calendarEvent.Location,
                Uid = calendarEvent.Uid,
                IsAllDay = calendarEvent.IsAllDay,
                Sequence = calendarEvent.Sequence,
                Priority = calendarEvent.Priority,
                Status = calendarEvent.Status,
                Transparency = calendarEvent.Transparency,
                Class = calendarEvent.Class,
                Organizer = calendarEvent.Organizer,
                Attendees = calendarEvent?.Attendees,
               // Attachments = calendarEvent.Attachments,
               // RecurrenceRules = calendarEvent.RecurrencePatterns
            };

            var alarms = calendarEvent?.Alarms;
            foreach (var alarm in alarms)
            {
                e.Alarms.Add(new Alarm()
                {
                    Action = AlarmAction.Display,
                    Trigger = new Trigger(TimeSpan.FromDays(-1)),
                    Summary = calendarEvent.Summary + "due in " + (DateTime.Now - calendarEvent.Start)
                });
            }
            
            var calendar = new Calendar {Method = calendarEvent.Method};
            calendar.Events.Add(e);

            return calendar;
        }
        
        public string GesSendEmailCalendar(GesEmailSetting emailSetting, GesSmtpSetting gesSmtpSetting, GesCalendarEvent eventCalendar)
        {
            var serializer = new CalendarSerializer();
            var calendarContent = serializer.SerializeToString(GennerateCalendar(eventCalendar));
                
            var credential = new System.Net.NetworkCredential(gesSmtpSetting.SmtpUserName, gesSmtpSetting.SmtpUserPassword);

            var smtpclient = SendMailSmtp.GetSmtpClient(gesSmtpSetting.SmtpHost, gesSmtpSetting.SmtpPort, credential,
                gesSmtpSetting.SmtpEnableSsl);
            
            
            var emailContent = new MailMessage
            {
                From = new MailAddress(emailSetting.SenderEmailAddress, emailSetting.SenderName),
                Subject = emailSetting.Subject,
                Body = emailSetting.EmailBody
            };

           
            foreach (var attendee in emailSetting.Recipients)
            {
                var email = new MailAddress(attendee.Address, attendee.DisplayName);
                emailContent.To.Add(email);
                
            }
  
            var contype = new System.Net.Mime.ContentType("text/calendar");
            if (contype.Parameters != null)
            {
                contype.Parameters.Add("method", emailSetting.Method);
                contype.Parameters.Add("component", "VEVENT");
                contype.Parameters.Add("name", "Meeting.ics");
            }

            try
            {
                SendMailSmtp.SendEmail(emailContent, smtpclient, calendarContent, contype);
                return "Sent success";
            }
            catch (Exception e)
            {
                return "Send email failled, cause " + e.Message;
            }
        }
    }
    
    public class GesSmtpSetting
    {
        public string SmtpUserName { get; set; }
        public string SmtpUserPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public bool SmtpEnableSsl { get; set; }
    }

    public class GesEmailSetting
    {
        public string SenderEmailAddress { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public string EmailBody { get; set; }
        public string Method { get; set; }
        public IEnumerable<MailAddress> Recipients { get; set; }
    }

    public class GesCalendarEvent
    {
        public GesCalendarEvent(GesCalendarEventOrganizer calendarEventOrganizer, IEnumerable<GesCalendarEventAttendee> attendees,
            IEnumerable<GesCalendarEventAlarm> alarms, IEnumerable<GesCalendarEventAttachment> attachments,
            IEnumerable<GesCalendarEventRecurrencePattern> recurrencePatterns)
        {

            var  localAttendees = attendees.Select(attendee => new Attendee()
                {
                    CommonName = attendee.CommonName,
                    ParticipationStatus = attendee.ParticipationStatus,
                    Rsvp = true,
                    Value = attendee.Value
                })
                .ToList();

            this.Attendees = localAttendees;

            var localAlarms = alarms.Select(alarm => new Alarm()
                {
                    Action = alarm.Action,
                    Trigger = new Trigger(TimeSpan.FromDays(-1)),
                    Summary = this.Summary + "due in "
                })
                .ToList();

            Alarms = localAlarms;
            
            var localAttachments = attachments.Select(attachment => new Attachment()
                {
                    Uri = attachment.Uri,
                    ValueEncoding = attachment.ValueEncoding,
                    FormatType = attachment.FormatType
                })
                .ToList();
            Attachments = localAttachments;

            var localRecurrencePatterns = recurrencePatterns.Select(recurrencePattern => new RecurrencePattern(FrequencyType.Daily, 1) {Count = recurrencePattern.Count,}).ToList();
            RecurrencePatterns = localRecurrencePatterns;

            var localOrganizer = new Organizer
            {
                CommonName = calendarEventOrganizer.CommonName,
                Value = calendarEventOrganizer.Value
            };

            this.Organizer = localOrganizer;
        }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Location { get; set; }
        public string Uid { get; set; }
        public bool IsAllDay { get; set; }
        public int Sequence { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }

        public string Transparency => TransparencyType.Transparent;
        public string Class { get; set; }
        public string Method { get; set; }
        public Organizer Organizer { get; set; }
        public List<Attendee> Attendees { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Alarm> Alarms { get; set; }
        public List<RecurrencePattern> RecurrencePatterns { get; set; }
    }

    public class GesCalendarEventOrganizer
    {
        public string CommonName { get; set; }
        public Uri Value { get; set; }
        
    }

    public class GesCalendarEventAttendee
    {
        public string CommonName { get; set;}
        public string ParticipationStatus => "REQ-PARTICIPANT";

        public bool Rsvp => true;

        public Uri Value { get; set; }
        public string Email { get; set; }
    }

    public class GesCalendarEventAttachment
    {
        private Encoding _valueEncoding = Encoding.UTF8;

        public Uri Uri { get; set; }

        public Encoding ValueEncoding
        {
            get
            {
                return this._valueEncoding;
            }
            set
            {
                if (value == null)
                    return;
                this._valueEncoding = value;
            }
        }

        public string FormatType{get;set; }
    }

    public class GesCalendarEventAlarm
    {
        public string Action => AlarmAction.Display;

        public virtual Trigger Trigger => new Trigger(TimeSpan.FromDays(-1));

        public string Summary { get; set; }
    }

    public class GesCalendarEventRecurrencePattern
    {
        public string FrequencyType { get; set; }
        public int Count { get; set; }
    }

}