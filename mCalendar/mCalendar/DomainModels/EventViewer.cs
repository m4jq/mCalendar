using System;
using System.Collections.Generic;
using System.Linq;
using mCalendar.DomainModels.Converters;
using mCalendar.DomainModels.Interfaces;
using mCalendar.Models;
using mCalendar.Models.Repositories;

namespace mCalendar.DomainModels
{
    public class EventViewer : IEventViewer
    {
        private readonly IEventRepository _eventRepository;

        public EventViewer(IEventRepository repo)
        {
            _eventRepository = repo;
        }

        private List<EventDto> GetEvents(DateTime date)
        {
            return _eventRepository.Events.Where(
                    e =>
                        e.EventData.Any(
                            ed =>
                                EventTimeDataDtoToOccourenceConverter.Convert(new EventTimeDataDto(ed))
                                    .IsOccouring(date))).Select(e => new EventDto(e)).ToList();
        }

        public Dictionary<DateTime, List<EventDto>> GetEvents(DateTime startDate, DateTime endDate)
        {
            var result = new Dictionary<DateTime, List<EventDto>>();
            var actualDate = startDate;
            var daysBetween = (endDate - startDate).Days;

            for (int i = 0; i < daysBetween + 1; i++)
            {
                result.Add(actualDate, GetEvents(actualDate));
                actualDate = actualDate.AddDays(1);
            }

            return result;
        }
    }
}