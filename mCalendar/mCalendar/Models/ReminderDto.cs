using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
        public int MinutesBefore { get; set; }

        [Required]
        public ReminderType ReminderType { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public ReminderDto(){}

        public ReminderDto(Reminder reminder)
        {
            ReminderId = reminder.Id;
            EventId = reminder.EventId;
            MinutesBefore = reminder.MinutesBefore;
            ReminderType = reminder.ReminderType;
            Title = reminder.Title;
            Content = reminder.Content;
        }

        public Reminder ToEntity()
        {
            var reminder = new Reminder
            {
                Id = ReminderId,
                EventId = EventId,
                MinutesBefore = MinutesBefore,
                ReminderType = ReminderType,
                Title = Title,
                Content = Content
            };

            return reminder;
        }
    }
}