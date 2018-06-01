using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.ModelBinding;
using System;

namespace TerminatorWebApi.Modules
{
    public class AgentHistoryModule : NancyModule
    {
         private IAgentDataService _agentDataService;

        public AgentHistoryModule(IAgentDataService agentDataService)
        {
            this._agentDataService = agentDataService;
            Post["/api/agentHistory"] = p =>
            {
                AgentExecution agentExecution = this.Bind<AgentExecution>();
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