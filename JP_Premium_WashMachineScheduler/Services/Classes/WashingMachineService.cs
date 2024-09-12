using JP_Premium_WashMachineScheduler.Data;
using JP_Premium_WashMachineScheduler.Models;
using JP_Premium_WashMachineScheduler.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JP_Premium_WashMachineScheduler.Services.Classes
{
    public class WashingMachineService : IWashingMachineService
    {
        private readonly ApplicationDbContext _context;

        public WashingMachineService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddWashingMachineAsync(string machineName)
        {
            var machine = new WashingMachine
            {
                MachineName = machineName,
                IsAvailable = true
            };

            _context.WashingMachines.Add(machine);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<WashingMachine>> GetAvailableMachinesAsync()
        {
            return await _context.WashingMachines.ToListAsync();
        }

        public async Task<WashingMachine> GetMachineByIdAsync(int id)
        {
            return await _context.WashingMachines.FindAsync(id);
        }

        public async Task<bool> UpdateMachineAsync(WashingMachine machine)
        {
            _context.Entry(machine).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMachineAsync(int id)
        {
            var machine = await _context.WashingMachines.FindAsync(id);
            if (machine == null)
            {
                return false;
            }

            _context.WashingMachines.Remove(machine);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<WashingMachine>> GetAvailableWashingMachinesAsync()
        {
            return await _context.WashingMachines
                .Where(m => m.IsAvailable)
                .ToListAsync();
        }
        public async Task<string> GetMachineNameByIdAsync(int machineId)
        {
            var machine = await _context.WashingMachines
                .FirstOrDefaultAsync(m => m.MachineId == machineId);

            return machine?.MachineName;
        }






    }
}
