using System.Data;
using System.Linq;

namespace mCalendar.Models.Repositories
{
    public class EFEventRepository : IEventRepository
    {
        private readonly ScheduleContext _db = new ScheduleContext();

        public IQueryable<Event> Events
        {
            get { return _db.Events; }
        }

        public Event GetById(int id)
        {
            return _db.Events.Find(id);
        }

        public Event Save(Event ev)
        {
            if (ev.Id == 0)
            {
                _db.Events.Add(ev);
            }
            else
            {
                _db.Entry(ev).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return ev;
        }

        public void Delete(Event ev)
        {
            _db.Events.Remove(ev);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}