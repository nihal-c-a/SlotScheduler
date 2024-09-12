using System.ComponentModel.DataAnnotations;

namespace JP_Premium_WashMachineScheduler.Models
{
    public class WashingMachine
    {
        [Key]
        public int MachineId { get; set; }

        [Required]
        public string MachineName { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}
