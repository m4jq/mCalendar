using System;
using System.Collections.Generic;
using mCalendar.DomainModels.ReminderData;

namespace mCalendar.DomainModels.SchedulerData
{
    public class SchedulerEventData //plain data from form
    {
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string EventLocation { get; set; }
        public string Description { get; set; }
        public List<EventReminderData> Reminders { get; set; }
        public EventVisibilityType VisibilityType { get; set; }
        public int ScheduleId { get; set; } 
    }
}