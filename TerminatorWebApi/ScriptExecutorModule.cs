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
                try
                {
                    var filePath = this.Request.Body.AsString();
                    if (EmptyScript(filePath))
                        return HttpStatusCode.BadRequest;
                    var scriptOutput = scriptExecutor.ExecutePowershell(filePath);
                    return Negotiate
                        .WithStatusCode(scriptOutput.StatusCode == 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError)
                        .WithModel(scriptOutput.Message);
                }
                catch (Exception e)
                {
                    return HttpStatusCode.InternalServerError;
                }
            };
        }

        private bool EmptyScript(string filePath)
        {
            return string.IsNullOrWhiteSpace(System.IO.File.ReadAllText(filePath));
        }
    }
}