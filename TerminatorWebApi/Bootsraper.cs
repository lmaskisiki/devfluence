using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.TinyIoc;

namespace TerminatorWebApi
{
    public class Bootsraper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register<IIpAddressGenerator, IpAddressGenerator>();
            container.Register<IFullqualifiedHostnameGenerator, FullqualifiedHostnameGenerator>();
            container.Register<IHostnameGenerator, HostnameGenerator>();
            container.Register<IOSGenerator, OSGenerator>();
            container.Register<IScriptExecutor, ScriptExecutor>();

        }
    }
}
