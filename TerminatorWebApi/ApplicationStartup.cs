using System;
using System.Text;
using Nancy;
using Nancy.Bootstrapper;

namespace TerminatorWebApi
{
    public class ApplicationStartup : IApplicationStartup
    {
        public void Initialize(IPipelines pipelines)
        {
            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) => ctx.Response
            .WithHeader("Access-Control-Allow-Origin", "*")
            .WithHeader("Access-Control-Allow-Methods", "POST,GET")
            .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type"));
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
