using System;

namespace MachineInformationApp.Interfaces
{
    public class AgentExecution
    {
        public int ExecutionId { get; set; }
        public string Command { get; set; }
        public string Result { get; set; }
        public DateTime ExecutionTime{get;set;}
    }
}