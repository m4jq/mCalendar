using mCalendar.DomainModels.BaseClasses;
using mCalendar.DomainModels.Occourences;

namespace mCalendar.DomainModels.Converters
{
    public static class EventTimeDataDtoToOccourenceConverter
    {
        public static Occourence Convert(EventTimeDataBase eventData)
        {
            Occourence result;
            if (eventData.RepeatDay.HasValue)
            {
                result = new SerieComplexDateOccourence();
                
            }
            else if (eventData.RepeatInterval.HasValue)
            {
                result = new SerieSimpleOccourence();
            }
            else if (eventData.RepeatWeekOfMonth.HasValue || eventData.RepeatDayOfWeek.HasValue ||
                     (!eventData.RepeatDay.HasValue && eventData.RepeatMonth.HasValue))
            {
                result = new SerieComplexDayOccourence();
                
            }
            else
            {
                result = new OneTimeOccourence();
            }
            
            result.Initialize(eventData);

            return result;
        }
    }
}