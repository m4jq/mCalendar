using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mCalendar.DomainModels.Interfaces;

namespace mCalendar.DomainModels
{
    public class SmsReminder : IReminder
    {
        public void Remind()
        {
            //send sms
        }
    }
}