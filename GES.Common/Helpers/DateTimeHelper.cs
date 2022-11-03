using System;

namespace GES.Common.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime ConvertCetOrCestDateTimeToUtc(DateTime? dateTime, TimeSpan? time)
        {
            DateTime dt;
            
            var timeValue = time.ToString();
            
            var dateTimeValue = Convert.ToDateTime(dateTime?.Year + "-" + dateTime?.Month + "-" +
                                               dateTime?.Day + " " + timeValue);
            
            var marOfYear = Convert.ToDateTime(dateTime?.Year + "-3" + "-1");
            marOfYear = Convert.ToDateTime(dateTime?.Year + "-3-" + GetLastSundayOfTheMonth(marOfYear).Day +
                                           " " + "2:00:00");
            
            var octOfYear = Convert.ToDateTime(dateTime?.Year + "-10" + "-1");
            octOfYear = Convert.ToDateTime(dateTime?.Year + "-10-" + GetLastSundayOfTheMonth(octOfYear).Day +
                                           " " + "10:00:00");
            
            if (dateTimeValue >= marOfYear && dateTimeValue <= octOfYear)
            {
                var cetStartDate = dateTime?.Year + "-" + dateTime?.Month + "-" +
                                   dateTime?.Day + " " + timeValue + " +02:00";
            
                dt = Convert.ToDateTime(cetStartDate);
            }
            else
            {
                dt = Convert.ToDateTime(dateTime?.Year + "-" + dateTime?.Month + "-" +
                                        dateTime?.Day + " " + timeValue + " +01:00");
            }

            return dt.ToUniversalTime();
        }
        
        private static DateTime GetLastSundayOfTheMonth(DateTime date)
        {
            var lastDayOfMonth = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));

            while (lastDayOfMonth.DayOfWeek != DayOfWeek.Sunday)
                lastDayOfMonth = lastDayOfMonth.AddDays(-1);

            return lastDayOfMonth;
        }
    }
}