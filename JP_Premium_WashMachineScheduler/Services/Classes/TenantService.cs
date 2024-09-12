using JP_Premium_WashMachineScheduler.Data;
using JP_Premium_WashMachineScheduler.Models;
using JP_Premium_WashMachineScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JP_Premium_WashMachineScheduler.Services.Classes
{
    public class TenantService : ITenantService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly PasswordHasher<IdentityUser> _passwordHasher;
        private readonly SignInManager<IdentityUser> _signInManager;

        public TenantService(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _passwordHasher = new PasswordHasher<IdentityUser>();
            _signInManager = signInManager;
        }

        public async Task<bool> CreateTenantAsync(string userName, string name, string phoneNumber, string roomNumber)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
            {
                throw new ArgumentException("Room number is required.");
            }

            var user = new IdentityUser { UserName = userName, Email = userName,EmailConfirmed=true };
            var defaultPassword = phoneNumber+"@Jp";

            user.PasswordHash = _passwordHasher.HashPassword(user, defaultPassword);
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "tenant");

                var tenant = new Tenant
                {
                    UserId = user.Id,
                    Name = name,
                    PhoneNumber = phoneNumber,
                    RoomNumber = roomNumber,
                    Password = defaultPassword
                };

                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> UpdateTenantAsync(int tenantId,string name, string phoneNumber, string roomNumber)
        {
            if (string.IsNullOrWhiteSpace(roomNumber))
            {
                throw new ArgumentException("Room number is required.");
            }

            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null)
            {
                return false;
            }
            tenant.Name = name;

            tenant.PhoneNumber = phoneNumber;
            tenant.RoomNumber = roomNumber;
            

            var user = await _userManager.FindByIdAsync(tenant.UserId);
            if (user != null)
            {
               
               
                await _userManager.UpdateAsync(user);
            }

            
            _context.Tenants.Update(tenant);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Tenant> GetTenantByIdAsync(int tenantId)
        {
            return await _context.Tenants.FindAsync(tenantId);
        }

        public async Task<Tenant> GetTenantByUserIdAsync(string userId)
        {
            return await _context.Tenants.FirstOrDefaultAsync(t => t.UserId == userId);
        }

        public async Task<List<Tenant>> GetAllTenantsAsync()
        {
            return await _context.Tenants.ToListAsync();
        }

        public async Task<bool> DeleteTenantAsync(int tenantId)
        {
            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant == null)
            {
                return false;
            }

            var user = await _userManager.FindByIdAsync(tenant.UserId);
            if (user != null)
            {
                _context.Tenants.Remove(tenant);
                await _userManager.DeleteAsync(user);
            }

            
            await _context.SaveChangesAsync();

            return true;
        }
        public string  GetTenantEmailByUserIdAsync(string userId)
        {
            var user =  _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Email)
                .FirstOrDefault();

            return user;
        }
        public async Task<string> GetUserIdAsync()
        {
            var user = await _signInManager.UserManager.GetUserAsync(_signInManager.Context.User);
            return user?.Id;
        }
        public async Task<int?> GetTenantIdByUserIdAsync(string userId)
        {
            var tenant = await _context.Tenants
                                       .FirstOrDefaultAsync(t => t.UserId == userId);

            return tenant?.TenantId;
        }
        public async Task<string> GetTenantNameByIdAsync(int tenantId)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.TenantId == tenantId);

            return tenant?.Name;
        }
        public async Task<int?> GetTenantIdByNameAsync(string name)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.Name == name);

            return tenant?.TenantId;
        }
        public async Task<string> GetTenantPhoneByIdAsync(int tenantId)
        {
            var tenant = await _context.Tenants
                .FirstOrDefaultAsync(t => t.TenantId == tenantId);

            return tenant?.PhoneNumber;
        }

    }
}
