using mCalendar.DomainModels.SchedulerData;
using mCalendar.Models.Repositories;

namespace mCalendar.DomainModels.Interfaces
{
    public interface IScheduler
    {
        void ScheduleEvent(SchedulerEventData eventData, IEventRepository repository);
    }
}
