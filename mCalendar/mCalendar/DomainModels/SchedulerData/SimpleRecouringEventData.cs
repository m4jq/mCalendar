using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mCalendar.DomainModels.SchedulerData
{
    public class SimpleRecouringEventData : SchedulerEventData
    {
        public int Interval { get; set; }
        public int? RepeatCount { get; set; }
    }
}