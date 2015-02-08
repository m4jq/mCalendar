using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mCalendar.Models
{
    /// <summary>
    /// Data transfer object for <see cref="Schedule"/>
    /// </summary>
    public class ScheduleDto
    {
        [Key]
        public int ScheduleId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual List<EventDto> Events { get; set; }

        public ScheduleDto(){}

        public ScheduleDto(Schedule schedule)
        {
            ScheduleId = schedule.Id;
            UserId = schedule.UserId;
            Title = schedule.Title;
            Events = new List<EventDto>();
            foreach (var ev in schedule.Events)
            {
                Events.Add(new EventDto(ev));
            }
        }

        public Schedule ToEntity()
        {
            var schedule = new Schedule
            {
                Id = ScheduleId,
                UserId = UserId,
                Title = Title,
                Events = new List<Event>()
            };

            foreach (var ev in Events)
            {
                schedule.Events.Add(ev.ToEntity());
            }

            return schedule;
        }
    }
}