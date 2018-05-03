using MachineInformationApp;
using Nancy;
using System;

namespace TerminatorWebApi
{
    public class OSEndpointModule : NancyModule
    {
        public OSEndpointModule(IOSGenerator osGenerator)
        {
            Get["/api/os"] = _ => Negotiate
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentType("application/json")
                .WithModel(osGenerator.GetOsVersion());
        }
    }
}
