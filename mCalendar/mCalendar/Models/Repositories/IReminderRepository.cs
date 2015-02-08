using System;
using System.Linq;

namespace mCalendar.Models.Repositories
{
    public interface IReminderRepository : IDisposable
    {
        IQueryable<Reminder> Reminders { get; }
        Reminder Save(Reminder reminder);
        void Delete(Reminder reminder);
    }
}
