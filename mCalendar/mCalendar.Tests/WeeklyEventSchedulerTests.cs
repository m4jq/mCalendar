using System;
using System.Collections.Generic;
using System.Linq;
using mCalendar.DomainModels.Factories;
using mCalendar.DomainModels.Interfaces;
using mCalendar.DomainModels.SchedulerData;
using mCalendar.DomainModels.Schedulers;
using mCalendar.Helpers;
using mCalendar.Models;
using mCalendar.Models.Repositories;
using Moq;
using NUnit.Framework;

namespace mCalendar.Tests
{
    [TestFixture]
    public class WeeklyEventSchedulerTests
    {
        private Mock<IEventRepository> _eventsRepositoryMock;
        private const string WeeklySchedulerTypeName = "WeeklyEventsScheduler";
        private Event _event;
        private IScheduler _scheduler;

        [SetUp]
        public void SetUp()
        {
            _event = null;
            _eventsRepositoryMock = new Mock<IEventRepository>();
            _eventsRepositoryMock.Setup(h => h.Save(It.IsAny<Event>()))
                .Callback<Event>(r => _event = r);

            var factory = new SchedulerFactory();
            _scheduler = factory.CreateInstance(WeeklySchedulerTypeName);
        }

        [Test]
        public void CreateWeeklyEventDto_OneWeekInterval_Monday_NoEndDate_Success()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 1,
                WeekDays = WeekDaysFlags.Monday,
                StartDate = new DateTime(2015, 2, 15, 20, 0, 0),
                EndDate = null,
                RepeatCount = null
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert
            Assert.IsNotNull(_event);
            Assert.IsNotEmpty(_event.EventData);
            Assert.AreEqual(1, _event.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(_event.EventData);
            Assert.AreEqual(new DateTime(2015, 2, 16, 20, 0, 0).ToUniversalTime(), _event.EventData.First().StartDate);
            Assert.False(_event.EventData.First().EndDate.HasValue);
            Assert.AreEqual(7, _event.EventData.First().RepeatInterval);
            Assert.False(_event.EventData.First().RepeatWeekOfMonth.HasValue);
            Assert.False(_event.EventData.First().RepeatMonth.HasValue);
            Assert.False(_event.EventData.First().RepeatDay.HasValue);
            Assert.False(_event.EventData.First().RepeatDayOfWeek.HasValue);
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedEndDate_Success()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 3,
                WeekDays = WeekDaysFlags.Monday | WeekDaysFlags.Friday,
                StartDate = new DateTime(2015, 2, 15, 20, 0, 0),
                EndDate = new DateTime(2015, 5, 15, 20, 0, 0),
                RepeatCount = null
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert
            Assert.IsNotNull(_event);
            Assert.IsNotEmpty(_event.EventData);
            Assert.AreEqual(2, _event.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(_event.EventData);

            var expectedCollection = new List<EventTimeData>
            {
                new EventTimeData
                {
                    StartDate = new DateTime(2015,3,2,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,5,15,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeData
                {
                    StartDate = new DateTime(2015,3,6,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,5,15,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(_event.EventData, Has.All.Matches<EventTimeData>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_NoDayOfWeek_FixedEndDate_Success()
        {
            //Arrange
            var startDate = new DateTime(2015, 2, 15, 20, 0, 0);
            DateTime? endDate = new DateTime(2015, 5, 15, 20, 0, 0);
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 3,
                WeekDays = WeekDaysFlags.None,
                StartDate = startDate,
                EndDate = endDate,
                RepeatCount = null
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert
            Assert.IsNotNull(_event);
            Assert.IsNotEmpty(_event.EventData);
            Assert.AreEqual(1, _event.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(_event.EventData);
            Assert.AreEqual(startDate.ToUniversalTime(), _event.EventData.First().StartDate);
            Assert.IsTrue(_event.EventData.First().EndDate.HasValue);
            Assert.AreEqual(endDate.Value.ToUniversalTime(), _event.EventData.First().EndDate.Value);
            Assert.AreEqual(21, _event.EventData.First().RepeatInterval);
            Assert.False(_event.EventData.First().RepeatWeekOfMonth.HasValue);
            Assert.False(_event.EventData.First().RepeatMonth.HasValue);
            Assert.False(_event.EventData.First().RepeatDay.HasValue);
            Assert.False(_event.EventData.First().RepeatDayOfWeek.HasValue);
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_Monday_FixedRepeatCount_Success()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 3,
                WeekDays = WeekDaysFlags.Monday,
                StartDate = new DateTime(2015, 2, 15, 20, 0, 0),
                EndDate = null,
                RepeatCount = 3
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert
            Assert.IsNotNull(_event);
            Assert.IsNotEmpty(_event.EventData);
            Assert.AreEqual(1, _event.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(_event.EventData);
            Assert.AreEqual(new DateTime(2015, 3, 2, 20, 0, 0).ToUniversalTime(), _event.EventData.First().StartDate);
            Assert.IsTrue(_event.EventData.First().EndDate.HasValue);
            Assert.AreEqual(new DateTime(2015, 4, 13, 20, 0, 0).ToUniversalTime(), _event.EventData.First().EndDate.Value);
            Assert.AreEqual(21, _event.EventData.First().RepeatInterval);
            Assert.False(_event.EventData.First().RepeatWeekOfMonth.HasValue);
            Assert.False(_event.EventData.First().RepeatMonth.HasValue);
            Assert.False(_event.EventData.First().RepeatDay.HasValue);
            Assert.False(_event.EventData.First().RepeatDayOfWeek.HasValue);
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedRepeatCount_StartDateBetween_Success()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 3,
                WeekDays = WeekDaysFlags.Monday | WeekDaysFlags.Friday,
                StartDate = new DateTime(2015, 2, 18, 20, 0, 0),
                EndDate = null,
                RepeatCount = 3
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert
            Assert.IsNotNull(_event);
            Assert.IsNotEmpty(_event.EventData);
            Assert.AreEqual(2, _event.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(_event.EventData);

            var expectedCollection = new List<EventTimeData>
            {
                new EventTimeData
                {
                    StartDate = new DateTime(2015,3,9,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,9,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeData
                {
                    StartDate = new DateTime(2015,2,20,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,13,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(_event.EventData, Has.All.Matches<EventTimeData>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedRepeatCount_StartDateMonday_Success()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 3,
                WeekDays = WeekDaysFlags.Monday | WeekDaysFlags.Friday,
                StartDate = new DateTime(2015, 2, 16, 20, 0, 0),
                EndDate = null,
                RepeatCount = 3
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert
            Assert.IsNotNull(_event);
            Assert.IsNotEmpty(_event.EventData);
            Assert.AreEqual(2, _event.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(_event.EventData);

            var expectedCollection = new List<EventTimeData>
            {
                new EventTimeData
                {
                    StartDate = new DateTime(2015,2,16,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,9,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeData
                {
                    StartDate = new DateTime(2015,2,20,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,2,20,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(_event.EventData, Has.All.Matches<EventTimeData>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        public void CreateWeeklyEventDto_TreeWeekInterval_MondayFriday_FixedRepeatCount_StartDateSunday_Success()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 3,
                WeekDays = WeekDaysFlags.Monday | WeekDaysFlags.Friday,
                StartDate = new DateTime(2015, 2, 15, 20, 0, 0),
                EndDate = null,
                RepeatCount = 3
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert
            Assert.IsNotNull(_event);
            Assert.IsNotEmpty(_event.EventData);
            Assert.AreEqual(2, _event.EventData.Count);
            CollectionAssert.AllItemsAreNotNull(_event.EventData);

            var expectedCollection = new List<EventTimeData>
            {
                new EventTimeData
                {
                    StartDate = new DateTime(2015,3,2,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,23,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                },
                new EventTimeData
                {
                    StartDate = new DateTime(2015,3,6,20,0,0).ToUniversalTime(),
                    EndDate = new DateTime(2015,3,6,20,0,0).ToUniversalTime(),
                    RepeatInterval = 21,
                }
            };

            Assert.That(_event.EventData, Has.All.Matches<EventTimeData>(f => IsInExpected(f, expectedCollection)));
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid input. EndDate and repeatCount have value.")]
        public void CreateWeeklyEventDto_FixedRepeatCount_And_FixedEndDate_Fails()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 3,
                WeekDays = WeekDaysFlags.Monday,
                StartDate = new DateTime(2015, 2, 16, 20, 0, 0),
                EndDate = new DateTime(2015, 3, 15, 20, 0, 0),
                RepeatCount = 3
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert - attribute
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException), ExpectedMessage = "Invalid input. Number of weeks have to be greater or equal 1.")]
        public void CreateWeeklyEventDto_ZeroAsNumberOfWeeks_Fails()
        {
            //Arrange
            var eventData = new WeeklySchedulerEventData
            {
                Interval = 0,
                WeekDays = WeekDaysFlags.Monday,
                StartDate = new DateTime(2015, 2, 15, 20, 0, 0),
                EndDate = null,
                RepeatCount = 3
            };

            //Act
            _scheduler.ScheduleEvent(eventData, _eventsRepositoryMock.Object);

            //Assert - attribute
        }



        private static bool IsInExpected(EventTimeData item, IEnumerable<EventTimeData> expected)
        {
            var matchedItem = expected.FirstOrDefault(f =>
                f.Id == item.Id &&
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
