using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JP_Premium_WashMachineScheduler.Models;

namespace JP_Premium_WashMachineScheduler.Services
{
    public interface IBookingService
    {
        Task<Booking> CreateBookingAsync(int tenantId, int machineId, DateTime bookingDate, TimeSpan timeSlot);
        Task<Booking> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByTenantAsync(int tenantId);
        Task<IEnumerable<Booking>> GetBookingsByDateAndMachineAsync(DateTime date, int machineId);
        Task<bool> IsSlotAvailableAsync(int machineId, DateTime bookingDate, TimeSpan timeSlot);
        Task CancelBookingAsync(int bookingId);
        public Task<int?> GetMachineIdByNameAsync(string machineName);
        public Task<int> GetTenantIdByBookingAsync(int machineId, DateTime date, TimeSpan timeSlot);
     
        Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date);
        Task<IEnumerable<BookingInfoViewModel>> GetBookingsByTenantIdAsync(int tenantId);
        Task DeleteBookingAsync(int bookingId);


        // public Task<BookingInfoViewModel> GetBookingDetailsAsync(string machineName,DateTime datetime, string timeSlot);
    }
}
