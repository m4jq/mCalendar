using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mCalendar.Models;

namespace mCalendar.DomainModels.Interfaces
{
    interface IEventViewer
    {
        Dictionary<DateTime, List<EventDto>> GetEvents(DateTime startDate, DateTime endDate);
    }
}
