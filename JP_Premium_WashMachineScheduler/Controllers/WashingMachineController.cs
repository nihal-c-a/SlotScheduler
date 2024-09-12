using JP_Premium_WashMachineScheduler.Models;
using JP_Premium_WashMachineScheduler.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JP_Premium_WashMachineScheduler.Controllers
{
    public class WashingMachineController : Controller
    {
        private readonly IWashingMachineService _washingMachineService;

        public WashingMachineController(IWashingMachineService washingMachineService)
        {
            _washingMachineService = washingMachineService;
        }

        // GET: WashingMachine
        public async Task<IActionResult> Index()
        {
            var machines = await _washingMachineService.GetAvailableMachinesAsync();
            return View(machines);
        }

        // GET: WashingMachine/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var machine = await _washingMachineService.GetMachineByIdAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            return View(machine);
        }

        // GET: WashingMachine/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WashingMachine/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MachineName")] WashingMachine machine)
        {
            if (ModelState.IsValid)
            {
                await _washingMachineService.AddWashingMachineAsync(machine.MachineName);
                return RedirectToAction(nameof(Index));
            }
            return View(machine);
        }

        // GET: WashingMachine/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var machine = await _washingMachineService.GetMachineByIdAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            return View(machine);
        }

        // POST: WashingMachine/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MachineId,MachineName,IsAvailable")] WashingMachine machine)
        {
            if (id != machine.MachineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _washingMachineService.UpdateMachineAsync(machine);
                return RedirectToAction(nameof(Index));
            }
            return View(machine);
        }

        // GET: WashingMachine/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var machine = await _washingMachineService.GetMachineByIdAsync(id);
            if (machine == null)
            {
                return NotFound();
            }
            return View(machine);
        }

        // POST: WashingMachine/Delete/5
       
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _washingMachineService.DeleteMachineAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
