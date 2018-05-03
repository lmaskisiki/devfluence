using MachineInformationApp.Interfaces;
using Nancy;
using System;

namespace TerminatorWebApi
{
    public class HostnameModule : NancyModule
    {
        public HostnameModule(IHostnameGenerator hostnameGenerator, IFullqualifiedHostnameGenerator fullqualifiedHostnameGenerator)
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
                    .WithModel(fullqualifiedHostnameGenerator.GetFullQualifiedHostName());
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
