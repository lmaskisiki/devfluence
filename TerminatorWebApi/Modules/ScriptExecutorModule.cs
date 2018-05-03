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
                var filePath = this.Request.Body.AsString();
                if (EmptyScript(filePath))
                    return HttpStatusCode.BadRequest;
                try
                {
                    var scriptOutput = scriptExecutor.ExecutePowershell(filePath);
                    return Negotiate
                        .WithStatusCode(scriptOutput.StatusCode == 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError)
                        .WithContentType("application/json")
                        .WithModel(scriptOutput.Message);
                }
                catch (Exception e)
                {
                    return Negotiate
                    .WithStatusCode(HttpStatusCode.InternalServerError)
                    .WithModel(e.Message);
                }

            };
        }

        private bool EmptyScript(string filePath)
        {
            return string.IsNullOrWhiteSpace(System.IO.File.ReadAllText(filePath));
        }
    }
}