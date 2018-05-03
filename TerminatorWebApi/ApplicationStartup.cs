using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses;

namespace TerminatorWebApi
{
    public class ApplicationStartup : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            pipelines.OnError += PipelinesOnError();
        }

        private Func<NancyContext, Exception, dynamic> PipelinesOnError()
        {
            return (ctx, ex) =>
            {
                return new Response
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Contents = stream => stream.Write(Encoding.UTF8.GetBytes(ex.Message), 0,
                        ex.Message.Length)
                };
            };
        }
    }
}
