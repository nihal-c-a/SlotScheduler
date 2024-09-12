using JP_Premium_WashMachineScheduler.Models;
using JP_Premium_WashMachineScheduler.Services;
using JP_Premium_WashMachineScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JP_Premium_WashMachineScheduler.Controllers
{
    public class BookingController : Controller
    {
        private readonly IWashingMachineService _washingMachineService;
        private readonly IBookingService _bookingService;
        private readonly ITenantService _tenantService;

        public BookingController(IWashingMachineService washingMachineService, IBookingService bookingService, ITenantService tenantService)
        {
            _washingMachineService = washingMachineService;
            _bookingService = bookingService;
            _tenantService = tenantService;
        }

        // GET: Booking/SelectDay
        public IActionResult SelectDay()
        {
            var model = new SelectDayViewModel
            {
                UpcomingDays = GetNextSevenDays()
            };

            return View(model);
        }

        // POST: Booking/SelectDay
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectDay(DateTime selectedDate)
        {
            // Store the selected date in TempData for the next step
            TempData["SelectedDate"] = selectedDate;

            // Redirect to the next step (e.g., SelectMachine)
            return RedirectToAction("SelectMachine");
        }

        private List<DateTime> GetNextSevenDays()
        {
            var days = new List<DateTime>();
            var tomorrow = DateTime.Today;

            for (int i = 0; i < 7; i++)
            {
                days.Add(tomorrow.AddDays(i));
            }

            return days;
        }

        // GET: Booking/SelectMachine
        public async Task<IActionResult> SelectMachine()
        {
            var machines = await _washingMachineService.GetAvailableWashingMachinesAsync();

            var model = new SelectMachineViewModel
            {
                WashingMachines = machines
            };

            return View(model);
        }

        // POST: Booking/SelectMachine
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SelectMachine(SelectMachineViewModel model)
        {
           
          
            // Assign SelectedMachineId to TempData if present in the model
            TempData["SelectedMachineId"] = model.SelectedMachineId;

            return RedirectToAction("SelectTimeSlot");
        }
        // GET: Booking/SelectTimeSlot
        public async Task<IActionResult> SelectTimeSlot()
        {
           
            if (TempData["SelectedDate"] == null)
            {
                return RedirectToAction("SelectDay");
            }
            var selectedDate = (DateTime)TempData["SelectedDate"];
            var selectedMachineId = (int)TempData["SelectedMachineId"];

            var timeSlots = GetAvailableTimeSlots(selectedDate, selectedMachineId);

            var model = new SelectTimeSlotViewModel
            {
                BookingDate = selectedDate,
                MachineId = selectedMachineId,
                TimeSlots = timeSlots
            };

            return View(model);
        }
        [Authorize(Roles = "Tenant")]
        // POST: Booking/SelectTimeSlot
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> SelectTimeSlot(SelectTimeSlotViewModel model)
        {



            var currentTime = DateTime.Now;
            var userId = await _tenantService.GetUserIdAsync();
            var tenantId =(int) await _tenantService.GetTenantIdByUserIdAsync(userId);

            var bookings = await _bookingService.GetBookingsByTenantIdAsync(tenantId);
            int currentbookingLimit=4,nextBookingLimit = 4; // Example limit, adjust as needed

          
            int currentWeekBookings = bookings.Count(b => IsDateInCurrentWeek(b.dateTime));
            int NextWeekBookings = bookings.Count(b => !IsDateInCurrentWeek(b.dateTime));
            if ((IsDateInCurrentWeek(model.BookingDate)&&currentWeekBookings >= currentbookingLimit))
            {
                TempData["BookingLimitReached"] = "You have already reached your limit for this week.";
                return RedirectToAction("ViewMyBookings");
            }
            else if (!IsDateInCurrentWeek(model.BookingDate) && NextWeekBookings >= nextBookingLimit)
                    {
                TempData["BookingLimitReached"] = "You have already reached your limit for next week.";
                return RedirectToAction("ViewMyBookings");

            }
            else
            {






                await _bookingService.CreateBookingAsync(tenantId, model.MachineId, model.BookingDate, model.SelectedTimeSlot);

                // Redirect to confirmation or success page
                var bookedBy = await _tenantService.GetTenantNameByIdAsync(tenantId);
                var machineName = await _washingMachineService.GetMachineNameByIdAsync(model.MachineId);
                var phoneNumber = await _tenantService.GetTenantPhoneByIdAsync(tenantId);
                var datetime = model.BookingDate;
                var timeSlot = model.SelectedTimeSlot;

                // Redirect to BookingInfo page with data passed as arguments
                return RedirectToAction("BookingInfo", new
                {
                    bookedBy = bookedBy,
                    machineId = model.MachineId,
                    phoneNumber = phoneNumber,
                    datetime = datetime,
                    timeSlot = timeSlot
                });


                // If model state is invalid, reload the time slots
                model.TimeSlots = GetAvailableTimeSlots(model.BookingDate, model.MachineId);
                return View(model);
            }
        }

        private List<TimeSlot> GetAvailableTimeSlots(DateTime date, int machineId)
        {
            var timeSlots = new List<TimeSlot>();
            var startTime = new TimeSpan(0, 0, 0); // Start of day
            var endTime = new TimeSpan(23, 30, 0); // End of day

            for (var time = startTime; time <= endTime; time = time.Add(TimeSpan.FromMinutes(30)))
            {
                var isAvailable = _bookingService.IsSlotAvailableAsync(machineId, date, time).Result;
                timeSlots.Add(new TimeSlot { Slot = time, IsAvailable = isAvailable });
            }

            return timeSlots;
        }
        public async Task<IActionResult> BookingInfo(DateTime datetime, TimeSpan timeSlot, int MachineId =-1, string bookedBy = "Empty",string MachineName="Empty")
        {
            // Check if bookedBy is "Empty"
            string phoneNumber;
            if (bookedBy == "Empty")
            {
                // Assuming you have the machine ID and booking date, fetch the booking details
                var tenantId = await _bookingService.GetTenantIdByBookingAsync(MachineId,datetime, timeSlot);
                var BookedBy= await _tenantService.GetTenantNameByIdAsync(tenantId);
                 phoneNumber = await _tenantService.GetTenantPhoneByIdAsync(tenantId);

                if (BookedBy != null)
                {
                    bookedBy = BookedBy;
                }
            }
            else
            {
                var tenantId=(int) await _tenantService.GetTenantIdByNameAsync(bookedBy);
                phoneNumber = await _tenantService.GetTenantPhoneByIdAsync(tenantId);
            }
            
            string machineName;
            if (MachineId != -1)
            {
                 machineName = await _washingMachineService.GetMachineNameByIdAsync(MachineId);
            }
            else
            {
                 machineName = MachineName;
            }

            // Build the BookingInfoViewModel
            var bookingInfo = new BookingInfoViewModel
            {
                BookedBy = bookedBy,
                MachineName = machineName,
                PhoneNumber = phoneNumber,
                dateTime = datetime,
                TimeSlot = timeSlot
            };

            // Return the view with the booking information
            return View(bookingInfo);
        }


        public async Task<IActionResult> TodayBookings()
        {
            var today = DateTime.Today;
            var bookings = await _bookingService.GetBookingsByDateAsync(today);

            var bookingViewModels = new List<BookingInfoViewModel>();

            foreach (var booking in bookings)
            {
                var machineName = await _washingMachineService.GetMachineNameByIdAsync(booking.MachineId);
                var tenantName = await _tenantService.GetTenantNameByIdAsync(booking.TenantId);

                bookingViewModels.Add(new BookingInfoViewModel
                {
                    MachineName = machineName,
                    BookedBy = tenantName,
                    TimeSlot = booking.TimeSlot,
                    dateTime = today
                });
            }

            return View(bookingViewModels);
        }





        public async Task<IActionResult> ViewMyBookings()
        {
            var userId = await _tenantService.GetUserIdAsync();
            var tenantId = (int)await _tenantService.GetTenantIdByUserIdAsync(userId);

            var bookings = await _bookingService.GetBookingsByTenantIdAsync(tenantId);
            var currentTime = DateTime.Now;

            var ActiveBookingsCurrent = bookings.Where(b =>( b.dateTime.Date > currentTime.Date
                                      || (b.dateTime.Date == currentTime.Date && b.TimeSlot > currentTime.TimeOfDay))&& IsDateInCurrentWeek(b.dateTime.Date))
                             .ToList();

            var ActiveBookingsNext = bookings.Where(b => b.dateTime.Date > currentTime.Date
                                      && !IsDateInCurrentWeek(b.dateTime.Date))
                            .ToList();

            

        


            // Booking history: Past dates or today's slots that are in the past or now
            var bookingHistory = bookings.Where(b => b.dateTime.Date < currentTime.Date
                                                  || (b.dateTime.Date == currentTime.Date && b.TimeSlot <= currentTime.TimeOfDay))
                                         .Reverse().ToList();
            // Check if user has reached booking limit for the week


            var model = new MyBookingsViewModel
            {
                ActiveBookingsCurrent = ActiveBookingsCurrent,
                ActiveBookingsNext=ActiveBookingsNext,
                BookingHistory = bookingHistory
            };

            return View(model);
        }

        // Helper method to check if a date is in the current week
        private bool IsDateInCurrentWeek(DateTime date)
        {
            var currentDate = DateTime.Now;

            // Find the start of the current week (Sunday)
            var startOfWeek = currentDate.Date.AddDays(-(int)currentDate.DayOfWeek);

            // Find the end of the current week (Saturday)
            var endOfWeek = startOfWeek.AddDays(6);

            // Check if the given date is between the start and end of the current week
            return date.Date >= startOfWeek && date.Date <= endOfWeek;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBooking(int bookingId)
        {
            await _bookingService.DeleteBookingAsync(bookingId);
            return RedirectToAction("ViewMyBookings");
        }


    }
}
