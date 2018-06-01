using System.Collections.Generic;
using MachineInformationApp.Interfaces;

namespace MachineInformationApp.Interfaces
{
    public interface IDashboardDataService
    {
        IEnumerable<DashboardExecution> GetDashboardDetails();
        int  GetInsertDetails(DashboardExecution execution);
    }
}