using System;

namespace MachineInformationApp.Interfaces
{
    public class DashboardExecution
    {
        public int ID { get; set; }
        public string Target { get; set; }
        public string Action { get; set; }
        public string ActionResult { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}
