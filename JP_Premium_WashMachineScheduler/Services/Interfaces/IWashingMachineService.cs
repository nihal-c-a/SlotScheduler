using JP_Premium_WashMachineScheduler.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JP_Premium_WashMachineScheduler.Services.Interfaces
{
    public interface IWashingMachineService
    {
        Task<bool> AddWashingMachineAsync(string machineName);
        Task<IEnumerable<WashingMachine>> GetAvailableMachinesAsync();
        Task<WashingMachine> GetMachineByIdAsync(int id);
        Task<bool> UpdateMachineAsync(WashingMachine machine);
        Task<bool> DeleteMachineAsync(int id);
        Task<List<WashingMachine>> GetAvailableWashingMachinesAsync();
        Task<string> GetMachineNameByIdAsync(int machineId);

    }
}
