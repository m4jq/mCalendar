using System;
using System.Collections.Generic;
using System.Linq;
using mCalendar.DomainModels.Interfaces;
using mCalendar.DomainModels.ReminderData;
using mCalendar.DomainModels.SchedulerData;
using mCalendar.Exceptions;
using mCalendar.Helpers;
using mCalendar.Models;
using mCalendar.Models.Repositories;

namespace mCalendar.DomainModels.Schedulers
{
    public class WeeklyEventsScheduler : IScheduler
    {
        public void ScheduleEvent(SchedulerEventData eventData, IEventRepository repository)
        {
            var weeklyEventData = eventData as WeeklySchedulerEventData;

            if(weeklyEventData == null)
                throw new InvalidSchedulerEventDataException();

            var weeklyEventTimeData = CreateWeeklyEventTimeData(weeklyEventData.Interval, weeklyEventData.WeekDays,
                weeklyEventData.StartDate, weeklyEventData.EndDate, weeklyEventData.RepeatCount);

            var reminders = CreateReminders(weeklyEventData.Reminders);

            var weeklyEventDto = new EventDto
            {
                Title = weeklyEventData.Title,
                Description = weeklyEventData.Description,
                EventData = weeklyEventTimeData,
                IsPrivate = weeklyEventData.VisibilityType == EventVisibilityType.Private, //TODO: inherit visibility time from schedule
                Reminders = reminders,
                ScheduleId = weeklyEventData.ScheduleId
            };

            repository.Save(weeklyEventDto.ToEntity());
        }

        public static List<EventTimeDataDto> CreateWeeklyEventTimeData(int numberOfWeeks, WeekDaysFlags weekDays, 
            DateTime startDate, DateTime? endDate, int? repeatCount)
        {
            const int numberOfWeekDays = 7;

            if (numberOfWeeks < 1)
                throw new InvalidOperationException("Invalid input. Number of weeks have to be greater or equal 1.");
            if (endDate.HasValue && repeatCount.HasValue)
                throw new InvalidOperationException("Invalid input. EndDate and repeatCount have value.");

            var interval = numberOfWeeks * numberOfWeekDays;

            var result = new List<EventTimeDataDto>();

            var daysOfWeek = new List<int>();
            if (weekDays != 0)
            {
                daysOfWeek =
                    ConvertToDaysOfWeekNumbers(Enum.GetValues(weekDays.GetType()).Cast<Enum>().Where(weekDays.HasFlag));
            }
            else
            {
                daysOfWeek.Add(startDate.GetWeekDayNumber());
            }

            if (repeatCount.HasValue)
            {
                var eventDays = GetRepetitions(daysOfWeek, repeatCount.Value, startDate.GetWeekDayNumber());
                foreach (var eventDayOfWeek in eventDays.Where(e => e.Value > 0))
                {
                    DateTime startDateTime = GetStartDateTime(startDate, eventDayOfWeek.Key, interval);
                    DateTime endDateTime = GetEndDateTime(startDateTime, eventDayOfWeek.Value, interval);

                    var eventTimeData = new EventTimeDataDto
                    {
                        StartDate = startDateTime.ToUniversalTime(),
                        EndDate = endDateTime.ToUniversalTime(),
                        RepeatInterval = interval,
                    };

                    result.Add(eventTimeData);
                }
            }
            else
            {
                foreach (var dayOfWeek in daysOfWeek)
                {
                    DateTime startDateTime = GetStartDateTime(startDate, dayOfWeek, interval);
                    DateTime? endDateTime = null;

                    if (endDate.HasValue)
                    {
                        endDateTime = endDate.Value.ToUniversalTime();
                    }

                    var eventTimeData = new EventTimeDataDto
                    {
                        StartDate = startDateTime.ToUniversalTime(),
                        EndDate = endDateTime,

                        RepeatInterval = interval,

                    };

                    result.Add(eventTimeData);
                }
            }

            return result;
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
                if (dayFlag == 0) //if None found
                    continue;

                daysOfWeekDic.TryGetValue(dayFlag, out weekDayNumber);
                if (weekDayNumber == 0)
                    throw new Exception("Invalid day of week."); //todo: custom exception

                daysOfWeek.Add(weekDayNumber);
            }

            return daysOfWeek;
        }

        private static DateTime GetStartDateTime(DateTime actualDate, int dayOfWeek, int interval)
        {
            DateTime startDateTime;
            int actualDayOfWeek = actualDate.GetWeekDayNumber();

            if (actualDayOfWeek == dayOfWeek)
            {
                startDateTime = actualDate;
            }
            else if (actualDayOfWeek > dayOfWeek)
            {
                startDateTime = actualDate.AddDays(dayOfWeek - actualDayOfWeek + interval);
            }
            else
            {
                startDateTime = actualDate.AddDays(dayOfWeek - actualDayOfWeek);
            }

            return startDateTime;
        }

        private static DateTime GetEndDateTime(DateTime startDate, int repeatCount, int interval)
        {
            return startDate.AddDays((repeatCount - 1) * interval);
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
                            if (tmp == 0)
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

        private List<ReminderDto> CreateReminders(List<EventReminderData> remindersData)
        {
            var result = new List<ReminderDto>();

            if (remindersData != null && remindersData.Count > 0)
            {
                result.AddRange(remindersData.Select(
                    reminderData =>
                        new ReminderDto {ReminderType = reminderData.Type, TimeBefore = reminderData.TimeBefore})
                    .ToList());
            }

            return result;
        }
    }
}