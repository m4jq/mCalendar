using System;
using mCalendar.DomainModels.BaseClasses;

namespace mCalendar.DomainModels.Occourences
{
    public class SerieComplexDateOccourence : Occourence
    {
        public int? RepeatDay { get; set; }
        public int? RepeatMonth { get; set; }

        public override void Initialize(EventTimeDataBase eventData)
        {
            base.Initialize(eventData);

            RepeatDay = eventData.RepeatDay;
            RepeatMonth = eventData.RepeatMonth;
        }

        public override bool IsOccouring(DateTime date)
        {
            return CheckStartAndEndDate(date) && CheckRepeat(date);
        }

        private bool CheckRepeat(DateTime date)
        {
            bool repeatDayMatches = RepeatDay.HasValue && RepeatDay.Value == date.Day;
            bool repeatMonthMatches = RepeatMonth.HasValue && RepeatMonth.Value == date.Month;

            return (repeatDayMatches && repeatMonthMatches)
                   || (repeatDayMatches && !RepeatMonth.HasValue)
                   || (!RepeatDay.HasValue && repeatMonthMatches);
        }
    }
}