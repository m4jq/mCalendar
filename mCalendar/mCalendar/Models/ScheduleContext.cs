using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace mCalendar.Models
{
    public class ScheduleContext : DbContext
    {
        public ScheduleContext()
            : base("name=DefaultConnection")
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventTimeData> EventTimeData { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}