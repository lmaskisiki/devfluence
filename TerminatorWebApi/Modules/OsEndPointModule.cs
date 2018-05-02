using MachineInformationApp;
using Nancy;

namespace TerminatorWebApi
{
    public class OSEndpointModule : NancyModule
    {
        public OSEndpointModule(IOSGenerator osGenerator)
        {
            Get["/api/os"] = _ =>
            {
                try
                {
                    return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithContentType("application/json")
                    .WithModel(osGenerator.GetOsVersion());
                }
                catch
                {
                    return HttpStatusCode.InternalServerError;
                }
            };
        }
    }
}
