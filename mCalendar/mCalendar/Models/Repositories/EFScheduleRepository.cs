using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace mCalendar.Models.Repositories
{
    public class EFScheduleRepository : IScheduleRepository
    {
        private readonly ScheduleContext _db = new ScheduleContext();

        public IQueryable<Schedule> Schedules
        {
            get { return _db.Schedules; }
        }

        public Schedule GetById(int id)
        {
            return _db.Schedules.Find(id);
        }

        public Schedule Save(Schedule schedule)
        {
            if (schedule.Id == 0)
            {
                _db.Schedules.Add(schedule);
            }
            else
            {
                _db.Entry(schedule).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return schedule;
        }

        public void Delete(Schedule schedule)
        {
            _db.Schedules.Remove(schedule);
            _db.SaveChanges();
        }

        public void Detach(Schedule schedule)
        {
            _db.Entry(schedule).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}