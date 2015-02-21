using mCalendar.DomainModels.Reminders;
using System;

namespace mCalendar.DomainModels.ReminderData
{
    public class EventReminderData
    {
        public ReminderType Type { get; set; }
        public TimeSpan TimeBefore { get; set; }
    }
}