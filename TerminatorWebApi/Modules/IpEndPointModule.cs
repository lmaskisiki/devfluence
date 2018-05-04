using MachineInformationApp.Interfaces;
using Nancy;

namespace TerminatorWebApi.Modules
{
    public class IpendpointModule : NancyModule
    {
        public IpendpointModule(IIpAddressGenerator ipAddressGenerator)
        {
            Get["/api/ip"] = _ => Negotiate
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentType("application/json")
                .WithModel(ipAddressGenerator.GetIpAddress());
        }
    }
}