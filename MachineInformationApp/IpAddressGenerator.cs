using System.Net;
using System.Net.Sockets;
using MachineInformationApp.Interfaces;

namespace MachineInformationApp
{

    public class IpAddressGenerator : IIpAddressGenerator
    {
        public ExecutionOutput GetIpAddress()
        {
            var ipAddress = Dns.GetHostByName(Dns.GetHostName()).AddressList[0];
            return new ExecutionOutput {Output = ipAddress.ToString()};
        }
    }
}