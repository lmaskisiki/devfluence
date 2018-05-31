using MachineInformationApp;
using MachineInformationApp.Interfaces;
using Nancy;
using Nancy.ModelBinding;

namespace TerminatorWebApi.Tests.Tests
{
    public class AgentHistoryModule : NancyModule
    {

        public AgentHistoryModule(IAgentDataService agentDataService)
        {
            Post["/api/agentHistory"] = p =>
            {

                AgentExecution agentExecution = this.Bind<AgentExecution>();
                if (agentExecution == null) return HttpStatusCode.BadRequest;

                agentDataService.GetInsertedData(agentExecution);
                return HttpStatusCode.OK;
            };

            Get["/api/agentHistory"] = p =>
            {

                var x = agentDataService.GetExecutedListAgentDetails();
                return Negotiate
                .WithContentType("aplication/json")
                .WithModel(x);
            };
        }
    }
}