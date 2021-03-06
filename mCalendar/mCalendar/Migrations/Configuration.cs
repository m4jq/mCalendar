using System.Collections.Generic;
using mCalendar.Models;

namespace mCalendar.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ScheduleContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ScheduleContext context)
        {
            context.Schedules.AddOrUpdate(
                new Schedule
                {
                    Events = new List<Event>
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
                    },
                    Title = "Work activities schedule",
                    UserId = "John Doe"
                }

                
                );
        }
    }
}
