using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInformationApp;
using Nancy;

namespace TerminatorWebApi
{
    public class OSEndpointModule:NancyModule
    {
        public OSEndpointModule(IOSGenerator osGenerator)
        {
            Get["/os"]=_ =>  Negotiate.WithStatusCode(HttpStatusCode.OK)
                .WithModel(osGenerator.GetOsVersion());
        }
    }
}
