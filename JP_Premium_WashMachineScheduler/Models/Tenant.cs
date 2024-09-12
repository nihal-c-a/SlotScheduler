using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace JP_Premium_WashMachineScheduler.Models
{
    public class Tenant
    {
        [Key]
        public int TenantId { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        [Required]
        public string Password { get; set; }  // This should be handled securely; consider hashing passwords
    }
}
