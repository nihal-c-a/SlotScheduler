using JP_Premium_WashMachineScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using JP_Premium_WashMachineScheduler.ViewModel;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace JP_Premium_WashMachineScheduler.Controllers
{
    public class TenantController : Controller
    {
        private readonly ITenantService _tenantService;

        public TenantController(ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        public async Task<IActionResult> Index()
        {
            var tenants = await _tenantService.GetAllTenantsAsync();
            return View(tenants);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }
            return View(tenant);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TenantViewModel model)
        {
           
                var result = await _tenantService.CreateTenantAsync(model.UserName, model.Name, model.PhoneNumber, model.RoomNumber);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error creating tenant");
           
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }
            var model = new TenantViewModel
            {
                UserName = _tenantService.GetTenantEmailByUserIdAsync(tenant.UserId), 
                Name = tenant.Name,
                PhoneNumber = tenant.PhoneNumber,
                RoomNumber = tenant.RoomNumber,
               
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TenantViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _tenantService.UpdateTenantAsync(id,model.Name, model.PhoneNumber, model.RoomNumber);
                if (result)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Error updating tenant");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var tenant = await _tenantService.GetTenantByIdAsync(id);
            if (tenant == null)
            {
                return NotFound();
            }
            var model = new TenantViewModel
            {
                UserName = _tenantService.GetTenantEmailByUserIdAsync(tenant.UserId), // Assuming Email as UserName
                Name = tenant.Name,
                PhoneNumber = tenant.PhoneNumber,
                RoomNumber = tenant.RoomNumber
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TenantViewModel model)
        {
            var result = await _tenantService.DeleteTenantAsync(model.Id);
            if (result)
            {
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}
