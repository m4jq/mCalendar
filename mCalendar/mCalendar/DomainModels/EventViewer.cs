using System;
using System.Collections.Generic;
using mCalendar.DomainModels.Interfaces;
using mCalendar.Models;

namespace mCalendar.DomainModels
{
    public class EventViewer : IEventViewer
    {
        public List<EventDto> GetEvents(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }
    }
}