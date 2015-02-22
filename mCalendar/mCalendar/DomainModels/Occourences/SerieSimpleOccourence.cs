using System;
using System.Diagnostics;
using mCalendar.DomainModels.BaseClasses;

namespace mCalendar.DomainModels.Occourences
{
    public class SerieSimpleOccourence : Occourence
    {
        public int Interval { get; set; }

        public override void Initialize(EventTimeDataBase eventData)
        {
            base.Initialize(eventData);

            Debug.Assert(eventData.RepeatInterval != null, "eventData.RepeatInterval != null");

            Interval = eventData.RepeatInterval.Value;
        }

        public override bool IsOccouring(DateTime date)
        {
            return CheckStartAndEndDate(date) && (((date.Date - StartDate.Date).Days)%Interval == 0);
        }
    }
}