using System;

namespace mCalendar.DomainModels.BaseClasses
{
    public class EventTimeDataBase
    {
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? RepeatInterval { get; set; } //days

        public int? RepeatWeekOfMonth { get; set; }

        public int? RepeatDayOfWeek { get; set; }

        public int? RepeatMonth { get; set; }

        public int? RepeatDay { get; set; }
    }
}