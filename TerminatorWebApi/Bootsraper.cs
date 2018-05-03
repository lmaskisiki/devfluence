using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.IO;
using System.Net;
using System.Text;

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
