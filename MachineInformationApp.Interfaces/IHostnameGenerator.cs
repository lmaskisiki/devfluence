using MachineInformationApp;

namespace MachineInformationApp.Interfaces
{
    public interface IHostnameGenerator
    {
        ExecutionOutput GetHostName();
        ExecutionOutput GetFullQualifiedHostName();
    }
}