using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace mCalendar.Models.Repositories
{
    public class EFReminderRepository : IReminderRepository
    {
        private readonly ScheduleContext _db = new ScheduleContext();
        
        public IQueryable<Reminder> Reminders
        {
            get { return _db.Reminders; }
        }

        public Reminder Save(Reminder reminder)
        {
            if (reminder.Id == 0)
            {
                _db.Reminders.Add(reminder);
            }
            else
            {
                _db.Entry(reminder).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return reminder;
        }

        public void Delete(Reminder reminder)
        {
            _db.Reminders.Remove(reminder);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}