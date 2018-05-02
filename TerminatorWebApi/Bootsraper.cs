using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System.IO;
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

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.OnError += (context, exception) =>
           {
               if (exception is FileNotFoundException)
               {
                   return new Response
                   {
                       StatusCode = HttpStatusCode.InternalServerError,
                       ContentType = "text/html",
                       Contents = (stream) =>
                       {
                           var errorMessage = Encoding.UTF8.GetBytes(exception.Message);
                           stream.Write(errorMessage, 0, errorMessage.Length);
                       }
                   };
               }
               return new Response().StatusCode = HttpStatusCode.InternalServerError;
           };
        }

        //protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        //{
        //    pipelines.OnError += (context, exception) =>
        //    {
        //        if (exception is FileNotFoundException)
        //            return new Response()
        //            {
        //                StatusCode = HttpStatusCode.InternalServerError,
        //                Contents = stream =>
        //                {
        //                    stream.Write(Encoding.UTF8.GetBytes(exception.Message), 0, exception.Message.Length);
        //                }
        //            };
        //        return new Response().StatusCode = HttpStatusCode.InternalServerError;
        //    };
        //}​
    }
}
