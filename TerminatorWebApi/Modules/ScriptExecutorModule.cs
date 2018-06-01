using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;

namespace TerminatorWebApi.Modules
{
    public class ScriptExecutorModule : NancyModule
    {
        public ScriptExecutorModule(IScriptExecutor scriptExecutor)
        {

            Post["/api/script"] = _ =>
            {
                var x = Request.Body.AsString();
                var scriptQuery = this.Bind<ScriptQuery>();
                if (EmptyScript(scriptQuery))
                    return Negotiate
                        .WithStatusCode(HttpStatusCode.BadRequest)
                        .WithModel("Invalid powershell script");

                var scriptOutput = scriptExecutor.ExecutePowershell(scriptQuery.PowerShellScript);
                return Negotiate
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithContentType("application/json")
                    .WithModel(scriptOutput);
            };
        }

        private bool EmptyScript(ScriptQuery scriptQuery)
        {
            return string.IsNullOrWhiteSpace(scriptQuery?.PowerShellScript);
        }
    }
}