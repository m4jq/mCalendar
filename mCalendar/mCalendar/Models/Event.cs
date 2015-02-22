using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace mCalendar.Models
{
    public class Event
    {
        public int Id { get; set; }

        public int ScheduleId { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        public TimeSpan Duration { get; set; }

        public bool IsPrivate { get; set; }
    
        public virtual ICollection<EventTimeData> EventData { get; set; }

        public virtual ICollection<Reminder> Reminders { get; set; }
    }
}