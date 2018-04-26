using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace TerminatorWebApi
{
    public class HealthApi : NancyModule
    {
        public HealthApi()
        {
            Get["/health"] = p => HttpStatusCode.OK;
        }
    }
}
