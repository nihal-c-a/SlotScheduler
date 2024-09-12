using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JP_Premium_WashMachineScheduler.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageTenants()
        {
            return RedirectToAction("Index", "Tenant");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddWashingMachine()
        {
            return RedirectToAction("Index", "WashingMachine");
        }

        public IActionResult ViewBookings()
        {
            return RedirectToAction("Selectday", "Booking");
        }
    }
}
