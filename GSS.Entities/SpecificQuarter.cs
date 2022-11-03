using System;

namespace Sustainalytics.GSS.Entities
{
    public class SpecificQuarter
    {
        public Quarter Numerator { get; set; }

        public int Year { get; set; }

        public SpecificQuarter GetNextQuarter() => new SpecificQuarter { Numerator = (Quarter)((int)Numerator % 4 + 1), Year = Year + (int)Numerator / 4 };
    }

    public static class GssQuarterExtensions
    {
        const int CalendarToGssQuarterMonthsOffset = -1;

        public static DateTime GetGssQuarterStartDate(this SpecificQuarter quarter)
        {
            var firstCalendarMonthInQuarter = 3 * (int)quarter.Numerator - 2;

            var calendarQuarterStartDate = new DateTime(quarter.Year, firstCalendarMonthInQuarter, 1);

            return GetLastFridayOfPreviousMonth(calendarQuarterStartDate.AddMonths(CalendarToGssQuarterMonthsOffset));// apply GSS quarter specific corrections
        }

        private static DateTime GetLastFridayOfPreviousMonth(DateTime date)
        {
            return date.AddDays(date.DayOfWeek == DayOfWeek.Saturday ? -1.0 : -2.0 - (double)date.DayOfWeek);
        }

        public static SpecificQuarter GetGssQuarter(this DateTime value)
        {
            return GetGssQuarter(value, new SpecificQuarter { Numerator = Quarter.One, Year = value.Year });
        }

        private static SpecificQuarter GetGssQuarter(DateTime date, SpecificQuarter currentQuarter)
        {
            if (date < currentQuarter.GetGssQuarterStartDate())
            {
                currentQuarter.Year--;

                return  GetGssQuarter(date, currentQuarter);
            }

            var nextQuarter = currentQuarter.GetNextQuarter();

            if (date < nextQuarter.GetGssQuarterStartDate())
            {
                return currentQuarter;
            }

            return GetGssQuarter(date, nextQuarter);
        }
    }
}