using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MachineInformationApp;
using Nancy.Hosting.Self;

namespace TerminatorWebApi
{
    class Program
    {
        static void Main(string[] args)
        {
            HostConfiguration hostConfigs = new HostConfiguration();
             
            hostConfigs.UrlReservations.CreateAutomatically = true;
            Uri uri = new Uri("http://localhost:1234");
            var host = new NancyHost(hostConfigs, uri);
            host.Start();
            Console.ReadKey();
        }
    }
}
