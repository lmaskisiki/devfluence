using MachineInformationApp.Interfaces;
using Nancy;
using System;

namespace TerminatorWebApi
{
    public class IpendpointModule : NancyModule
    {
        public IpendpointModule(IIpAddressGenerator ipAddressGenerator)
        {
            Get["/api/ip"] = _ =>
            {
                try
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.OK)
                   .WithModel(ipAddressGenerator.GetIpAddress());
                }
                catch(Exception)
                {
                    return HttpStatusCode.InternalServerError;
                }
            };
        }
    }
}