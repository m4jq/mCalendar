using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace mCalendar.Models.Repositories
{
    public class EFEventTimeDateRepository : IEventTimeDataRepository
    {
        private readonly ScheduleContext _db = new ScheduleContext();

        public IQueryable<EventTimeData> EventTimeData
        {
            get { return _db.EventTimeData; }
        }

        public EventTimeData Save(EventTimeData eventTimeData)
        {
            if (eventTimeData.Id == 0)
            {
                _db.EventTimeData.Add(eventTimeData);
            }
            else
            {
                _db.Entry(eventTimeData).State = EntityState.Modified;
            }

            _db.SaveChanges();

            return eventTimeData;
        }

        public void Delete(EventTimeData eventTimeData)
        {
            _db.EventTimeData.Remove(eventTimeData);
            _db.SaveChanges();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}