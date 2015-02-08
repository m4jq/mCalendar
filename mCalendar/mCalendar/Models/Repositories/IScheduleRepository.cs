using System;
using System.Linq;

namespace mCalendar.Models.Repositories
{
    public interface IScheduleRepository : IDisposable
    {
        IQueryable<Schedule> Schedules { get; }
        Schedule GetById(int id);
        Schedule Save(Schedule schedule);
        void Delete(Schedule schedule);
        void Detach(Schedule schedule);
    }
}
