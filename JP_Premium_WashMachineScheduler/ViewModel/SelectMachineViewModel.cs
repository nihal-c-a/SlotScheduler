using System.Collections.Generic;

namespace JP_Premium_WashMachineScheduler.Models
{
    public class SelectMachineViewModel
    {
        public List<WashingMachine> WashingMachines { get; set; }
        public int SelectedMachineId { get; set; }
    }
}
