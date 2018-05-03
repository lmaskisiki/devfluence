using System;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Extensions;

namespace TerminatorWebApi
{
    public class ScriptExecutorModule : NancyModule
    {
        public ScriptExecutorModule(IScriptExecutor scriptExecutor)
        {
            Post["/api/script"] = _ =>
            {
                var scriptText = this.Request.Body.AsString();
                if (EmptyScript(scriptText))
                    return HttpStatusCode.BadRequest;

                var scriptOutput = scriptExecutor.ExecutePowershell(scriptText);
                return Negotiate
                    .WithStatusCode(scriptOutput.StatusCode == 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError)
                    .WithContentType("application/json")
                    .WithModel(scriptOutput.Message);
 
            };
        }

        private bool EmptyScript(string scriptText)
        {
            return string.IsNullOrWhiteSpace(scriptText);
        }
    }
}