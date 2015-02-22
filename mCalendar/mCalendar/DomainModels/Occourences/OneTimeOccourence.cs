using System;

namespace mCalendar.DomainModels.Occourences
{
    public class OneTimeOccourence : Occourence
    {
        public override bool IsOccouring(DateTime date)
        {
            return CheckStartAndEndDate(date);
        }
    }
}