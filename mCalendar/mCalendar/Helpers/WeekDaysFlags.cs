﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mCalendar.Helpers
{
    [Flags]
    public enum WeekDaysFlags
    {
        None = 0,
        Monday = 1 << 0,
        Tuesday = 1 << 1,
        Wednesday = 1 << 2,
        Thursday = 1 << 3,
        Friday = 1 << 4,
        Saturday = 1 << 5,
        Sunday = 1 << 6
    }
}