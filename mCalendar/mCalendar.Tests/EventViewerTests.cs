using System;
using System.Collections.Generic;
using System.Linq;
using mCalendar.DomainModels;
using mCalendar.Models;
using mCalendar.Models.Repositories;
using Moq;
using NUnit.Framework;

namespace mCalendar.Tests
{
    [TestFixture]
    public class EventViewerTests
    {
        private Mock<IEventRepository> _eventsRepositoryMock;

        [SetUp]
        public void SetUp()
        {
            _eventsRepositoryMock = new Mock<IEventRepository>();
            _eventsRepositoryMock.Setup(x => x.Events).Returns(GetAllEvents());
        }

        [Test]
        public void GetEvents_OneDayView_Success()
        {
            //Arrange
            var date = new DateTime(2015, 2, 10);
            var eventViewer = new EventViewer(_eventsRepositoryMock.Object);

            //Act
            Dictionary<DateTime, List<EventDto>> result = eventViewer.GetEvents(date, date);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(7, result[date].Count);
            Assert.False(result[date].Any(e => e.Title == "Complex event 1"));
            Assert.False(result[date].Any(e => e.Title == "Complex event 2"));
        }


        [Test]
        public void GetEvents_4DayView_Success()
        {
            //Arrange
            var startDate = new DateTime(2015, 2, 10);
            var endDate = new DateTime(2015, 2, 13); 
            var eventViewer = new EventViewer(_eventsRepositoryMock.Object);

            //Act
            Dictionary<DateTime, List<EventDto>> result = eventViewer.GetEvents(startDate, endDate);

            //Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(7, result[startDate].Count);
            Assert.False(result[startDate].Any(e => e.Title == "Complex event 1"));
            Assert.False(result[startDate].Any(e => e.Title == "Complex event 2"));

            for (int i = 1; i < 4; i++)
            {
                Assert.AreEqual(1, result[startDate.AddDays(i)].Count);
                Assert.True(result[startDate.AddDays(i)].Any(e => e.Title == "Daily event"));
            }
        }

        [Test]
        public void GetEvents_WeekView_Success()
        {
            //Arrange
            var startDate = new DateTime(2015, 2, 9);
            var endDate = new DateTime(2015, 2, 15);
            var eventViewer = new EventViewer(_eventsRepositoryMock.Object);

            //Act
            Dictionary<DateTime, List<EventDto>> result = eventViewer.GetEvents(startDate, endDate);

            //Assert
            Assert.IsNotEmpty(result);

            Assert.IsEmpty(result[startDate]);

            Assert.AreEqual(7, result[startDate.AddDays(1)].Count);
            Assert.False(result[startDate.AddDays(1)].Any(e => e.Title == "Complex event 1"));
            Assert.False(result[startDate.AddDays(1)].Any(e => e.Title == "Complex event 2"));

            for (int i = 2; i < 7; i++)
            {
                Assert.AreEqual(1, result[startDate.AddDays(i)].Count);
                Assert.True(result[startDate.AddDays(i)].Any(e => e.Title == "Daily event"));
            }
        }

        [Test]
        public void GetEvents_MonthView_Success()
        {
            //Arrange
            var startDate = new DateTime(2015, 2, 1);
            var endDate = new DateTime(2015, 2, 28);
            var eventViewer = new EventViewer(_eventsRepositoryMock.Object);

            //Act
            Dictionary<DateTime, List<EventDto>> result = eventViewer.GetEvents(startDate, endDate);

            //Assert
            Assert.IsNotEmpty(result);

            for (int i = 0; i < 9; i++)
            {
                Assert.IsEmpty(result[startDate.AddDays(i)]);
            }

            Assert.AreEqual(7, result[startDate.AddDays(9)].Count);
            Assert.False(result[startDate.AddDays(9)].Any(e => e.Title == "Complex event 1"));
            Assert.False(result[startDate.AddDays(9)].Any(e => e.Title == "Complex event 2"));

            for (int i = 10; i < 28; i++)
            {
                if (i == 16 || i == 23)
                {
                    Assert.AreEqual(2, result[startDate.AddDays(i)].Count);
                    Assert.True(result[startDate.AddDays(i)].Any(e => e.Title == "Daily event"));
                    Assert.True(result[startDate.AddDays(i)].Any(e => e.Title == "Weekly event"));
                }
                else
                {
                    Assert.AreEqual(1, result[startDate.AddDays(i)].Count);
                    Assert.True(result[startDate.AddDays(i)].Any(e => e.Title == "Daily event"));
                }

            }
        }

