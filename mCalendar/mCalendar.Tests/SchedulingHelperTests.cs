using System;
using System.Collections.Generic;
using System.Linq;
using mCalendar.Helpers;
using mCalendar.Models;
using NUnit.Framework;

namespace mCalendar.Tests
{
    [TestFixture]
    public class SchedulingHelperTests
    {
        [Test]
        public void CreateWeeklyEventDto_OneWeekInterval_Monday_NoEndDate_Success()
        {
            //Arrange
            var numberOfWeeks = 1;
            var weekDays = WeekDaysFlags.Monday;
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = null;
            int? repeatCount = null;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.EventData);
            Assert.AreEqual(1, result.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(result.EventData);
            Assert.AreEqual(new DateTime(2015, 2, 16, 20, 0, 0).ToUniversalTime(), result.EventData.First().StartDate);
            Assert.False(result.EventData.First().EndDate.HasValue);
            Assert.AreEqual(7, result.EventData.First().RepeatInterval);
            Assert.False(result.EventData.First().RepeatWeekOfMonth.HasValue);
            Assert.False(result.EventData.First().RepeatMonth.HasValue);
            Assert.False(result.EventData.First().RepeatDay.HasValue);
            Assert.False(result.EventData.First().RepeatDayOfWeek.HasValue);
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedEndDate_Success()
        {
            //Arrange
            var numberOfWeeks = 3;
            var weekDays = WeekDaysFlags.Monday|WeekDaysFlags.Friday;
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = startDate.AddMonths(3);
            int? repeatCount = null;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.EventData);
            Assert.AreEqual(2, result.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(result.EventData);

            var expectedCollection = new List<EventTimeDataDto>
            {
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,3,2,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,5,15,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,3,6,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,5,15,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(result.EventData, Has.All.Matches<EventTimeDataDto>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_NoDayOfWeek_FixedEndDate_Success()
        {
            //Arrange
            var numberOfWeeks = 3;
            var weekDays = WeekDaysFlags.None;
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = startDate.AddMonths(3);
            int? repeatCount = null;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.EventData);
            Assert.AreEqual(1, result.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(result.EventData);
            Assert.AreEqual(startDate.ToUniversalTime(), result.EventData.First().StartDate);
            Assert.IsTrue(result.EventData.First().EndDate.HasValue);
            Assert.AreEqual(endDate.Value.ToUniversalTime(), result.EventData.First().EndDate.Value);
            Assert.AreEqual(21, result.EventData.First().RepeatInterval);
            Assert.False(result.EventData.First().RepeatWeekOfMonth.HasValue);
            Assert.False(result.EventData.First().RepeatMonth.HasValue);
            Assert.False(result.EventData.First().RepeatDay.HasValue);
            Assert.False(result.EventData.First().RepeatDayOfWeek.HasValue);
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_Monday_FixedRepeatCount_Success()
        {
            //Arrange
            var numberOfWeeks = 3;
            var weekDays = WeekDaysFlags.Monday;
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = null;
            int? repeatCount = 3;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.EventData);
            Assert.AreEqual(1, result.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(result.EventData);
            Assert.AreEqual(new DateTime(2015, 3, 2, 20, 0, 0).ToUniversalTime(), result.EventData.First().StartDate);
            Assert.IsTrue(result.EventData.First().EndDate.HasValue);
            Assert.AreEqual(new DateTime(2015, 4, 13, 20, 0, 0).ToUniversalTime(), result.EventData.First().EndDate.Value);
            Assert.AreEqual(21, result.EventData.First().RepeatInterval);
            Assert.False(result.EventData.First().RepeatWeekOfMonth.HasValue);
            Assert.False(result.EventData.First().RepeatMonth.HasValue);
            Assert.False(result.EventData.First().RepeatDay.HasValue);
            Assert.False(result.EventData.First().RepeatDayOfWeek.HasValue);
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedRepeatCount_StartDateBetween_Success()
        {
            //Arrange
            var numberOfWeeks = 3;
            var weekDays = WeekDaysFlags.Monday | WeekDaysFlags.Friday;
            var startDate = new DateTime(2015, 2, 18, 20, 0, 0);
            DateTime? endDate = null;
            int? repeatCount = 3;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.EventData);
            Assert.AreEqual(2, result.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(result.EventData);

            var expectedCollection = new List<EventTimeDataDto>
            {
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,3,9,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,9,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,2,20,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,13,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(result.EventData, Has.All.Matches<EventTimeDataDto>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedRepeatCount_StartDateMonday_Success()
        {
            //Arrange
            var numberOfWeeks = 3;
            var weekDays = WeekDaysFlags.Monday | WeekDaysFlags.Friday;
            var startDate = new DateTime(2015, 2, 16, 20, 0, 0);
            DateTime? endDate = null;
            int? repeatCount = 3;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.EventData);
            Assert.AreEqual(2, result.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(result.EventData);

            var expectedCollection = new List<EventTimeDataDto>
            {
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,2,16,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,9,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,2,20,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,2,20,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(result.EventData, Has.All.Matches<EventTimeDataDto>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedRepeatCount_StartDateSunday_Success()
        {
            //Arrange
            var numberOfWeeks = 3;
            var weekDays = WeekDaysFlags.Monday | WeekDaysFlags.Friday;
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = null;
            int? repeatCount = 3;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result.EventData);
            Assert.AreEqual(2, result.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(result.EventData);

            var expectedCollection = new List<EventTimeDataDto>
            {
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,3,2,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,23,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeDataDto
                {
                    StartDate = new DateTime(2015,3,6,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,6,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(result.EventData, Has.All.Matches<EventTimeDataDto>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid input. EndDate and repeatCount have value.")]
        public void CreateWeeklyEventDto_FixedRepeatCount_And_FixedEndDate_Fails()
        {
            //Arrange
            var numberOfWeeks = 3;
            var weekDays = WeekDaysFlags.Monday;
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = startDate.AddMonths(3);
            int? repeatCount = 3;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert - attribute
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid input. Number of weeks have to be greater or equal 1.")]
        public void CreateWeeklyEventDto_ZeroAsNumberOfWeeks_Fails()
        {
            //Arrange
            var numberOfWeeks = 0;
            var weekDays = WeekDaysFlags.Monday;
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = null;
            int? repeatCount = 3;

            //Act
            EventDto result = SchedulingHelper.CreateWeeklyEventDto(numberOfWeeks, weekDays, startDate, endDate,
                repeatCount);

            //Assert - attribute
        }



        private static bool IsInExpected(EventTimeDataDto item, IEnumerable<EventTimeDataDto> expected)
        {
            var matchedItem = expected.FirstOrDefault(f =>
                f.EventTimeDataId == item.EventTimeDataId &&
                f.EventId == item.EventId &&
                f.StartDate == item.StartDate &&
                f.EndDate == item.EndDate &&
                f.RepeatInterval == item.RepeatInterval &&
                f.RepeatWeekOfMonth == item.RepeatWeekOfMonth &&
                f.RepeatDayOfWeek == item.RepeatDayOfWeek &&
                f.RepeatMonth == item.RepeatMonth &&
                f.RepeatDay == item.RepeatDay
            );

            return matchedItem != null;
        }

    }
}
