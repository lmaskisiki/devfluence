using System;
using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Extensions;
using Nancy.Responses;

namespace TerminatorWebApi
{
    public class ScriptExecutorModule : NancyModule
    {

        public ScriptExecutorModule(IScriptExecutor scriptExecutor)
        {
            Post["/script"] = _ =>
            {
                var filePath = this.Request.Body.AsString();
                var scriptOutput = scriptExecutor.ExecutePowershell(filePath);
                return Negotiate
                    .WithStatusCode(scriptOutput.StatusCode == 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError)
                    .WithModel(scriptOutput.Message);
            };
        }
    }
}