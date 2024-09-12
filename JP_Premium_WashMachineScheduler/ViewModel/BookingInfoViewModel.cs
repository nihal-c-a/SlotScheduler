namespace JP_Premium_WashMachineScheduler.Models
{
    public class BookingInfoViewModel
    {
        public int bookingId {  get; set; }
        public string BookedBy { get; set; }
        public string MachineName { get; set; }
        public string PhoneNumber {  get; set; }
        public DateTime dateTime { get; set; }
        public TimeSpan TimeSlot { get; set; }
    }
}
