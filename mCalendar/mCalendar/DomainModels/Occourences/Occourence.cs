using System;
using mCalendar.DomainModels.BaseClasses;

namespace mCalendar.DomainModels.Occourences
{
    public abstract class Occourence 
    {
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public abstract bool IsOccouring(DateTime date);

        public virtual void Initialize(EventTimeDataBase eventData)
        {
            StartDate = eventData.StartDate;
            EndDate = eventData.EndDate;
        }

        protected bool CheckStartAndEndDate(DateTime date)
        {
            return StartDate.Date <= date.Date && (!EndDate.HasValue || EndDate.Value.Date >= date.Date);
        }
    }
}