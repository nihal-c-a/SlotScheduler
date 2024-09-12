using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JP_Premium_WashMachineScheduler.Data;
using JP_Premium_WashMachineScheduler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace JP_Premium_WashMachineScheduler.Services
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;

        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize]

        public async Task<Booking> CreateBookingAsync(int tenantId, int machineId, DateTime bookingDate, TimeSpan timeSlot)
        {
            if (!await IsSlotAvailableAsync(machineId, bookingDate, timeSlot))
            {
                throw new InvalidOperationException("The selected slot is already booked.");
            }

            var booking = new Booking
            {
                TenantId = tenantId,
                MachineId = machineId,
                BookingDate = bookingDate,
                TimeSlot = timeSlot,
                IsBooked = true
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return booking;
        }

        public async Task<Booking> GetBookingByIdAsync(int bookingId)
        {
            return await _context.Bookings
                .Include(b => b.Tenant)
                .Include(b => b.WashingMachine)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByTenantAsync(int tenantId)
        {
            return await _context.Bookings
                .Where(b => b.TenantId == tenantId)
                .Include(b => b.WashingMachine)
                .ToListAsync();
        }

        public async Task<IEnumerable<Booking>> GetBookingsByDateAndMachineAsync(DateTime date, int machineId)
        {
            return await _context.Bookings
                .Where(b => b.BookingDate.Date == date.Date && b.MachineId == machineId)
                .ToListAsync();
        }

        public async Task<bool> IsSlotAvailableAsync(int machineId, DateTime bookingDate, TimeSpan timeSlot)
        {
            return !await _context.Bookings
                .AnyAsync(b => b.MachineId == machineId && b.BookingDate.Date == bookingDate.Date && b.TimeSlot == timeSlot);
        }

        public async Task CancelBookingAsync(int bookingId)
        {
            var booking = await GetBookingByIdAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<int?> GetMachineIdByNameAsync(string machineName)
        {
            return await _context.WashingMachines
                .Where(wm => wm.MachineName == machineName)
                .Select(wm => wm.MachineId)
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetTenantIdByBookingAsync(int MachineId, DateTime date, TimeSpan timeSlot)
        {
            // Step 1: Get the MachineID based on the MachineName
            
            // Step 2: Use the MachineID to query the Booking table and get the TenantName
            var tenantId = await _context.Bookings
                .Include(b => b.Tenant) // Include Tenant to access TenantName
                .Where(b => b.MachineId == MachineId
                            && b.BookingDate == date.Date
                            && b.TimeSlot == timeSlot)
                .Select(b => b.Tenant.TenantId) // Select only the TenantName
                .FirstOrDefaultAsync();

            return tenantId;
        }
       
        public async Task<IEnumerable<Booking>> GetBookingsByDateAsync(DateTime date)
        {
            return await _context.Bookings
                .Where(b => b.BookingDate.Date == date.Date)
                .ToListAsync();
        }


        public async Task<IEnumerable<BookingInfoViewModel>> GetBookingsByTenantIdAsync(int tenantId)
        {
            var bookings = await _context.Bookings
                .Include(b => b.WashingMachine)
                .Include(b => b.Tenant)
                .Where(b => b.TenantId == tenantId)
                .Select(b => new BookingInfoViewModel
                {
                    bookingId = b.BookingId,
                    MachineName = b.WashingMachine.MachineName,
                    BookedBy = b.Tenant.Name,
                    dateTime = b.BookingDate,
                    TimeSlot = b.TimeSlot
                })
                .ToListAsync();

            return bookings;
        }

        public async Task DeleteBookingAsync(int bookingId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
            }
        }

    }
}
