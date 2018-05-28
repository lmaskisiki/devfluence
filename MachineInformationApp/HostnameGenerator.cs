using System.Net;
using MachineInformationApp.Interfaces;

namespace MachineInformationApp
{
    public class HostnameGenerator : IHostnameGenerator
    {
        public ExecutionOutput GetHostName()
        {
            return new ExecutionOutput {result = Dns.GetHostName()};
        }

        public ExecutionOutput GetFullQualifiedHostName()
        {
            var hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            return new ExecutionOutput { result = hostEntry.HostName };
        }
    }
}