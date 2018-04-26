using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;

namespace TerminatorWebApi
{
    public class WebApi : NancyModule
    {
        public WebApi()
        {
            Get["/"] = _ =>
            {
                var r = Request;
                return Negotiate.WithModel(Request.Url.IsSecure ? "Hello HTTPS" : "Hello HTTP");
            };
        }
    }
}
