﻿using System;
using mCalendar.DomainModels.Interfaces;
using mCalendar.DomainModels.SchedulerData;
using mCalendar.Models.Repositories;

namespace mCalendar.DomainModels.Schedulers
{
    public class MonthlyEventsScheduler : IScheduler
    {
        public void ScheduleEvent(SchedulerEventData eventData, IEventRepository repository)
        {
            throw new NotImplementedException();
        }
    }
}