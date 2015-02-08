using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace mCalendar.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<Event> Events { get; set; }
    }
}