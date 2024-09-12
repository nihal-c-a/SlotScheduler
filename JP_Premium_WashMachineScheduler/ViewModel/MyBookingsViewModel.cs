namespace JP_Premium_WashMachineScheduler.Models
{
    public class MyBookingsViewModel
    {
        public List<BookingInfoViewModel> ActiveBookingsCurrent { get; set; }
        public List<BookingInfoViewModel> ActiveBookingsNext { get; set; }


        public List<BookingInfoViewModel> BookingHistory { get; set; }
    }
}
