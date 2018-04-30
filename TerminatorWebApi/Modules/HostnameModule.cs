using MachineInformationApp.Interfaces;
using Nancy;
using System;

namespace TerminatorWebApi
{
    public class HostnameModule : NancyModule
    {
        public HostnameModule(IHostnameGenerator hostnameGenerator, IFullqualifiedHostnameGenerator fullqualifiedHostnameGenerator)
        {
            Get["/api/hostname"] = param =>
            {
                try
                {
                    if (this.Request.Query["fully-qualified"])
                    {
                        var fullyQualified = false;
                        Boolean.TryParse(this.Request.Query["fully-qualified"], out fullyQualified);
                        if (fullyQualified)
                        {
                            var result = fullqualifiedHostnameGenerator.GetFullQualifiedHostName();
                            return Negotiate
                            .WithStatusCode(HttpStatusCode.OK)
                            .WithModel(result);
                        }
                    }
                    return Negotiate
                   .WithStatusCode(HttpStatusCode.OK)
                   .WithModel(hostnameGenerator.GetHostName());
                }
                catch (Exception)
                {
                    return HttpStatusCode.InternalServerError;
                }
            };
        }
    }
}
