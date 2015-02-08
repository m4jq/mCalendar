using System;
using System.Linq;

namespace mCalendar.Models.Repositories
{
    public interface IEventRepository : IDisposable
    {
        IQueryable<Event> Events { get; }
        Event GetById(int id);
        Event Save(Event ev);
        void Delete(Event ev);
    }
}
