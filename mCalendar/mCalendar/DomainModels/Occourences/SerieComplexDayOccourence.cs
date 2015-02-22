using System;
using mCalendar.DomainModels.BaseClasses;
using mCalendar.Helpers;

namespace mCalendar.DomainModels.Occourences
{
    public class SerieComplexDayOccourence : Occourence
    {
        public int? RepeatDayOfWeek { get; set; }
        public int? RepeatWeekOfMonth { get; set; }
        public int? RepeatMonth { get; set; }

        public override void Initialize(EventTimeDataBase eventData)
        {
            base.Initialize(eventData);

            RepeatDayOfWeek = eventData.RepeatDayOfWeek;
            RepeatWeekOfMonth = eventData.RepeatWeekOfMonth;
            RepeatMonth = eventData.RepeatMonth;
        }

        public override bool IsOccouring(DateTime date)
        {
            return CheckStartAndEndDate(date) && CheckRepeat(date);
        }

        private bool CheckRepeat(DateTime date)
        {
            bool repeatDayOfWeekMatches = RepeatDayOfWeek.HasValue && RepeatDayOfWeek.Value == date.GetWeekDayNumber();
            bool repeatWeekOfMonthMatches = RepeatWeekOfMonth.HasValue &&
                                            RepeatWeekOfMonth.Value == date.GetWeekNumberOfMonth();
            bool repeatMonthMathes = RepeatMonth.HasValue && RepeatMonth.Value == date.Month;

            return (repeatDayOfWeekMatches && !RepeatWeekOfMonth.HasValue && !RepeatMonth.HasValue)
                   || (repeatWeekOfMonthMatches && !RepeatDayOfWeek.HasValue && !RepeatMonth.HasValue)
                   || (repeatMonthMathes && !RepeatDayOfWeek.HasValue && !RepeatWeekOfMonth.HasValue)
                   || (repeatDayOfWeekMatches && repeatWeekOfMonthMatches && !RepeatMonth.HasValue)
                   || (!RepeatDayOfWeek.HasValue && repeatWeekOfMonthMatches && repeatMonthMathes)
                   || (repeatDayOfWeekMatches && !RepeatWeekOfMonth.HasValue && repeatMonthMathes)
                   || (repeatDayOfWeekMatches && repeatWeekOfMonthMatches && repeatMonthMathes);
        }
    }
}