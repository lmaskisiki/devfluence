using System;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Newtonsoft.Json;

namespace TerminatorWebApi
{
    public class ScriptExecutorModule : NancyModule
    {
        public ScriptExecutorModule(IScriptExecutor scriptExecutor)
        {
            Post["/api/script"] = _ =>
            {
                var scriptQuery = this.Bind<ScriptQuery>();
                if (EmptyScript(scriptQuery))
                    return Negotiate
                        .WithStatusCode(HttpStatusCode.BadRequest)
                        .WithModel("Invalid powershell script");

                var scriptOutput = scriptExecutor.ExecutePowershell(scriptQuery.Script);
                return Negotiate
                    .WithStatusCode(scriptOutput.StatusCode == 0 ? HttpStatusCode.OK : HttpStatusCode.InternalServerError)
                    .WithContentType("application/json")
                    .WithModel(scriptOutput);
            };
        }

        private bool EmptyScript(ScriptQuery scriptQuery)
        {
            if (scriptQuery != null)
            {
                return string.IsNullOrWhiteSpace(scriptQuery.Script);
            }

            return true;
        }
    }
}