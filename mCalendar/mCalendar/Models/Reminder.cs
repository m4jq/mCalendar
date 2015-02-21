using System;
using System.ComponentModel.DataAnnotations;
using mCalendar.DomainModels.Reminders;

namespace mCalendar.Models
{
    public class Reminder
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        [Required]
        public TimeSpan TimeBefore { get; set; }
        
        [Required]
        public ReminderType ReminderType { get; set; }
    }
}