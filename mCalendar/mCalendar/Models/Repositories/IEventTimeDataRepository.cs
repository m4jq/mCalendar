using System;
using System.Linq;

namespace mCalendar.Models.Repositories
{
    public interface IEventTimeDataRepository : IDisposable
    {
        IQueryable<EventTimeData> EventTimeData { get; }
        EventTimeData Save(EventTimeData eventTimeData);
        void Delete(EventTimeData eventTimeData);
    }
}
