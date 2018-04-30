using MachineInformationApp.Interfaces;
using Nancy;
using System;

namespace TerminatorWebApi
{
    public class HostnameModule : NancyModule
    {
        public HostnameModule(IHostnameGenerator hostnameGenerator )
        {
            Get["/hostname"] = _ =>
            {
                try
                {
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
