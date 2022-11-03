using System;
using System.IO;
using System.Web;

namespace GES.Common.Helpers
{
    public class Appointment
    {
        private StreamWriter _writer = null;

        public Appointment()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private string GetFormatedDate(DateTime date)
        {
            var YY = date.Year.ToString();
            var MM = string.Empty;
            var DD = string.Empty;
            if (date.Month < 10) MM = "0" + date.Month.ToString();
            else MM = date.Month.ToString();
            if (date.Day < 10) DD = "0" + date.Day.ToString();
            else DD = date.Day.ToString();
            return YY +"-"+ MM + "-" + DD;
        }

        private string GetFormattedTime(string time)
        {
            var times = time.Split(':');
            var HH = string.Empty;
            var MM = string.Empty;
            if (Convert.ToInt32(times[0]) < 10) HH = "0" + times[0];
            else HH = (Convert.ToInt32(times[0]) + 12).ToString();
            if (Convert.ToInt32(times[1]?.Substring(0,2)) < 10) MM = "0" + times[1];
            else MM = times[1];
            return HH + ":" + MM?.Substring(0,2) + ":" + "00";

        }

        public string MakeDayEvent(string subject, string location, DateTime startDate, DateTime endDate, string fileName)
        {
            var path = HttpContext.Current.Server.MapPath(@"\iCal\");
            var filePath = path + fileName + ".ics";
            _writer = new StreamWriter(filePath);
            _writer.WriteLine("BEGIN:VCALENDAR");
            _writer.WriteLine("VERSION:2.0");
            _writer.WriteLine("PRODID:-//hacksw/handcal//NONSGML v1.0//EN");
            _writer.WriteLine("BEGIN:VEVENT");


            var startDay = "VALUE=DATE:" + GetFormatedDate(startDate);
            var endDay = "VALUE=DATE:" + GetFormatedDate(endDate);

            _writer.WriteLine("DTSTART;" + startDay + "T230000Z");
            _writer.WriteLine("DTEND;" + endDay + "T233000Z");
            _writer.WriteLine("SUMMARY:" + subject);
            _writer.WriteLine("LOCATION:" + location);
            _writer.WriteLine("END:VEVENT");
            _writer.WriteLine("END:VCALENDAR");
            _writer.Close();

            return filePath;
        }

        public string MakeHourEvent(string subject, string location, DateTime date, string startTime, string endTime, string fileName)
        {
            var path = HttpContext.Current.Server.MapPath(@"\iCal\");
            var filePath = path + fileName + ".ics";
            _writer = new StreamWriter(filePath);
            _writer.WriteLine("BEGIN:VCALENDAR");
            _writer.WriteLine("VERSION:2.0");
            _writer.WriteLine("PRODID:-//hacksw/handcal//NONSGML v1.0//EN");
            _writer.WriteLine("BEGIN:VEVENT");

            var startDateTime = GetFormatedDate(date) + "T" + GetFormattedTime(startTime)+"Z";
            var endDateTime = GetFormatedDate(date) + "T" + GetFormattedTime(endTime)+"Z";

            _writer.WriteLine("DTSTART:" + startDateTime);
            _writer.WriteLine("DTEND:" + endDateTime);
            _writer.WriteLine("SUMMARY:" + subject);
            _writer.WriteLine("LOCATION:" + location);
            _writer.WriteLine("END:VEVENT");
            _writer.WriteLine("END:VCALENDAR");
            _writer.Close();

            return filePath;
        }
    }
}