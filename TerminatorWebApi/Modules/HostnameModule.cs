using MachineInformationApp.Interfaces;
using Nancy;

namespace TerminatorWebApi.Modules
{
    public class HostnameModule : NancyModule
    {
        public HostnameModule(IHostnameGenerator hostnameGenerator)
        {
            Get["/api/hostname"] = _ =>
            {
                if (!FullyQualifiedHostnameRequest(this.Request))
                    return Negotiate
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithContentType("application/json")
                        .WithModel(hostnameGenerator.GetHostName());

                return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithContentType("application/json")
                    .WithModel(hostnameGenerator.GetFullQualifiedHostName());
            };
        }

        private static bool FullyQualifiedHostnameRequest(Request request)
        {
            if (!request.Query["fully-qualified"]) return false;
            bool.TryParse(request.Query["fully-qualified"], out bool fullyQualified);
            return fullyQualified;
        }
    }
}
