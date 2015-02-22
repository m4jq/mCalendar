using System;
using System.Globalization;

namespace mCalendar.Helpers
{
    public static class DateTimeExtensions
    {
        public static int GetWeekDayNumber(this DateTime dateTime)
        {
            return (int)(dateTime.DayOfWeek + 6) % 7 + 1; //Monday = 1
        }

        public static int GetWeekNumberOfMonth(this DateTime date)
        {
            return GetWeekOfYear(date) - GetWeekOfYear(new DateTime(date.Year, date.Month, 1)) + 1;
        }

        private static int GetWeekOfYear(DateTime time) //Iso8601
        {
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek,
                DayOfWeek.Monday);
        }
    }
}