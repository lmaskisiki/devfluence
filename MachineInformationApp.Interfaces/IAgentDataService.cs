using System.Collections.Generic;
 
namespace MachineInformationApp.Interfaces
{
    public interface IAgentDataService
    {
        IEnumerable<AgentExecution> GetExecutedAgentDetails();
        IEnumerable<AgentExecution> GetExecutedListAgentDetails();
        int GetInsertedData(AgentExecution agent);
    }
}