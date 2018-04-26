 using MachineInformationApp.Interfaces;
 using Nancy;

namespace TerminatorWebApi
{
    public class HostnameModule : NancyModule
    {
        public HostnameModule(IHostnameGenerator hostnameGenerator)
        {
            Get["/hostname"] = _ =>
          Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithModel(hostnameGenerator.GetHostName());

        }
    }
}