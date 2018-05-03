using MachineInformationApp.Interfaces;
using Nancy;
using System;
using System.IO;

namespace TerminatorWebApi
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