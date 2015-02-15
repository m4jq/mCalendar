using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mCalendar.Models
{
    /// <summary>
    /// Data transfer object for <see cref="EventTimeData"/>
    /// </summary>
    public class EventTimeDataDto
    {
        [Key]
        public int EventTimeDataId { get; set; }

        [Required]   
        public int EventId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? RepeatInterval { get; set; } //days

        public int? RepeatWeekOfMonth { get; set; }

        public int? RepeatDayOfWeek { get; set; }

        public int? RepeatMonth { get; set; }

        public int? RepeatDay { get; set; }

        public EventTimeDataDto(){}

        public EventTimeDataDto(EventTimeData eventTimeData)
        {
            EventTimeDataId = eventTimeData.Id;
            EventId = eventTimeData.EventId;
            StartDate = eventTimeData.StartDate;
            EndDate = eventTimeData.EndDate;
            RepeatInterval = eventTimeData.RepeatInterval;
            RepeatWeekOfMonth = eventTimeData.RepeatWeekOfMonth;
            RepeatDayOfWeek = eventTimeData.RepeatDayOfWeek;
            RepeatMonth = eventTimeData.RepeatMonth;
            RepeatDay = eventTimeData.RepeatDay;
        }

        public EventTimeData ToEntity()
        {
            var eventTimeData = new EventTimeData
            {
                Id = EventTimeDataId,
                EventId = EventId,
                StartDate = StartDate,
                EndDate = EndDate,
                RepeatInterval = RepeatInterval,
                RepeatWeekOfMonth = RepeatWeekOfMonth,
                RepeatDayOfWeek = RepeatDayOfWeek,
                RepeatMonth = RepeatMonth,
                RepeatDay = RepeatDay
            };

            return eventTimeData;
        }
    }

}