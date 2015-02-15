using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mCalendar.Models
{
    public class EventTimeData
    {
        public int Id { get; set; }

        [Required]
        public int EventId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        
        public int? RepeatInterval { get; set; } //days

        public int? RepeatWeekOfMonth { get; set; }

        public int? RepeatDayOfWeek { get; set; }

        public int? RepeatMonth { get; set; }

        public int? RepeatDay { get; set; }
        
    }
}