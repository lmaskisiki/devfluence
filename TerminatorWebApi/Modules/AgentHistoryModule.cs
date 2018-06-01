using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.Extensions;
using Nancy.ModelBinding;
using Newtonsoft.Json;
using System;

namespace TerminatorWebApi.Modules
{
    public class AgentHistoryModule : NancyModule
    {
         private IAgentDataService _agentDataService;

        public AgentHistoryModule(IAgentDataService agentDataService)
        {
            this._agentDataService = agentDataService;
            Post["/api/agentHistory"] = _ =>
            {
                var x = this.Request.Body.AsString();
                var agentExecution = JsonConvert.DeserializeObject<AgentExecution>(x);
                var scriptQuery = this.Bind<ScriptQuery>();

                if (agentExecution == null) return HttpStatusCode.BadRequest;
                agentExecution.ExecutionTime = DateTime.UtcNow;
                _agentDataService.GetInsertedData(agentExecution);
                return HttpStatusCode.OK;
            };

            Get["/api/agentHistory"] = p =>
            {
                var x = _agentDataService.GetExecutedListAgentDetails();
                return Negotiate
                .WithContentType("aplication/json")
                .WithModel(x);
            };
        }
    }
}