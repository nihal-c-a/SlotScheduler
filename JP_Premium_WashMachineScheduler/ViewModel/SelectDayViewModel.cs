using System;
using System.Collections.Generic;

namespace JP_Premium_WashMachineScheduler.Models
{
    public class SelectDayViewModel
    {
        public List<DateTime> UpcomingDays { get; set; }

        public DateTime SelectedDate { get; set; }
    }
}
