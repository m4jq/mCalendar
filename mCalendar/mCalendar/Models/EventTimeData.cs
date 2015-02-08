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

        [ForeignKey("Event")]
        public int EventId { get; set; }

        /*Possible keys:
         * start_date
         * end_date
         * timezone_id
         * repeat_interval_* --interval id, for advanced intervals
         * repeat_year_*
         * repeat_month_*
         * repeat_week_*
         * repeat_weekday_* 
         */
        [Required]
        public string Key { get; set; }
        
        [Required]
        public string Value { get; set; }
    }
}