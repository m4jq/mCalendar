using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace mCalendar.Models
{
    public class Reminder
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        [Required]
        public int MinutesBefore { get; set; }
        
        [Required]
        public ReminderType ReminderType { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        public string Content { get; set; }
    }
}