        private IQueryable<Event> GetAllEvents()
        {
            return new List<Event>
            {
                new Event
                        {
                            Description = "One-time event: With no length",
                            Title = "With no length",
                            Duration = new TimeSpan(0), 
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    EndDate = new DateTime(2015, 2, 10, 9, 30, 0)
                                }
                            }

                        },
                        new Event
                        {
                            Description = "One-time event: With fixed length",
                            Title = "With fixed length",
                            Duration = new TimeSpan(1, 0, 0),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    EndDate = new DateTime(2015, 2, 10, 10, 30, 0)
                                }
                            }

                        },
                        new Event
                        {
                            Description = "One-time event: Full day",
                            Title = "With fixed length",
                            Duration = new TimeSpan(23,59,59),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10),
                                    EndDate = new DateTime(2015, 2, 10, 23, 59, 59)
                                }
                            }

                        },
                        new Event
                        {
                            Description = "Recouring events - simple: Daily event",
                            Title = "Daily event",
                            Duration = new TimeSpan(1, 0, 0),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    RepeatInterval = 1
                                }
                            }

                        },
                        new Event
                        {
                            Description = "Recouring events - simple: Weekly event",
                            Title = "Weekly event",
                            Duration = new TimeSpan(1, 0, 0),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    RepeatInterval = 7
                                }
                            }

                        },
                        new Event
                        {
                            Description = "Recouring events - simple: Monthly event",
                            Title = "Monthly event",
                            Duration = new TimeSpan(1, 0, 0),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    RepeatDay = 10
                                }
                            }

                        },
                        new Event
                        {
                            Description = "Recouring events - simple: Annual event",
                            Title = "Annual event",
                            Duration = new TimeSpan(1, 0, 0),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    RepeatDay = 10, 
                                    RepeatMonth = 2
                                }
                            }

                        },
                        new Event
                        {
                            Description = "Recouring events - complex: Occours every thuesday and friday of every 2nd week of May every year till 2050",
                            Title = "Complex event 1",
                            Duration = new TimeSpan(1, 0, 0),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    EndDate = new DateTime(2050, 1, 1),
                                    RepeatDayOfWeek = 2,
                                    RepeatWeekOfMonth = 2,
                                    RepeatMonth = 5
                                },
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    EndDate = new DateTime(2050, 1, 1),
                                    RepeatDayOfWeek = 5,
                                    RepeatWeekOfMonth = 2,
                                    RepeatMonth = 5
                                },
                            }

                        },
                         new Event
                        {
                            Description = "Recouring events - complex: Occours every monday on summer break (June, July, August)",
                            Title = "Complex event 2",
                            Duration = new TimeSpan(1, 0, 0),
                            IsPrivate = false,
                            Reminders = new List<Reminder>(),
                            EventData = new List<EventTimeData>
                            {
                                new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    RepeatDayOfWeek = 1,
                                    RepeatMonth = 6
                                },
                                 new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    RepeatDayOfWeek = 1,
                                    RepeatMonth = 7
                                },
                                 new EventTimeData
                                {
                                    StartDate = new DateTime(2015, 2, 10, 9, 30, 0),
                                    RepeatDayOfWeek = 1,
                                    RepeatMonth = 8
                                }
                            }
                        }
                
            }.AsQueryable();
        }
    }
}
