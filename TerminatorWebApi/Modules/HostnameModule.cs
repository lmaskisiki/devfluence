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
                try
                {
                    if (!FullyQualifiedHostnameRequest(this.Request))
                        return Negotiate
                            .WithStatusCode(HttpStatusCode.OK)
                            .WithContentType("xml")
                            .WithModel(hostnameGenerator.GetHostName());

                    return Negotiate
                        .WithStatusCode(HttpStatusCode.OK)
                        .WithContentType("xml")
                        .WithModel(fullqualifiedHostnameGenerator.GetFullQualifiedHostName());
                }
                catch (Exception)
                {
                    return HttpStatusCode.InternalServerError;
                }
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
