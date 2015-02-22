namespace mCalendar.DomainModels.SchedulerData
{
    public class WeeklySchedulerEventData : SimpleRecouringEventData
    {
        public WeekDaysFlags WeekDays { get; set; }
    }
}