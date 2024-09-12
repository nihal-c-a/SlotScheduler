using System.ComponentModel.DataAnnotations;

namespace JP_Premium_WashMachineScheduler.ViewModel
{
    public class TenantViewModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress] // Assuming email as the username for simplicity
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string RoomNumber { get; set; }

       
    }
}
