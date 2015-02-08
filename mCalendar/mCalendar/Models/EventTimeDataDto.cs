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

        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }

        public EventTimeDataDto(){}

        public EventTimeDataDto(EventTimeData eventTimeData)
        {
            EventTimeDataId = eventTimeData.Id;
            EventId = eventTimeData.EventId;
            Key = eventTimeData.Key;
            Value = eventTimeData.Value;
        }

        public EventTimeData ToEntity()
        {
            var eventTimeData = new EventTimeData
            {
                Id = EventTimeDataId,
                EventId = EventId,
                Key = Key,
                Value = Value

            };

            return eventTimeData;
        }
    }

}