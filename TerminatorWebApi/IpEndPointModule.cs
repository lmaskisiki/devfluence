using MachineInformationApp.Interfaces;
using Nancy;

namespace TerminatorWebApi
{
    public class IpendpointModule : NancyModule
    {
        private readonly IIpAddressGenerator _ipAddressGenerator;
        
        public IpendpointModule(IIpAddressGenerator reIpAddressGenerator)
        {
            _ipAddressGenerator = reIpAddressGenerator;

            Get["/ip"] = _ => Negotiate.WithStatusCode(HttpStatusCode.OK)
                   .WithModel(reIpAddressGenerator.GetIpAddress());

        }
    }
}