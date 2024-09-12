using JP_Premium_WashMachineScheduler.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JP_Premium_WashMachineScheduler.Services.Interfaces
{
    public interface ITenantService
    {
        Task<bool> CreateTenantAsync(string userName, string name, string phoneNumber, string roomNumber);
        Task<bool> UpdateTenantAsync(int tenantId, string name, string phoneNumber, string roomNumber);
        Task<Tenant> GetTenantByIdAsync(int tenantId);
        Task<Tenant> GetTenantByUserIdAsync(string userId);
        Task<List<Tenant>> GetAllTenantsAsync();
        Task<bool> DeleteTenantAsync(int tenantId);
       public string GetTenantEmailByUserIdAsync(string userId);
        Task<string> GetUserIdAsync();
        Task<int?> GetTenantIdByUserIdAsync(string userId);
        Task<string> GetTenantNameByIdAsync(int tenantId);
        public Task<int?> GetTenantIdByNameAsync(string name);
        Task<string> GetTenantPhoneByIdAsync(int tenantId);

    }
}
