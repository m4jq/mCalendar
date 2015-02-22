using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mCalendar.Models
{
    /// <summary>
    /// Data transfer object for <see cref="Event"/>
    /// </summary>
    public class EventDto
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public int ScheduleId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsPrivate { get; set; }

        public List<EventTimeDataDto> EventData { get; set; }

        public List<ReminderDto> Reminders { get; set; }

        public EventDto() { }

        public EventDto(Event ev)
        {
            EventId = ev.Id;
            ScheduleId = ev.ScheduleId;
            Title = ev.Title;
            Description = ev.Description;
            Duration = ev.Duration;
            IsPrivate = ev.IsPrivate;

            EventData = new List<EventTimeDataDto>();

            foreach (var eventData in ev.EventData)
            {
                EventData.Add(new EventTimeDataDto(eventData));
            }

            Reminders = new List<ReminderDto>();

            foreach (var reminder in ev.Reminders)
            {
                Reminders.Add(new ReminderDto(reminder));
            }
        }

        public Event ToEntity()
        {
            var ev = new Event
            {
                Id = EventId,
                ScheduleId = ScheduleId,
                Title = Title,
                Description = Description,
                Duration = Duration,
                IsPrivate = IsPrivate,
                EventData = new List<EventTimeData>(),
                Reminders = new List<Reminder>()
            };

            foreach (var eventData in EventData)
            {
                ev.EventData.Add(eventData.ToEntity());
            }

            foreach (var reminder in Reminders)
            {
                ev.Reminders.Add(reminder.ToEntity());
            }

            return ev;

        }
    }

}