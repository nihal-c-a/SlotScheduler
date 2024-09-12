using Microsoft.AspNetCore.Mvc;

namespace JP_Premium_WashMachineScheduler.Controllers
{
    public class TenantHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
