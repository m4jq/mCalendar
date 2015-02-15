using System;
using System.Collections.Generic;
using System.Linq;
using mCalendar.Models;

namespace mCalendar.Helpers
{
    public class SchedulingHelper
    {
        public static EventDto CreateWeeklyEventDto(int numberOfWeeks, WeekDaysFlags weekDays, DateTime startDate, DateTime? endDate, int? repeatCount)
        {
            if(endDate.HasValue && repeatCount.HasValue)
                throw new InvalidOperationException("Invalid input. EndDate and repeatCount have value.");

            var interval = numberOfWeeks*7;

            var ev = new Event {EventData = new List<EventTimeData>()}; //fill data from event form - external

            var daysOfWeek = new List<int>();
            if (weekDays != 0)
            {
                daysOfWeek = ConvertToDaysOfWeekNumbers(Enum.GetValues(weekDays.GetType()).Cast<Enum>().Where(weekDays.HasFlag));
            }
            else
            {
                daysOfWeek.Add(GetWeekDayNumber(startDate));
            }

            if (repeatCount.HasValue)
            {
                var eventDays = GetRepetitions(daysOfWeek, repeatCount.Value, GetWeekDayNumber(startDate));
                foreach (var eventDayOfWeek in eventDays.Where(e => e.Value > 0))
                {
                    DateTime startDateTime = GetStartDateTime(startDate, eventDayOfWeek.Key);
                    DateTime endDateTime = GetEndDateTime(startDateTime, eventDayOfWeek.Value, interval);
                    
                    var eventTimeData = new EventTimeData
                    {
                        StartDate = startDateTime.ToUniversalTime(),
                        EndDate = endDateTime.ToUniversalTime(),
                        RepeatInterval = interval,
                    };

                    ev.EventData.Add(eventTimeData);
                }
            }
            else
            {
                foreach (var dayOfWeek in daysOfWeek)
                {
                    DateTime startDateTime = GetStartDateTime(startDate, dayOfWeek);
                    DateTime? endDateTime = null;

                    if (endDate.HasValue)
                    {
                        endDateTime = endDate.Value.ToUniversalTime();
                    }

                    var eventTimeData = new EventTimeData
                    {
                        StartDate = startDateTime.ToUniversalTime(),
                        EndDate = endDateTime,

                        RepeatInterval = interval,

                    };

                    ev.EventData.Add(eventTimeData);
                }
            }
         
            return new EventDto(ev);
        }

        private static int GetWeekDayNumber(DateTime dateTime)
        {
            return (int)(dateTime.DayOfWeek + 6) % 7 + 1; //Monday = 1
        }

        private static List<int> ConvertToDaysOfWeekNumbers(IEnumerable<Enum> enumDaysOfWeek)
        {
            var daysOfWeek = new List<int>();
            var daysOfWeekDic = new Dictionary<WeekDaysFlags, int>
            {
                {WeekDaysFlags.Monday, 1},
                {WeekDaysFlags.Tuesday, 2},
                {WeekDaysFlags.Wednesday, 3},
                {WeekDaysFlags.Thursday, 4},
                {WeekDaysFlags.Friday, 5},
                {WeekDaysFlags.Saturday, 6},
                {WeekDaysFlags.Sunday, 7}
            };

            foreach (var enumDayOfWeek in enumDaysOfWeek)
            {
                int weekDayNumber;
                WeekDaysFlags dayFlag;
                Enum.TryParse(enumDayOfWeek.ToString(), out dayFlag);
                if(dayFlag == 0) //if None found
                    continue;

                daysOfWeekDic.TryGetValue(dayFlag, out weekDayNumber);
                if(weekDayNumber == 0)
                    throw new Exception("Invalid day of week."); //todo: custom exception

                daysOfWeek.Add(weekDayNumber);
            }

            return daysOfWeek;
        }

        private static DateTime GetStartDateTime(DateTime actualDate, int dayOfWeek)
        {
            DateTime startDateTime;
            int actualDayOfWeek = GetWeekDayNumber(actualDate);

            if (actualDayOfWeek == dayOfWeek)
            {
                startDateTime = actualDate;
            }
            else if (actualDayOfWeek > dayOfWeek)
            {
                startDateTime = actualDate.AddDays(7 - Math.Abs(actualDayOfWeek - dayOfWeek));
            }
            else
            {
                startDateTime = actualDate.AddDays(dayOfWeek - actualDayOfWeek);
            }

            return startDateTime;
        }

        private static DateTime GetEndDateTime(DateTime startDate, int repeatCount, int interval)
        {
            return startDate.AddDays((repeatCount - 1)*interval);
        }

        private static Dictionary<int, int> GetRepetitions(IEnumerable<int> chosenDaysNumbers, 
            int repeatCount, int startDayNumber)
        {
            var daysNumbers = chosenDaysNumbers as List<int> ?? chosenDaysNumbers.ToList();
            var result = daysNumbers.ToDictionary(chosenDayNumber => chosenDayNumber, chosenDayNumber => 0);

            if (result.Count == 1)
            {
                result[result.First().Key] = repeatCount;
                return result;
            }

            daysNumbers.Sort();

            var dayFound = daysNumbers.Any(d => d >= startDayNumber)
                ? daysNumbers.First(d => d >= startDayNumber)
                : daysNumbers.Min();

            var tmp = repeatCount;
            while (tmp > 0)
            {
                foreach (var dayNumber in daysNumbers)
                {
                    if (tmp == repeatCount) //first loop
                    {
                        if (dayNumber >= dayFound)
                        {
                            result[dayNumber] += 1;
                            tmp--;
                            if(tmp == 0)
                                break;
                        }

                        continue;
                    }

                    result[dayNumber] += 1;
                    tmp--;
                    if (tmp == 0)
                        break;
                }
                
            }

            return result;
        }
    }
}