using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            container.AutoRegister();
        }
    }
}
