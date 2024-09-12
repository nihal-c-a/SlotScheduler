using System;
using System.Collections.Generic;

namespace JP_Premium_WashMachineScheduler.Models
{
    public class SelectTimeSlotViewModel
    {
        public DateTime BookingDate { get; set; }
        public int MachineId { get; set; }
        public List<TimeSlot> TimeSlots { get; set; } // List of available time slots
        public TimeSpan SelectedTimeSlot { get; set; }
    }

    public class TimeSlot
    {
        public TimeSpan Slot { get; set; }
        public bool IsAvailable { get; set; }
    }
}
