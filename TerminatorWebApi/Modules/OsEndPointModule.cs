using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;

namespace TerminatorWebApi.Modules
{
    public class OsEndpointModule : NancyModule
    {
        public OsEndpointModule(IOSGenerator osGenerator)
        {
            Get["/api/os"] = _ => Negotiate
                .WithStatusCode(HttpStatusCode.OK)
                .WithContentType("application/json")
                .WithModel(osGenerator.GetOsVersion());
        }
    }
}
