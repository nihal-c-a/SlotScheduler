using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JP_Premium_WashMachineScheduler.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [ForeignKey("Tenant")]
        public int TenantId { get; set; }
        public Tenant Tenant { get; set; }

        [Required]
        [ForeignKey("WashingMachine")]
        public int MachineId { get; set; }
        public WashingMachine WashingMachine { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }

        [Required]
        public TimeSpan TimeSlot { get; set; }

        public bool IsBooked { get; set; }
    }
}
