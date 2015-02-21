using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using mCalendar.DomainModels.Reminders;

namespace mCalendar.Models
{
    /// <summary>
    /// Data transfer object for <see cref="Reminder"/>
    /// </summary>
    public class ReminderDto
    {
        [Key]
        public int ReminderId { get; set; }

        [Required]
        public int EventId { get; set; }

        [Required]
        public TimeSpan TimeBefore { get; set; }

        [Required]
        public ReminderType ReminderType { get; set; }

        public ReminderDto(){}

        public ReminderDto(Reminder reminder)
        {
            ReminderId = reminder.Id;
            EventId = reminder.EventId;
            TimeBefore = reminder.TimeBefore;
            ReminderType = reminder.ReminderType;
        }

        public Reminder ToEntity()
        {
            var reminder = new Reminder
            {
                Id = ReminderId,
                EventId = EventId,
                TimeBefore = TimeBefore,
                ReminderType = ReminderType,
            };

            return reminder;
        }
    }
}